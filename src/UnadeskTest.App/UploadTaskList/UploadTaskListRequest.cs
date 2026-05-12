namespace UnadeskTest.App.UploadTaskList;

public sealed record UploadTaskListRequest(int Offset, int Limit)
{
    private const int DefaultOffset = 0;
    private const int DefaultLimit = 20;
    
    public static UploadTaskListRequest Create(int? offset, int? limit) => new(
        offset ?? DefaultOffset, 
        limit ?? DefaultLimit);
}
