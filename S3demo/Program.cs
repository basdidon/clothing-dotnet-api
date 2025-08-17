using Amazon.S3;
using Amazon.S3.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/images/{key}/presigned-url", (string key,IAmazonS3 s3Client) => {
    try
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "angles-images-bucket",
            Key = $"images/{key}",
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };
        string presignedUrl = s3Client.GetPreSignedURL(request);

        return Results.Ok(new
        {
            UploadUrl = presignedUrl,
            ImageUrl = presignedUrl.Split('?')[0]
        });
    }
    catch (AmazonS3Exception ex)
    {
        return Results.BadRequest($"Error generating presigned URL: {ex.Message}");
    }
});

app.Run();
