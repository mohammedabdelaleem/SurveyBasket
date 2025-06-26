

using Microsoft.AspNetCore.Authentication.JwtBearer;
using SurveyBasket.API.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SurveyBasket.API.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Hangfire;
using SurveyBasket.API.Health;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Asp.Versioning;

namespace SurveyBasket.API;

public static class DependencyInjection
{

	public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		var constr = configuration.GetConnectionString("DefaultConStr") ??
		throw new InvalidOperationException("There is no Connection String For The 'DefaultConStr' Key ");


		// Add services to the container.
		services.AddControllers().AddNewtonsoftJson();

		services.AddHybridCache();

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
			//.AddSwaggerConfig()
			.AddMappsterrConfig()
			.AddFluentValidationConfig()
			.AddDataBaseConfig(configuration);


		services.AddScoped<IPollService, PollService>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IQuestionService, QuestionService>();
		services.AddScoped<IVoteService, VoteService>();
		services.AddTransient<IEmailSender, EmailService>();
		services.AddScoped<IResultService, ResultService>();
		services.AddScoped<INotificationService, NotificationService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IRoleService, RoleService>();

		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();

		services.AddBackgroundJobsConfig(configuration);


		services.AddHttpContextAccessor();

		services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings))); // as we know after this ---> we can inject for this class using IOption Interface i can read the data inside appsettings | user secret file  


		services.AddHealthChecks()
			.AddSqlServer(name: "Database", connectionString: constr)
			.AddHangfire(options => { options.MinimumAvailableServers = 1; })
			.AddUrlGroup(name: "Google API" , uri: new Uri("https://www.google.com"), tags: ["api","testing"], httpMethod:HttpMethod.Get)
			.AddUrlGroup(name: "Facebook API", uri: new Uri("https://www.Facebook.com"), tags: ["api"], httpMethod: HttpMethod.Get)
			.AddCheck< MailProviderHealthChecks>(name:"email provider health check");

		services.AddRateLimiterConfig();


		services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = new ApiVersion(1);
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true;

			options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
		})
			.AddApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'V";
				options.SubstituteApiVersionInUrl = true;
			});


		services.AddEndpointsApiExplorer();

		return services;
	}

	//private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
	//{

	//	services.AddEndpointsApiExplorer();
	//	services.AddSwaggerGen(options =>
	//	{
	//		//options.SwaggerDoc("v1", new OpenApiInfo
	//		//{
	//		//    Version = "v1",
	//		//    Title = "ToDo API",
	//		//    Description = "An ASP.NET Core Web API for managing ToDo items",
	//		//    TermsOfService = new Uri("https://example.com/terms"),
	//		//    Contact = new OpenApiContact
	//		//    {
	//		//        Name = "Example Contact",
	//		//        Url = new Uri("https://example.com/contact")
	//		//    },
	//		//    License = new OpenApiLicense
	//		//    {
	//		//        Name = "Example License",
	//		//        Url = new Uri("https://example.com/license")
	//		//    }
	//		//});

	//		var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	//		options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

	//		options.OperationFilter<SwaggerDefaultValues>();
	//	});

	//	services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

	//	return services;

	//}

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


	private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddHangfire(config => config
	   .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
		.UseSimpleAssemblyNameTypeSerializer()
		.UseRecommendedSerializerSettings()
		.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

		services.AddHangfireServer();

		return services;	
	}
	private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IJWTProvider, JWTProvider>();

		services.AddIdentity<ApplicationUser, ApplicationRole>()
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();


		services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
		services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


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


		services.Configure<IdentityOptions>(options =>
		{
			options.Password.RequiredLength = 8;
			options.SignIn.RequireConfirmedEmail = true;
			options.User.RequireUniqueEmail = true;

			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			options.Lockout.MaxFailedAccessAttempts = 3;
			options.Lockout.AllowedForNewUsers = true;


		});




		return services;
	}


	private static IServiceCollection AddRateLimiterConfig(this  IServiceCollection services)
	{
		services.AddRateLimiter(rateLimiterOptions =>
		{
			rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;



			rateLimiterOptions.AddPolicy(RateLimiterInfo.IpAddressPolicy, httpContext =>
			RateLimitPartition.GetFixedWindowLimiter(
				partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
				factory: _ => new FixedWindowRateLimiterOptions
				{
					PermitLimit = 10,
					Window = TimeSpan.FromSeconds(30),
				}
				));


			rateLimiterOptions.AddPolicy(RateLimiterInfo.UserAddressPolicy, httpContext =>
			RateLimitPartition.GetFixedWindowLimiter(
			partitionKey: httpContext.User.GetUserId(),
			factory: _ => new FixedWindowRateLimiterOptions
			{
				PermitLimit = 10,
				Window = TimeSpan.FromSeconds(30),
			}
			));


			rateLimiterOptions.AddConcurrencyLimiter(RateLimiterInfo.ConcurrenncyPolicy, options =>
			{
				options.PermitLimit = 1;
				options.QueueLimit = 2;
				options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
			});

			#region other Algorithms


			//rateLimiterOptions.AddTokenBucketLimiter(RateLimiterInfo.TokenBuckedPolicy, options =>
			//{
			//	options.TokenLimit = 10;
			//	options.QueueLimit = 2;
			//	options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
			//	options.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
			//	options.TokensPerPeriod = 2;
			//	options.AutoReplenishment = true;
			//});


			//rateLimiterOptions.AddFixedWindowLimiter(RateLimiterInfo.FixedWinsowPolicy, options =>
			//{
			//	options.PermitLimit = 100;
			//	options.Window = TimeSpan.FromHours(2);
			//	options.QueueLimit = 100;
			//	options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
			//});


			//rateLimiterOptions.AddSlidingWindowLimiter(RateLimiterInfo.SlidingWinsowPolicy, options =>
			//{
			//	options.PermitLimit = 100;
			//	options.Window = TimeSpan.FromHours(2);
			//	options.SegmentsPerWindow = 2;
			//	options.QueueLimit = 100;
			//	options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
			//});

			#endregion



		});
		return services;
	}

}
