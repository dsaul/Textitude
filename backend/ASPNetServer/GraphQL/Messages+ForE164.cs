// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;

namespace ASPNetServer.GraphQL
{
	public partial class Messages : GraphController
	{
		[Query("forE164")]
		public IEnumerable<MessageDocument> ForE164(string e164)
		{
			return (from message in MessageDocuments.AsQueryable()
					orderby message.ISO8601Timestamp ascending
					where message.To == e164 || message.From == e164
					select message);
		}
	}
}
