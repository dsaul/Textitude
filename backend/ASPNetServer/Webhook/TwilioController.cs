// (c) 2023 Dan Saul
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using Twilio.Http;
using MongoDB.Driver;
using Textitude.Spam;
using nBayes;
using Amazon.S3;
using Amazon.S3.Model;
using NodaTime.Text;
using NodaTime;
using NodaTime.Extensions;
using Serilog;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using ASPNetServer.Mongo;

namespace Textitude.Webhook
{
	[ApiController]
	[Route("[controller]")]
	public class TwilioController : ControllerBase
	{
		System.Net.Http.HttpClient HttpClient { get; init; }
		IMongoCollection<MessageDocument> Messages { get; set; }
		SpamFilterController SpamFilterController { get; set; }
		AmazonS3Client S3Client { get; init; }

		public TwilioController(
			SpamFilterController _SpamFilterController,
			IMongoCollection<MessageDocument> _Messages,
			System.Net.Http.HttpClient _HttpClient,
			AmazonS3Client _S3Client
			)
		{
			HttpClient = _HttpClient;
			Messages = _Messages;
			SpamFilterController = _SpamFilterController;
			S3Client = _S3Client;
		}

		static void GetSize(TemporaryFile file, out int? imageWidthPx, out int? imageHeightPx)
		{
			using Image image = Image.Load(file.FilePath);
			imageWidthPx = image.Width;
			imageHeightPx = image.Height;
		}

