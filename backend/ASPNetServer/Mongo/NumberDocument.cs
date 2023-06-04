// (c) 2023 Dan Saul
using GraphQL.AspNet.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ASPNetServer.Mongo
{
	[BsonIgnoreExtraElements]
	public class NumberDocument
	{
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

		[JsonProperty]
		public bool? IsOwned { get; set; }
		[JsonProperty]
		public string? E164 { get; set; }
		[JsonProperty]
		public bool? CanSend { get; set; }
		[JsonProperty]
		public bool? IsFallback { get; set; }
	}
}
