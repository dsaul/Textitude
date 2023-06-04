// (c) 2023 Dan Saul
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace ASPNetServer.GraphQL
{
	[GraphRoute("cardDavSources")]
	public partial class CardDavSources : GraphController
	{
        public CardDavSources()
        {
            
        }
    }
}
