// (c) 2023 Dan Saul
using DanSaul.SharedCode.CardDav;
using DanSaul.SharedCode.Mongo;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;

namespace ASPNetServer.GraphQL
{
	[GraphRoute("cardDavContacts")]
	public partial class CardDavContacts : GraphController
	{
		IMongoCollection<VCard> VCards { get; init; }
		public CardDavContacts(IMongoCollection<VCard> _VCards)
		{
			VCards = _VCards;
		}

		
	}
}
