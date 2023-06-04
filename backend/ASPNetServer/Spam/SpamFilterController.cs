// (c) 2023 Dan Saul
using MongoDB.Driver;
using nBayes;
using System.Text;
using Newtonsoft.Json.Linq;
using DanSaul.SharedCode.Spam;
using ASPNetServer.Mongo;

namespace Textitude.Spam
{
    public class SpamFilterController
    {
		IMongoCollection<MessageDocument> Messages { get; init; }

		nBayes.Index SpamIndex { get; set; }

		nBayes.Index HamIndex { get; set; }

		public SpamFilterController(
			IMongoCollection<MessageDocument> _Messages
			)
		{
			Messages = _Messages;

			SpamIndex = nBayes.Index.CreateMemoryIndex();
			HamIndex = nBayes.Index.CreateMemoryIndex();

			ReloadRules(true);
		}

		public void ReloadRules(bool initial = false)
		{
			if (!initial)
			{
				SpamIndex = nBayes.Index.CreateMemoryIndex();
				HamIndex = nBayes.Index.CreateMemoryIndex();
			}

			byte[] preloadSpam = Resources.Spam;
			string preloadSpamString = Encoding.UTF8.GetString(preloadSpam);
			JArray preloadSpamParsed = JArray.Parse(preloadSpamString);
			foreach (string? spam in preloadSpamParsed.Values<string>())
			{
				if (spam == null)
					continue;

				SpamIndex.Add(Entry.FromString(spam));
			}



			byte[] preloadHam = Resources.Ham;
			string preloadHamString = Encoding.UTF8.GetString(preloadHam);
			JArray preloadHamParsed = JArray.Parse(preloadHamString);
			foreach (string? ham in preloadHamParsed.Values<string>())
			{
				if (ham == null)
					continue;

				HamIndex.Add(Entry.FromString(ham));
			}



			//where message.IsSpam != null && message.IsSpam.Value == true

			// Load messages we've flagged as Spam/Ham.
			var ourSpamMsgs = from message in Messages.AsQueryable()
							  where (message.IsHumanConfirmedSpam != null && true == message.IsHumanConfirmedSpam.Value) ||
							  (message.IsAIDetectedSpam != null && true == message.IsAIDetectedSpam.Value)
							  select message;
			foreach (MessageDocument badMsg in ourSpamMsgs)
			{
				if (string.IsNullOrWhiteSpace(badMsg.Body))
					continue;
				SpamIndex.Add(Entry.FromString(badMsg.Body));
			}

			var ourHamMsgs = from message in Messages.AsQueryable()
							 where (message.IsHumanConfirmedSpam != null && false == message.IsHumanConfirmedSpam.Value) ||
								(message.IsAIDetectedSpam != null && false == message.IsAIDetectedSpam.Value)
							 orderby message.ISO8601Timestamp descending
							 select message;
			foreach (MessageDocument goodMsg in ourHamMsgs)
			{
				if (string.IsNullOrWhiteSpace(goodMsg.Body))
					continue;
				HamIndex.Add(Entry.FromString(goodMsg.Body));
			}

		}

		public CategorizationResult Analyze(string input)
		{
			Analyzer analyzer = new();
			return analyzer.Categorize(
				 Entry.FromString(input),
				 SpamIndex,
				 HamIndex);
		}

	}
}
