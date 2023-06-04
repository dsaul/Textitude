// (c) 2023 Dan Saul
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using MongoDB.Driver;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace Textitude.Webhook
{
	[Route("api/[controller]")]
	[ApiController]
	public class TwilioDeliveryStatusController : ControllerBase
	{
		private IMongoClient Mongo { get; set; }

		public TwilioDeliveryStatusController(
			IMongoClient mongo
			)
		{
			Mongo = mongo;
		}




		[HttpPost]
		public IActionResult Post(

			[FromForm] string? AccountSid = null,
			[FromForm] string? From = null,
			[FromForm] string? MessageSid = null,
			[FromForm] string? MessageStatus = null,
			[FromForm] string? SmsSid = null,
			[FromForm] string? SmsStatus = null

			)
		{
			//var message = new Message
			//{
			//	TwilioToCountry = ToCountry,
			//	TwilioToState = ToState,
			//	TwilioSmsMessageSid = SmsMessageSid,
			//	TwilioNumMedia = NumMedia,
			//	TwilioToCity = ToCity,
			//	TwilioFromZip = FromZip,
			//	TwilioSmsSid = SmsSid,
			//	TwilioFromState = FromState,
			//	TwilioSmsStatus = SmsStatus,
			//	TwilioFromCity = FromCity,
			//	Body = Body,
			//	TwilioFromCountry = FromCountry,
			//	To = To,
			//	TwilioMessagingServiceSid = MessagingServiceSid,
			//	TwilioToZip = ToZip,
			//	TwilioNumSegments = NumSegments,
			//	TwilioReferralNumMedia = ReferralNumMedia,
			//	TwilioMessageSid = MessageSid,
			//	TwilioAccountSid = AccountSid,
			//	From = From,
			//	TwilioApiVersion = ApiVersion,
			//	BackendService = Message.kBackendServiceTwilio,
			//};


			//Messages.InsertOne(message);

			var response2 = new MessagingResponse();
			//response2.Message("Hello from a .NET Minimal API!");
			string s = response2.ToString();

			return Content(s, "text/xml");





		}








	}
}
