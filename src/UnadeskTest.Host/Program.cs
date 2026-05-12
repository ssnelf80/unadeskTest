using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Minio;
using UnadeskTest.Adapters.PdfParsers;
using UnadeskTest.Adapters.S3Clients;
using UnadeskTest.App.ChangeUploadTaskStatus;
using UnadeskTest.App.GetPdfDocument;
using UnadeskTest.App.GetPdfDocumentsList;
using UnadeskTest.App.ParsePdf;
using UnadeskTest.App.UploadPdf;
using UnadeskTest.App.UploadTaskList;
using UnadeskTest.DAL.BackgroundWorker;
using UnadeskTest.DAL.PdfContext;
using UnadeskTest.DAL.PdfDocument;
using UnadeskTest.Domain.ReadModelProviders;
using UnadeskTest.Domain.Repositories;
using UnadeskTest.Domain.Services;
using UnadeskTest.Host.Extensions;
using UnadeskTest.Host.Models;
using UnadeskTest.Host.Options;
using UnadeskTest.Host.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UnadeskTest API", 
        Version = "v1"
    });
    
    options.DescribeAllParametersInCamelCase();
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddScoped<IS3Client, MinioS3Client>();
builder.Services.AddScoped<IPdfParser, PdfPigParser>();

builder.Services.AddScoped<IBackgroundWorkerRepository, BackgroundWorkerRepository>();
builder.Services.AddScoped<IPdfDocumentRepository, PdfDocumentRepository>();

builder.Services.AddScoped<IBackgroundWorkerReadModelProvider, BackgroundWorkerReadModel>();
builder.Services.AddScoped<IPdfDocumentReadModelProvider, PdfDocumentReadModel>();

builder.Services.AddScoped<IValidator<PaginationParameters>, PaginationParametersValidator>();
builder.Services.AddScoped<IValidator<UploadFileRequest>, UploadFileRequestValidator>();

var minioOptions = builder.Configuration.GetSection(nameof(MinioOptions)).Get<MinioOptions>()!;

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioOptions.Url)
    .WithCredentials(minioOptions.User, minioOptions.Password)
    .WithSSL(false)
    .Build());

builder.Services.AddDbContext<PdfDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PdfDbConnection")));

var rabbitOptions = builder.Configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>()!;

builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<UploadPdfRequest>();
    x.AddRequestClient<UploadTaskListRequest>();
    x.AddRequestClient<GetPdfDocumentsListRequest>();
    x.AddRequestClient<GetPdfDocumentRequest>();
    
    x.AddConsumer<UploadPdfConsumer>();
    x.AddConsumer<ParsePdfConsumer>();
    x.AddConsumer<UploadTaskListConsumer>();
    x.AddConsumer<ChangeUploadTaskStatusConsumer>();
    x.AddConsumer<GetPdfDocumentsListConsumer>();
    x.AddConsumer<GetPdfDocumentConsumer>();
    
    x.AddEntityFrameworkOutbox<PdfDbContext>(o =>
    {
        o.UsePostgres();
        o.QueryDelay = TimeSpan.FromSeconds(1);
        o.UseBusOutbox();
    });
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitOptions.Url, "/", h =>
        {
            h.Username(rabbitOptions.Username);
            h.Password(rabbitOptions.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var s3Client = scope.ServiceProvider.GetRequiredService<IS3Client>();
await s3Client.InitializeAsync();

await scope.ServiceProvider.PdfDbMigrateAsync();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseSwagger();
app.UseSwaggerUI();

app.Run();