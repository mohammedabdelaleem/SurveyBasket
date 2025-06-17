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

app.UseCors();

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
		DashboardTitle = "Survey Basket Dashboard"
	}
	);


app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();

app.UseExceptionHandler();

app.Run();
