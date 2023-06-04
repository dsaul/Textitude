// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;

namespace ASPNetServer.GraphQL
{
	public partial class Numbers : GraphController
	{
		[Query("all")]
		public IEnumerable<NumberDocument> GetAll()
		{
			var results = from number in NumberDocuments.AsQueryable()
						  orderby number.E164 ascending
						  select number;

			foreach (NumberDocument numberDocument in results)
				yield return numberDocument;

			yield break;
		}
	}
}
