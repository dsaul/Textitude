// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;
using Textitude.Spam;

namespace ASPNetServer.GraphQL
{
	[GraphRoute("messages")]
	public partial class Messages : GraphController
	{
		[GraphSkip]
		IMongoCollection<MessageDocument> MessageDocuments { get; init; }
		[GraphSkip]
		SpamFilterController SpamFilterController { get; init; }

		public Messages(IMongoCollection<MessageDocument> _MessageDocuments, SpamFilterController _SpamFilterController)
		{
			MessageDocuments = _MessageDocuments;
			SpamFilterController = _SpamFilterController;
		}


	}
}
