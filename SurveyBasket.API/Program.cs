using MapsterMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPollService, PollService>();


// Register Mapster 

var mappingConfigurations = TypeAdapterConfig.GlobalSettings;
mappingConfigurations.Scan(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IMapper>( new Mapper(mappingConfigurations));



var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
