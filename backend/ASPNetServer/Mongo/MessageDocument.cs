// (c) 2023 Dan Saul
using GraphQL.AspNet.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;

namespace ASPNetServer.Mongo
{
	[BsonIgnoreExtraElements]
	public class MessageDocument
	{
		public const string kBackendServiceTwilio = "Twilio";

		[BsonId]
		[GraphSkip]
		public ObjectId Id { get; set; }

		[GraphField("id")]
		public string ApolloId
		{
			get
			{
				return Id.ToString();
			}
		}


		// Twilio
		[JsonProperty]
		public string? TwilioToCountry { get; set; }
		[JsonProperty]
		public string? TwilioToState { get; set; }
		[JsonProperty]
		public string? TwilioSmsMessageSid { get; set; }
		[JsonProperty]
		public string? TwilioNumMedia { get; set; }
		[JsonProperty]
		public string? TwilioToCity { get; set; }
		[JsonProperty]
		public string? TwilioFromZip { get; set; }
		[JsonProperty]
		public string? TwilioSmsSid { get; set; }
		[JsonProperty]
		public string? TwilioFromState { get; set; }
		[JsonProperty]
		public string? TwilioSmsStatus { get; set; }
		[JsonProperty]
		public string? TwilioFromCity { get; set; }
		[JsonProperty]
		public string? TwilioFromCountry { get; set; }
		[JsonProperty]
		public string? TwilioMessagingServiceSid { get; set; }
		[JsonProperty]
		public string? TwilioToZip { get; set; }
		[JsonProperty]
		public string? TwilioNumSegments { get; set; }
		[JsonProperty]
		public string? TwilioReferralNumMedia { get; set; }
		[JsonProperty]
		public string? TwilioMessageSid { get; set; }
		[JsonProperty]
		public string? TwilioAccountSid { get; set; }
		[JsonProperty]
		public string? TwilioURI { get; set; }
		[JsonProperty]
		public string? TwilioApiVersion { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl0 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl1 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl2 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl3 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl4 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl5 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl6 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl7 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl8 { get; set; }
		[JsonProperty]
		public string? TwilioMediaUrl9 { get; set; }

		// General
		[JsonProperty]
		[GraphField("iso8601Timestamp")]
		public string? ISO8601Timestamp { get; set; } = OffsetDateTimePattern.GeneralIso.Format(SystemClock.Instance.InUtc().GetCurrentOffsetDateTime());
		[JsonIgnore]
		[GraphSkip]
		[BsonIgnore]
		public OffsetDateTime? Timestamp { 
			get
			{
				if (string.IsNullOrWhiteSpace(ISO8601Timestamp))
					return null;
				var parseResult = OffsetDateTimePattern.GeneralIso.Parse(ISO8601Timestamp);
				if (parseResult.Success == false)
					return null;
				return parseResult.Value;
			}
		}
		[JsonProperty]
		public string? Price { get; set; }
		[JsonProperty]
		public string? From { get; set; }
		[JsonProperty]
		public string? ErrorMessage { get; set; }
		[JsonProperty]
		public bool? IsAIDetectedSpam { get; set; }
		[JsonProperty]
		public bool? IsHumanConfirmedSpam { get; set; }
		[JsonProperty]
		public string? BackendService { get; set; }
		[JsonProperty]
		public string? Body { get; set; }
		[JsonProperty]
		public string? To { get; set; }
		[JsonProperty]
		public bool IsRead { get; set; } = false;
		[JsonProperty]
		public List<MessageAttachment> Attachments { get; init; } = new List<MessageAttachment>();

		[BsonIgnore]
		[JsonProperty]
		public bool? IsSpam
		{
			get
			{
				if (null != IsHumanConfirmedSpam)
				{
					return IsHumanConfirmedSpam.Value;
				}
				else if (null != IsAIDetectedSpam)
				{
					return IsAIDetectedSpam.Value;
				}
				return null;
			}
		}

	}
}
