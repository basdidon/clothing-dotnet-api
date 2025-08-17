using Amazon.S3;
using Amazon.S3.Model;
using FastEndpoints;

namespace Api.Endpoints.Images.GetPresignedUrl
{
    public class Response
    {
        public string UploadUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class Endpoint(IAmazonS3 s3Client) : EndpointWithoutRequest<Response>
    {
        public override void Configure()
        {
            Get("/images/presigned-url");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                var key = $"images/{id}.png"; // or decide extension safely
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = "angles-images-bucket",
                    Key =key,
                    Verb = HttpVerb.PUT,
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    
                };
                string presignedUrl = s3Client.GetPreSignedURL(request);

                // 2. Download/View URL (GET)
                var downloadRequest = new GetPreSignedUrlRequest
                {
                    BucketName = "angles-images-bucket",
                    Key = key,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.AddMinutes(60), // longer expiry for viewing
                };
                var imageUrl = s3Client.GetPreSignedURL(downloadRequest);

                Response = new()
                {
                    UploadUrl = presignedUrl,
                    ImageUrl = imageUrl
                };
            }
            catch (AmazonS3Exception ex)
            {
                AddError($"Error generating presigned URL: {ex.Message}");
                await Send.ErrorsAsync(400, ct);
            }
        }
    }
}
