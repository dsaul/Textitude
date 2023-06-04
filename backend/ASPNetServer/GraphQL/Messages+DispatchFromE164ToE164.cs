// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using nBayes;
using Textitude;
using Twilio.Rest.Api.V2010.Account;

namespace ASPNetServer.GraphQL
{
	public partial class Messages : GraphController
	{
		[Mutation("dispatchFromE164ToE164")]
		public GenericReturn DispatchFromE164ToE164(string fromE164, string toE164, string body)
		{
			bool? isAIDetectedSpam = null;
			if (false == string.IsNullOrWhiteSpace(body))
			{
				CategorizationResult result = SpamFilterController.Analyze(body);
				switch (result)
				{
					case CategorizationResult.First:
						isAIDetectedSpam = true;
						break;
					case CategorizationResult.Undetermined:
						break;
					case CategorizationResult.Second:
						isAIDetectedSpam = false;
						break;
				}
			}

			MessageResource message = MessageResource.Create(
				body: body,
				from: new Twilio.Types.PhoneNumber(fromE164),
				to: new Twilio.Types.PhoneNumber(toE164),
				maxPrice: Konstants.kMaxOutboundSMSPrice
			);

			MessageDocument dbMsg = new()
			{
				Price = message.Price,
				ErrorMessage = message.ErrorMessage,
				TwilioToCountry = null,
				TwilioToState = null,
				TwilioSmsStatus = message.Status.ToString(),
				Body = message.Body,
				To = message.To,
				TwilioMessagingServiceSid = message.MessagingServiceSid,
				TwilioURI = message.Uri,
				TwilioNumSegments = message.NumSegments,
				TwilioNumMedia = message.NumMedia,
				TwilioMessageSid = message.Sid,
				TwilioAccountSid = message.AccountSid,
				From = message.From.ToString(),
				TwilioApiVersion = message.ApiVersion,
				BackendService = MessageDocument.kBackendServiceTwilio,
				IsAIDetectedSpam = isAIDetectedSpam,
			};


			MessageDocuments.InsertOne(dbMsg);


			return new GenericReturn()
			{
				Status = "success",
			};
		}
	}
}
