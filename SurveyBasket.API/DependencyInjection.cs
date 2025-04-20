

using Microsoft.AspNetCore.Authentication.JwtBearer;
using SurveyBasket.API.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SurveyBasket.API;

public static class DependencyInjection
{

	public static IServiceCollection AddDependencies(this IServiceCollection services , IConfiguration configuration)
	{


		// Add services to the container.
		services.AddControllers().AddNewtonsoftJson();
		services.AddAuthConfig();


		services
			.AddSwaggerConfig()
			.AddMappsterrConfig()
			.AddFluentValidationConfig()
			.AddDataBaseConfig(configuration);


		services.AddScoped<IPollService, PollService>();
		services.AddScoped<IAuthService, AuthService>();
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


	private static IServiceCollection AddAuthConfig(this IServiceCollection services)
	{
		services.AddSingleton<IJWTProvider, JWTProvider>();

		services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<AppDbContext>();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bxCTTfkq3heROwz2wvI5pGo8QmWYkOQB")),
					ValidIssuer = "SurveyBasket",
					ValidAudience = "SurveyBasket users"

				};
			});
	       
		return services;
	}



}
