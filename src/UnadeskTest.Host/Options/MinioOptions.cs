namespace UnadeskTest.Host.Options;

public sealed class MinioOptions
{
    public required string User { get; init; }
    public required string Password { get; init; }
    public required string Url { get; init; }
}