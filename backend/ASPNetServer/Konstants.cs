// (c) 2023 Dan Saul
namespace Textitude
{
	public static class Konstants
	{
		public const decimal kMaxOutboundSMSPrice = 0.05M;

		#region VITE_SEND_FROM_E164
		public static string? VITE_SEND_FROM_E164
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("VITE_SEND_FROM_E164");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("VITE_SEND_FROM_E164 empty or missing.");
				return str;
			}
		}
		#endregion
		#region VITE_ATTACHMENT_URI_PREFIX
		public static string? VITE_ATTACHMENT_URI_PREFIX
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("VITE_ATTACHMENT_URI_PREFIX");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("VITE_ATTACHMENT_URI_PREFIX empty or missing.");
				return str;
			}
		}
		#endregion
		#region WEB_PUSH_APPLICATION_SERVER_KEY_FILE
		public static string WEB_PUSH_APPLICATION_SERVER_KEY_FILE
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("WEB_PUSH_APPLICATION_SERVER_KEY_FILE");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("WEB_PUSH_APPLICATION_SERVER_KEY_FILE empty or missing.");
				return str;
			}
		}
		public static string WEB_PUSH_APPLICATION_SERVER_KEY
		{
			get
			{
				string? env = WEB_PUSH_APPLICATION_SERVER_KEY_FILE;
				if (string.IsNullOrWhiteSpace(env))
					throw new InvalidOperationException("WEB_PUSH_APPLICATION_SERVER_KEY_FILE empty or missing.");
				return File.ReadAllText(env);
			}
		}
		#endregion



		


















	}
}
