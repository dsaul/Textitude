// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;

namespace ASPNetServer.GraphQL
{
	[GraphRoute("numbers")]
	public partial class Numbers : GraphController
	{
		IMongoCollection<NumberDocument> NumberDocuments { get; init; }

		public Numbers(IMongoCollection<NumberDocument> _NumberDocuments)
        {
			NumberDocuments = _NumberDocuments;

		}

		
	}
}
