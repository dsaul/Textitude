// (c) 2023 Dan Saul
using ASPNetServer.Mongo;
using GraphQL.AspNet.Attributes;
using NodaTime.Text;
using NodaTime;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;

namespace ASPNetServer.GraphQL
{
	public partial class Messages : GraphController
	{

		[Query("latestMessages")]
		public IEnumerable<MessageDocument> AllLatestMessages()
		{
			// We want to return the latest to or from for a specific number.
			Dictionary<string, MessageDocument> filtered = new();

			OffsetDateTime distantPast = OffsetDateTimePattern.GeneralIso.Parse("1000-01-01T00:00:00Z").Value;

			void ProcessMessage(IGrouping<string, MessageDocument> kvp)
			{
				MessageDocument message = kvp.First();

				if (!string.IsNullOrWhiteSpace(message.To))
				{
					if (!filtered.ContainsKey(message.To))
					{
						filtered[message.To] = message;
					}
					else
					{
						OffsetDateTime leftDt = message.Timestamp ?? distantPast;
						OffsetDateTime rightDt = filtered[message.To].Timestamp ?? distantPast;

						int comparison = OffsetDateTime.Comparer.Instant.Compare(leftDt, rightDt);

						if (comparison > 0)
							filtered[message.To] = message;
					}
				}
				if (!string.IsNullOrWhiteSpace(message.From))
				{
					if (!filtered.ContainsKey(message.From))
					{
						filtered[message.From] = message;
					}
					else
					{
						OffsetDateTime leftDt = message.Timestamp ?? distantPast;
						OffsetDateTime rightDt = filtered[message.From].Timestamp ?? distantPast;

						int comparison = OffsetDateTime.Comparer.Instant.Compare(leftDt, rightDt);

						if (comparison > 0)
							filtered[message.From] = message;
					}
				}
			}

			var resLatestFrom = from message in MessageDocuments.AsQueryable()
								orderby message.ISO8601Timestamp descending
								group message by message.From into groupFrom
								select groupFrom;
			var latestFrom = resLatestFrom.ToArray();
			foreach (var message in latestFrom)
				ProcessMessage(message);


			var resLatestTo = from message in MessageDocuments.AsQueryable()
							  orderby message.ISO8601Timestamp descending
							  group message by message.To into groupTo
							  select groupTo;
			var latestTo = resLatestTo.ToArray();
			foreach (var message in latestTo)
				ProcessMessage(message);


			HashSet<MessageDocument> deduplicated = new();
			deduplicated.UnionWith(filtered.Values);

			var sorted = from message in deduplicated
						 orderby message.ISO8601Timestamp descending
						 select message;

			return sorted;
		}

	}
}
