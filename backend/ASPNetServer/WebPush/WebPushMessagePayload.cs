using ASPNetServer.Mongo;
using Newtonsoft.Json;

namespace ASPNetServer.WebPush
{
	public class WebPushMessagePayload
	{
		public static WebPushMessagePayload ForMessage(MessageDocument payload)
		{
			WebPushMessagePayload ret = new();
			ret.MessageFrom = payload.From;
			ret.MessageBody = payload.Body;
			foreach (MessageAttachment attachment in payload.Attachments)
			{
				ret.Attachments.Add(new WebPushMessagePayloadAttachment()
				{
					URI = attachment.URI,
				});
			}
			return ret;
		}

		public string Serialize()
		{
			return JsonConvert.SerializeObject(this);
		}

		public string? MessageFrom { get; set; }
		public string? MessageBody { get; set; }
		public List<WebPushMessagePayloadAttachment> Attachments { get; set; } = new();
	}
}
