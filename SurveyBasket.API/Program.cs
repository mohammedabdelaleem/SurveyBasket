


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);


builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
	.AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.Run();
