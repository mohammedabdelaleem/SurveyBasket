

using Microsoft.AspNetCore.Authentication.JwtBearer;
using SurveyBasket.API.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SurveyBasket.API;

public static class DependencyInjection
{

	public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
	{


		// Add services to the container.
		services.AddControllers().AddNewtonsoftJson();

		// AddCors for all origins 
		//services.AddCors(
		//	options => options.AddPolicy("AllowAll", builder =>
		//	builder.AllowAnyOrigin()
		//	.AllowAnyMethod()
		//	.AllowAnyHeader()));


		// AddCors for specific origin(s)
		services.AddCors(
		options => options.AddDefaultPolicy( builder =>
		builder
		.AllowAnyMethod()
		.AllowAnyHeader()
		.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)));


		services.AddAuthConfig(configuration);


		services
			.AddSwaggerConfig()
			.AddMappsterrConfig()
			.AddFluentValidationConfig()
			.AddDataBaseConfig(configuration);


		services.AddScoped<IPollService, PollService>();
		services.AddScoped<IAuthService, AuthService>();


		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();
		return services;
	}

	private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
	{

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;

	}

	private static IServiceCollection AddMappsterrConfig(this IServiceCollection services)
	{
		var mappingConfigurations = TypeAdapterConfig.GlobalSettings;
		mappingConfigurations.Scan(Assembly.GetExecutingAssembly());

		services.AddSingleton<IMapper>(new Mapper(mappingConfigurations));

		return services;
	}

	private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
	{
		services
			.AddFluentValidationAutoValidation()
			.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


		return services;
	}


	private static IServiceCollection AddDataBaseConfig(this IServiceCollection services, IConfiguration configuration)
	{

		var constr = configuration.GetConnectionString("DefaultConStr") ??
			throw new InvalidOperationException("There is no Connection String For The 'DefaultConStr' Key ");

		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlServer(constr);
		});

		return services;
	}


	private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IJWTProvider, JWTProvider>();

		services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<AppDbContext>();


		// Configurations 
		//	services.Configure<JWTOptions>(configuration.GetSection(JWTOptions.SectionName));
		services.AddOptions<JWTOptions>()
				.BindConfiguration(JWTOptions.SectionName)
				.ValidateDataAnnotations()
				.ValidateOnStart();

		var jwtSettings = configuration.GetSection(JWTOptions.SectionName).Get<JWTOptions>();

		// when you need to use [Authorize] with Controller or Endpoint 
		// you don't need to tell them you use JwtBearer
		// you just add the configurations here

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true; // any time during request if you need to reach the token , you can do it easily 
				options.TokenValidationParameters = new TokenValidationParameters
				{
					// more validations more rubost key less hacking 
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key)),
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,

				};
			});

		return services;
	}



}
