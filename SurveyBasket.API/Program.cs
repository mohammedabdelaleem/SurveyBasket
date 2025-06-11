using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddDistributedMemoryCache();

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

app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();

app.UseExceptionHandler();

app.Run();
