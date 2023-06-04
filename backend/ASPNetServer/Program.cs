// (c) 2023 Dan Saul
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Textitude.Spam;
using ASPNetServer.WebPush;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using DanSaul.SharedCode.CardDav;
using Sander0542.Authentication.Authelia;
using Amazon.S3;
using GraphQL.AspNet.Configuration;
using DanSaul.SharedCode.Mongo;
using ASPNetServer.Mongo;
using Twilio;
using WebPush;
using System.Net;

namespace Textitude
{
	public class Program
	{
		public static WebApplication? Application { get; private set; }

		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
				.WriteTo.Console()
				.CreateLogger();

			TwilioClient.Init(EnvTwilio.TWILIO_ACCOUNT_SID, EnvTwilio.TWILIO_AUTH_TOKEN);


			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Host.UseSerilog((HostBuilderContext ctx, LoggerConfiguration lc) =>
			{
				lc.WriteTo.Console();
			});

			var devCorsOrigin = "dev";
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(name: devCorsOrigin, policy =>
					policy
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod()
				);
			});

			builder.WebHost.UseUrls("http://*:6612/");
			builder.Services.AddAuthentication(AutheliaDefaults.AuthenticationScheme).AddAuthelia();
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// singletons
			builder.Services.AddSingleton<MongoClientSettings>((serviceProvider) =>
			{
				MongoClientSettings settings = MongoClientSettings.FromConnectionString(EnvMongo.MONGO_URI);
				settings.LinqProvider = MongoDB.Driver.Linq.LinqProvider.V3;
				return settings;
			});

			builder.Services.AddSingleton<MongoClient>((serviceProvider) =>
			{
				MongoClientSettings settings = serviceProvider.GetRequiredService<MongoClientSettings>();
				return new(settings);
			});
			builder.Services.AddSingleton<IMongoDatabase>((serviceProvider) =>
			{
				MongoClient mc = serviceProvider.GetRequiredService<MongoClient>();
				return mc.GetDatabase(EnvTextitude.kMongoDatabase);
			});
			builder.Services.AddSingleton<IMongoCollection<MessageDocument>>((serviceProvider) =>
			{
				IMongoDatabase db = serviceProvider.GetRequiredService<IMongoDatabase>();
				return db.GetCollection<MessageDocument>(EnvTextitude.kMongoCollectionMessages);
			});
			builder.Services.AddSingleton<IMongoCollection<NumberDocument>> ((serviceProvider) =>
			{
				IMongoDatabase db = serviceProvider.GetRequiredService<IMongoDatabase>();
				return db.GetCollection<NumberDocument>(EnvTextitude.kMongoCollectionNumbers);
			});
			builder.Services.AddSingleton<IMongoCollection<VCard>>((serviceProvider) =>
			{
				IMongoDatabase db = serviceProvider.GetRequiredService<IMongoDatabase>();
				return db.GetCollection<VCard>(EnvTextitude.kMongoCollectionCalDavContacts);
			});
			builder.Services.AddSingleton<IMongoCollection<CardDavSourceDocument>>((serviceProvider) =>
			{
				IMongoDatabase db = serviceProvider.GetRequiredService<IMongoDatabase>();
				return db.GetCollection<CardDavSourceDocument>(EnvTextitude.kMongoCollectionCalDavContacts);
			});
			builder.Services.AddSingleton<IMongoCollection<WebPushSubscriptionDocument>>((serviceProvider) =>
			{
				IMongoDatabase db = serviceProvider.GetRequiredService<IMongoDatabase>();
				return db.GetCollection<WebPushSubscriptionDocument>(EnvTextitude.kMongoCollectionWebPushSubscriptions);
			});
			builder.Services.AddSingleton<WebPushClient>((serviceProvider) =>
			{
				return new();
			});
			builder.Services.AddSingleton<VapidDetails>((serviceProvider) =>
			{
				return new(EnvVAPID.VAPID_SUBJECT, EnvVAPID.VAPID_PUBLIC_KEY, EnvVAPID.VAPID_PRIVATE_KEY);
			});
			builder.Services.AddSingleton<CredentialCache>();
			builder.Services.AddSingleton<HttpClient>((serviceProvider) =>
			{
				CredentialCache creds = serviceProvider.GetRequiredService<CredentialCache>();
				return new(new HttpClientHandler { Credentials = creds });
			});
			

			builder.Services.AddSingleton<AmazonS3Config>((serviceProvider) =>
			{
				return new()
				{
					ServiceURL = EnvAmazonS3.S3_SERVICE_URI,
					ForcePathStyle = true
				};
			});
			builder.Services.AddSingleton<AmazonS3Client>((serviceProvider) =>
			{
				AmazonS3Config config = serviceProvider.GetRequiredService<AmazonS3Config>();

				return new(
					EnvAmazonS3.S3_ACCESS_KEY,
					EnvAmazonS3.S3_SECRET_KEY,
					config
				);
			});
			builder.Services.AddSingleton<CardDAVController>();
			builder.Services.AddSingleton<SpamFilterController>();
			builder.Services.AddSingleton<WebPushController>();

			builder.Services.AddGraphQL();

			var Application = builder.Build();

			if (Application.Environment.IsDevelopment())
			{
				Application.UseCors(devCorsOrigin);

			}

			//Application.UseSerilogRequestLogging();

			// Configure the HTTP request pipeline.
			if (Application.Environment.IsDevelopment())
			{
				Application.UseSwagger();
				Application.UseSwaggerUI();
			}

			//Application.UseHttpsRedirection();



			Application.UseRouting();
			Application.MapControllers();
			Application.UseGraphQL();

			Application.UseDefaultFiles(); // Must be before UseStaticFiles
			Application.UseStaticFiles();
			Application.MapFallbackToFile("/index.html");

			Application.UseAuthentication();
			Application.UseAuthorization();

			Application.MapGet("/env/VITE_SEND_FROM_E164", async context => {
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(Konstants.VITE_SEND_FROM_E164 ?? "");
			});
			Application.MapGet("/env/VITE_ATTACHMENT_URI_PREFIX", async context => {
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(Konstants.VITE_ATTACHMENT_URI_PREFIX ?? "");
			});
			Application.MapGet("/env/WEB_PUSH_APPLICATION_SERVER_KEY", async context => {
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(Konstants.WEB_PUSH_APPLICATION_SERVER_KEY ?? "");
			});


			Application.Run();
		}

	}
}
