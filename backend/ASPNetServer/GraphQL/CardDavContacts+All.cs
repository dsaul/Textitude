// (c) 2023 Dan Saul
using DanSaul.SharedCode.CardDav;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;

namespace ASPNetServer.GraphQL
{
	public partial class CardDavContacts : GraphController
	{
		[Query("all")]
		public IEnumerable<VCard> GetAll()
		{
			var results = from message in VCards.AsQueryable()
						  select message;

			foreach (VCard document in results)
				yield return document;
			yield break;
		}
	}
}
