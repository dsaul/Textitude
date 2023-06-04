// (c) 2023 Dan Saul
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace ASPNetServer.GraphQL
{
	public partial class CardDavSources : GraphController
	{
		// This is due to apollo complaining, it should only change if the app is relaunched.
		static readonly Guid apolloId = Guid.NewGuid();
		[Query("id")]
		public string GetId()
		{
			return apolloId.ToString();
		}
	}
}
