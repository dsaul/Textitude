// (c) 2023 Dan Saul
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using GraphQL.AspNet.Attributes;

namespace ASPNetServer.Mongo
{
	[BsonIgnoreExtraElements]
	public class WebPushSubscriptionDocument
	{
		[BsonIgnoreExtraElements]
		public class KeysImpl
		{
			[BsonElement("p256dh")]
			public string? P256dh { get; set; }
			[BsonElement("auth")]
			public string? Auth { get; set; }
		}

		
		public string Id
		{
			get
			{
				return MongoId.ToString();
			}
		}
		[BsonId]
		public ObjectId MongoId { get; set; }

		

		[BsonElement("endpoint")]
		public string? Endpoint { get; set; }

		[BsonElement("expirationTime")]
		public string? ExpirationTime { get; set; }

		[BsonElement("keys")]
		public KeysImpl Keys { get; set; } = new KeysImpl();


	}
}
