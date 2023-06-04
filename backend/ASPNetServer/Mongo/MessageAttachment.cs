// (c) 2023 Dan Saul
using GraphQL.AspNet.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ASPNetServer.Mongo
{
	[BsonIgnoreExtraElements]
	public class MessageAttachment
	{
		
		[GraphField("id")]
		public string ApolloId
		{
			get
			{
				return $"{Key}";
			}
		}
		[JsonProperty]
		public string? Key { get; init; }
		[JsonProperty]
		public string? MediaType { get; init; }
		[JsonProperty]
		public int? ImageWidthPx { get; set; }
		[JsonProperty]
		public int? ImageHeightPx { get; set; }
		[JsonProperty]
		[GraphField("uri")]
		public string? URI
		{
			get
			{
				if (string.IsNullOrEmpty(Key)
					)
					return null;
				string key = System.Web.HttpUtility.UrlEncode(Key);
				return $"/api/Attachment/{key}";
			}
		}
	}
}
