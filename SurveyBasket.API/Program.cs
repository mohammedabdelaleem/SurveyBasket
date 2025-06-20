using Hangfire;
using HangfireBasicAuthenticationFilter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);


builder.Host.UseSerilog((context, configuration) =>
	configuration.ReadFrom.Configuration(context.Configuration));



var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseHangfireDashboard("/jobs",
	new DashboardOptions
	{
		Authorization =
		[
			new HangfireCustomBasicAuthenticationFilter
			{
				// just 2 values no need for class and option pattern
				User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
				Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
			}
			],
		DashboardTitle = "Survey Basket Dashboard",
		//IsReadOnlyFunc = (DashboardContext context) => true //normal users can't delete or trigger only read
	}
	);

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var scope = scopeFactory.CreateScope();
var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

RecurringJob.AddOrUpdate("SendNewPollsNotification", () => notificationService.SendNewPollsNotification(null) , Cron.Daily); // https://crontab.guru/  


app.UseCors();
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();

app.UseExceptionHandler();

app.Run();
