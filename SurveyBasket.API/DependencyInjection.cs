

namespace SurveyBasket.API;

public static class DependencyInjection
{

	public static IServiceCollection AddDependencies(this IServiceCollection services , IConfiguration configuration)
	{


		// Add services to the container.
		services.AddControllers().AddNewtonsoftJson();



		services
			.AddSwaggerConfigurations()
			.AddMappsterrConfigurations()
			.AddFluentValidationConfigurations()
			.AddDataBase(configuration);


		services.AddScoped<IPollService, PollService>();

		return services;
	}

	public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
	{

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;

	}

	public static IServiceCollection AddMappsterrConfigurations(this IServiceCollection services)
	{
		var mappingConfigurations = TypeAdapterConfig.GlobalSettings;
		mappingConfigurations.Scan(Assembly.GetExecutingAssembly());

		services.AddSingleton<IMapper>(new Mapper(mappingConfigurations));

		return services;
	}

	public static IServiceCollection AddFluentValidationConfigurations(this IServiceCollection services)
	{
		services
			.AddFluentValidationAutoValidation()
			.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


		return services;
	}


	public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
	{

		var constr = configuration.GetConnectionString("DefaultConStr") ??
			throw new InvalidOperationException("There is no Connection String For The 'DefaultConStr' Key ");

		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlServer(constr);
		});

		return services;
	}

}
