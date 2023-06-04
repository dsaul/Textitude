// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace ASPNetServer.GraphQL
{
	public partial class WebPushSubscription : GraphController
	{
		[Mutation("newSubscription")]
		public GenericReturn NewSubscription(WebPushSubscriptionDocument payload)
		{
			WebPushController.AddSubscription(payload);

			return new GenericReturn()
			{
				Status = "success",
			};
		}
	}
}
