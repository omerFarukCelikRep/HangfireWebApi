using FireApp.Services;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config => config.UseSimpleAssemblyNameTypeSerializer()
                                             .UseRecommendedSerializerSettings()
                                             .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                                             {
                                                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                                 QueuePollInterval = TimeSpan.Zero,
                                                 UseRecommendedIsolationLevel = true,
                                                 DisableGlobalLocks = true
                                             }));

builder.Services.AddHangfireServer();

builder.Services.AddTransient<IServiceManagement, ServiceManagement>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions{
    DashboardTitle = "Drivers Dashboard",
    Authorization = new []{
        new HangfireCustomBasicAuthenticationFilter(){
            User = "admin",
            Pass = "123"
        }
    }
});
app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<IServiceManagement>(x => x.SyncData(), "0 * * ? * *");

app.Run();
