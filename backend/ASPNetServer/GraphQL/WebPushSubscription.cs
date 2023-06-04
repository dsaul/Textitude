// (c) 2023 Dan Saul
using ASPNetServer.WebPush;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace ASPNetServer.GraphQL
{
	[GraphRoute("webPushSubscription")]
	public partial class WebPushSubscription : GraphController
	{

		WebPushController WebPushController { get; init; }

		public WebPushSubscription(WebPushController _WebPushController)
		{
			WebPushController = _WebPushController;
		}

		
	}
}
