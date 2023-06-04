// (c) 2023 Dan Saul
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace ASPNetServer.GraphQL
{
	[GraphRoute("settings")]
	public partial class Settings : GraphController
	{
        public Settings()
        {
            
        }
	}
}
