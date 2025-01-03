using PowerTradePosition.API.Config;
using PowerTradePosition.API.Data;
using PowerTradePosition.API.Services;

var builder = WebApplication.CreateBuilder(args);


ConfigureServices(builder.Services);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAny");

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddCors(options =>
        {
            options.AddPolicy("AllowAny",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });
    services.Configure<ApiOptions>(builder.Configuration.GetSection("ApiOptions"));
    services.AddSingleton<IReportService, ReportService>();
    services.AddSingleton<IReportRepository, ReportRepository>();
}