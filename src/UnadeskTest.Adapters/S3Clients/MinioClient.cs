using Minio;
using Minio.DataModel.Args;
using UnadeskTest.Domain.Services;

namespace UnadeskTest.Adapters.S3Clients;

public sealed class MinioS3Client(IMinioClient minioClient) : IS3Client
{
    private const string BucketName = "pdf-source";
    private const string ContentType = "application/pdf";

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var bucketArgs = new BucketExistsArgs().WithBucket(BucketName);
        if (!await minioClient.BucketExistsAsync(bucketArgs, cancellationToken))
        {
            var mbArgs = new MakeBucketArgs().WithBucket(BucketName);
            await minioClient.MakeBucketAsync(mbArgs, cancellationToken);
        }
    }

    public async Task<Guid> UploadFileAsync(Stream stream, long fileLength, CancellationToken cancellationToken)
    {
        var objectId = Guid.NewGuid();
        var uploadObjectArgs = new PutObjectArgs()
            .WithBucket(BucketName)
            .WithObject(objectId.ToString())
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(ContentType);
        
        var objectResponse = await minioClient.PutObjectAsync(uploadObjectArgs, cancellationToken);
        
        if (objectResponse.ResponseStatusCode != System.Net.HttpStatusCode.OK)
            throw new InvalidOperationException("Can't upload object");
        
        return objectId;
    }

    public async Task<Stream> DownloadFileAsync(Guid objectId, CancellationToken cancellationToken)
    {
        var responseStream = new MemoryStream();
        try
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectId.ToString())
                .WithCallbackStream(stream => { stream.CopyTo(responseStream); });

            await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
            responseStream.Position = 0;
            return responseStream;
        }
        catch
        {
            await responseStream.DisposeAsync();
            throw;
        }
       
    }
}