// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using MongoDB.Driver;
using Serilog;
using WebPush;

namespace ASPNetServer.WebPush
{
	public class WebPushController
	{
		
		IMongoCollection<MessageDocument> Messages { get; init; }
		IMongoCollection<WebPushSubscriptionDocument> WebPushSubscriptions { get; init; }
		WebPushClient WebPushClient { get; init; }
		VapidDetails VAPIDDetails { get; init; }


		public WebPushController(
			WebPushClient _WebPushClient,
			IMongoCollection<WebPushSubscriptionDocument> _WebPushSubscriptions,
			IMongoCollection<MessageDocument> _Messages,
			VapidDetails _VAPIDDetails
			)
		{
			WebPushClient = _WebPushClient;
			WebPushSubscriptions = _WebPushSubscriptions;
			Messages = _Messages;
			VAPIDDetails = _VAPIDDetails;
		}

		public void AddSubscription(WebPushSubscriptionDocument payload)
		{
			if (payload.Endpoint == null)
			{
				Log.Error("[{Class}.{Method}()] payload.Subscription.Endpoint == null",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);
				return;
			}
			if (payload.Keys?.P256dh == null)
			{
				Log.Error("[{Class}.{Method}()] payload.Subscription.Keys?.P256dh == null",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);
				return;
			}
			if (payload.Keys?.Auth == null)
			{
				Log.Error("[{Class}.{Method}()] payload.Subscription.Keys?.Auth == null",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);
				return;
			}

			var results = from sub in WebPushSubscriptions.AsQueryable()
						  where sub.Endpoint == payload.Endpoint && 
								sub.Keys.P256dh == payload.Keys.P256dh &&
								sub.Keys.Auth == payload.Keys.Auth
						  select sub;
			
			if (!results.Any())
			{
				WebPushSubscriptions.InsertOne(payload);
			}
			
			
		}

		public async Task NotifyNewMessage(string payload)
		{
			var results = from sub in WebPushSubscriptions.AsQueryable()
						  select sub;

			foreach (WebPushSubscriptionDocument sub in results)
			{
				PushSubscription wpcSubscription = new(sub.Endpoint, sub.Keys?.P256dh, sub.Keys?.Auth);

				try
				{
					try
					{
						await WebPushClient.SendNotificationAsync(wpcSubscription, payload, VAPIDDetails);
					}
					catch (WebPushException ex)
					{
						if (ex.StatusCode == System.Net.HttpStatusCode.Gone)
						{
							var deleteFilter = Builders<WebPushSubscriptionDocument>.Filter.Eq("_id", sub.Id);
							WebPushSubscriptions.DeleteOne(deleteFilter);
							continue;
						}
						else
						{
							throw ex;
						}
					}
					
					
					await Task.Delay(100); // don't want to overload the far side api
				}
				catch (WebPushException ex)
				{
					Log.Error(ex, "[{Class}.{Method}()] {Message} Http STATUS code {0}",
						GetType().Name,
						System.Reflection.MethodBase.GetCurrentMethod()?.Name,
						ex.Message,
						ex.StatusCode
					);
				}
			}

			
		}


	}
}