		async Task<MessageAttachment?> DownloadAndStoreMMSAttachment(string? from, string? mediaURL)
		{
			if (string.IsNullOrWhiteSpace(from))
				return null;
			if (string.IsNullOrWhiteSpace(mediaURL))
				return null;

			using HttpResponseMessage response = await HttpClient.GetAsync(mediaURL);

			if (!response.IsSuccessStatusCode)
				return null;

			HttpContent content = response.Content;
			string? mediaType = content.Headers.ContentType?.MediaType;
			if (mediaType == null)
				return null;

			
			using TemporaryFile tmpFile = new(await response.Content.ReadAsStreamAsync());
			
			int? imageWidthPx = null;
			int? imageHeightPx = null;
			switch (mediaType)
			{
				case "image/jpeg":
				case "image/jpg":
				case "image/gif":
				case "image/png":
				case "image/bmp":
				case "image/tiff":
				{
					
					GetSize(tmpFile, out imageWidthPx, out imageHeightPx);
					break;
				}
				default:
				{
					if (mediaType.StartsWith("image/"))
					{
						using Stream metadataStream = await response.Content.ReadAsStreamAsync();
						Log.Information("[{Class}.{Method}()] {Message} Unknown image type {ImageType}, trying to process it anyway.",
							typeof(TwilioController).Name,
							System.Reflection.MethodBase.GetCurrentMethod()?.Name,
							mediaType
						);

						try
						{
							GetSize(tmpFile, out imageWidthPx, out imageHeightPx);
						}
						catch (Exception ex)
						{
							Log.Error(ex, "[{Class}.{Method}()] {Message} Error while trying to decode unknown image type {ImageType}.",
								typeof(TwilioController).Name,
								System.Reflection.MethodBase.GetCurrentMethod()?.Name,
								ex.Message,
								mediaType
							);
						}
					}
					break;
				}
			}

			// https://www.twilio.com/docs/sms/accepted-mime-types
			string? extension = null;
			switch (mediaType)
			{
				case "image/jpeg":
				case "image/jpg":
					extension = "jpg";
					break;
				case "image/gif":
					extension = "gif";
					break;
				case "image/png":
					extension = "png";
					break;
				case "image/bmp":
					extension = "bmp";
					break;
				case "image/tiff":
					extension = "tif";
					break;
				case "audio/basic":
					extension = "au";
					break;
				case "audio/L24":
					extension = "L24";  // no idea what this is
					break;
				case "audio/mp4":
					extension = "mp4";
					break;
				case "audio/mpeg":
					extension = "mpeg";
					break;
				case "audio/ogg":
					extension = "ogg";
					break;
				case "audio/vnd.rn-realaudio":
					extension = "ra";
					break;
				case "audio/vnd.wave":
					extension = "wav";
					break;
				case "audio/3gpp":
					extension = "3gpp";
					break;
				case "audio/3gpp2":
					extension = "3gpp2"; 
					break;
				case "audio/ac3":
					extension = "ac3";
					break;
				case "audio/webm":
					extension = "webm";
					break;
				case "audio/amr-nb":
					extension = "amrnb"; // no idea what this is
					break;
				case "audio/amr":
					extension = "amr";
					break;
				case "video/mpeg":
					extension = "mpeg";
					break;
				case "video/mp4":
					extension = "mp4";
					break;
				case "video/quicktime":
					extension = "mov";
					break;
				case "video/webm":
					extension = "webm";
					break;
				case "video/3gpp":
					extension = "3gpp";
					break;
				case "video/3gpp2":
					extension = "3gpp2";
					break;
				case "video/3gpp-tt":
					extension = "3gpptt";
					break;
				case "video/H261":
					extension = "H261"; // not familiar enough
					break;
				case "video/H263":
					extension = "H263"; // not familiar enough
					break;
				case "video/H263-1998":
					extension = "H2631998"; // not familiar enough
					break;
				case "video/H263-2000":
					extension = "H2632000"; // not familiar enough
					break;
				case "video/H264":
					extension = "H264"; // isn't this in a different container?
					break;
				case "text/vcard":
					extension = "vcard";
					break;
				case "text/x-vcard":
					extension = "vcard";
					break;
				case "text/csv":
					extension = "csv";
					break;
				case "text/rtf":
					extension = "rtf";
					break;
				case "text/richtext":
					extension = "rtf";
					break;
				case "text/calendar":
					extension = "ical";
					break;
				case "text/directory":
					extension = "txt";
					break;
				case "application/pdf":
					extension = "pdf";
					break;
				case "application/vcard":
					extension = "vcard";
					break;
				case "application/msword":
					extension = "doc";
					break;
				case "application/vnd.ms-excel":
					extension = "xls";
					break;
				case "application/vnd.ms-powerpoint":
					extension = "ppt";
					break;
				case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
					extension = "pptx";
					break;
				case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
					extension = "xlsx";
					break;
				case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
					extension = "docx";
					break;
				case "audio/aac":
					extension = "aac";
					break;
				case "audio/ogg; codecs=opus":
					extension = "ogg";
					break;
				case "image/webp":
					extension = "webp";
					break;
			}

			string timestamp = OffsetDateTimePattern.GeneralIso.Format(SystemClock.Instance.InUtc().GetCurrentOffsetDateTime());
			string s3Key = Regex.Replace(
				$"{from}/{timestamp}_{DateTime.Now.Millisecond}.{extension}", 
				"[^a-zA-Z0-9./]", 
				"_"
			);

			using Stream inputStream = tmpFile.OpenRead();

			PutObjectRequest request = new()
			{
				BucketName = EnvAmazonS3.S3_BUCKET_MMS,
				Key = s3Key,
				ContentType = mediaType,
				InputStream = inputStream
			};

			await S3Client.PutObjectAsync(request);


			return new MessageAttachment {
				Key = s3Key,
				MediaType = mediaType,
				ImageHeightPx = imageHeightPx,
				ImageWidthPx = imageWidthPx,
			};

		}


