// (c) 2023 Dan Saul
using Amazon.S3;
using Amazon.S3.Model;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ASPNetServer
{
	[Route("api/[controller]")]
	[ApiController]
	public class AttachmentController : ControllerBase
	{
		AmazonS3Client S3Client { get; init; }
		public AttachmentController(AmazonS3Client _S3Client)
        {
			S3Client = _S3Client;
		}

        [HttpGet("{key}")]
		public async Task Get(
			[FromRoute] string key)
		{
			key = System.Web.HttpUtility.UrlDecode(key);

			GetObjectRequest request = new()
			{
				BucketName = EnvAmazonS3.S3_BUCKET_MMS,
				Key = key
			};

			GetObjectResponse s3response = await S3Client.GetObjectAsync(request);

			using Stream s3Stream = s3response.ResponseStream;

			Response.StatusCode = 200;
			Response.Headers.Add(HeaderNames.ContentType, s3response.Headers.ContentType);
			
			await s3Stream.CopyToAsync(Response.Body);
			await Response.Body.FlushAsync();
		}

	}
}
