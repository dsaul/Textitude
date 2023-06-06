using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Attributes;
using ASPNetServer.Mongo;
using MongoDB.Driver;
using Amazon.S3.Model;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Microsoft.AspNetCore.Mvc.Formatters;
using Org.BouncyCastle.Utilities.Zlib;

namespace ASPNetServer.GraphQL
{
	public partial class Messages : GraphController
	{
		[Mutation("removeFromTo")]
		public GenericReturn RemoveFromTo(string e164)
		{
			IEnumerable<MessageDocument> messages = from message in MessageDocuments.AsQueryable()
													where message.To == e164 || message.From == e164
													select message;

			foreach (MessageDocument message in messages)
			{
				// If there are any stored attachments, delete them.
				List<MessageAttachment> attachments = message.Attachments;
				if (attachments.Any())
				{
					foreach (MessageAttachment attachment in attachments)
					{
						DeleteObjectRequest request = new()
						{
							BucketName = EnvAmazonS3.S3_BUCKET_MMS,
							Key = attachment.Key,
						};
						S3Client.DeleteObjectAsync(request).Wait();
					}
				}

				var filter = Builders<MessageDocument>.Filter.Eq(entry => entry.Id, message.Id);
				MessageDocuments.DeleteOne(filter);
			}




			return new GenericReturn()
			{
				Status = "success",
			};
		}
	}
}