		[HttpPost]
		public async Task<IActionResult> Post(

			[FromForm] string? ToCountry = null,
			[FromForm] string? ToState = null,
			[FromForm] string? SmsMessageSid = null,
			[FromForm] string? NumMedia = null,
			[FromForm] string? ToCity = null,
			[FromForm] string? FromZip = null,
			[FromForm] string? SmsSid = null,
			[FromForm] string? FromState = null,
			[FromForm] string? SmsStatus = null,
			[FromForm] string? FromCity = null,
			[FromForm] string? Body = null,
			[FromForm] string? FromCountry = null,
			[FromForm] string? To = null,
			[FromForm] string? MessagingServiceSid = null,
			[FromForm] string? ToZip = null,
			[FromForm] string? NumSegments = null,
			[FromForm] string? ReferralNumMedia = null,
			[FromForm] string? MessageSid = null,
			[FromForm] string? AccountSid = null,
			[FromForm] string? From = null,
			[FromForm] string? ApiVersion = null,
			[FromForm] string? MediaUrl0 = null,
			[FromForm] string? MediaUrl1 = null,
			[FromForm] string? MediaUrl2 = null,
			[FromForm] string? MediaUrl3 = null,
			[FromForm] string? MediaUrl4 = null,
			[FromForm] string? MediaUrl5 = null,
			[FromForm] string? MediaUrl6 = null,
			[FromForm] string? MediaUrl7 = null,
			[FromForm] string? MediaUrl8 = null,
			[FromForm] string? MediaUrl9 = null
			)
		{


			bool? isAIDetectedSpam = null;
			if (false == string.IsNullOrWhiteSpace(Body))
			{
				CategorizationResult result = SpamFilterController.Analyze(Body);
				switch (result)
				{
					case CategorizationResult.First:
						isAIDetectedSpam = true;
						break;
					case CategorizationResult.Undetermined:
						break;
					case CategorizationResult.Second:
						isAIDetectedSpam = false;
						break;
				}
			}


			// Download and store attachment.
			List<MessageAttachment> attachments = new();
			if (NumMedia != null && int.TryParse(NumMedia, out int parsedNumMedia) && parsedNumMedia > 0)
			{
				List<Task<MessageAttachment?>> tasks = new()
				{
					DownloadAndStoreMMSAttachment(From, MediaUrl0),
					DownloadAndStoreMMSAttachment(From, MediaUrl1),
					DownloadAndStoreMMSAttachment(From, MediaUrl2),
					DownloadAndStoreMMSAttachment(From, MediaUrl3),
					DownloadAndStoreMMSAttachment(From, MediaUrl4),
					DownloadAndStoreMMSAttachment(From, MediaUrl5),
					DownloadAndStoreMMSAttachment(From, MediaUrl6),
					DownloadAndStoreMMSAttachment(From, MediaUrl7),
					DownloadAndStoreMMSAttachment(From, MediaUrl8),
					DownloadAndStoreMMSAttachment(From, MediaUrl9)
				};

				await Task.WhenAll(tasks);

				foreach (Task<MessageAttachment?> task in tasks)
				{
					MessageAttachment? res = task.Result;
					if (res != null)
					{
						attachments.Add(res);
					}
				}
			}


			var message = new MessageDocument
			{
				TwilioToCountry = ToCountry,
				TwilioToState = ToState,
				TwilioSmsMessageSid = SmsMessageSid,
				TwilioNumMedia = NumMedia,
				TwilioToCity = ToCity,
				TwilioFromZip = FromZip,
				TwilioSmsSid = SmsSid,
				TwilioFromState = FromState,
				TwilioSmsStatus = SmsStatus,
				TwilioFromCity = FromCity,
				Body = Body,
				TwilioFromCountry = FromCountry,
				To = To,
				TwilioMessagingServiceSid = MessagingServiceSid,
				TwilioToZip = ToZip,
				TwilioNumSegments = NumSegments,
				TwilioReferralNumMedia = ReferralNumMedia,
				TwilioMessageSid = MessageSid,
				TwilioAccountSid = AccountSid,
				From = From,
				TwilioApiVersion = ApiVersion,
				BackendService = MessageDocument.kBackendServiceTwilio,
				IsAIDetectedSpam = isAIDetectedSpam,
				TwilioMediaUrl0 = MediaUrl0,
				TwilioMediaUrl1 = MediaUrl1,
				TwilioMediaUrl2 = MediaUrl2,
				TwilioMediaUrl3 = MediaUrl3,
				TwilioMediaUrl4 = MediaUrl4,
				TwilioMediaUrl5 = MediaUrl5,
				TwilioMediaUrl6 = MediaUrl6,
				TwilioMediaUrl7 = MediaUrl7,
				TwilioMediaUrl8 = MediaUrl8,
				TwilioMediaUrl9 = MediaUrl9,

				Attachments = attachments,
			};


			Messages.InsertOne(message);
			

			var response2 = new MessagingResponse();
			//response2.Message("Hello from a .NET Minimal API!");
			string s = response2.ToString();

			return Content(s, "text/xml");



		}
	}
}
