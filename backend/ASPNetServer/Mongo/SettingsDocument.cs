// (c) 2023 Dan Saul
using GraphQL.AspNet.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ASPNetServer.Mongo
{
	[BsonIgnoreExtraElements]
	public class SettingsDocument
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

		public string? SendFromE164 { get; set; }
	}
}
