using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineWatchShop.DAL.Contracts;
using OnlineWatchShop.DAL.Implementations;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Helpers;
using OnlineWatchShop.Web.Implementations;
using OnlineWatchShop.Web.Middleware;
using OnlineWatchShop.Web.Profiles;

namespace OnlineWatchShop.Web
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddDbContext<DataContext>(options =>
				options.UseSqlServer(_configuration.GetConnectionString("DefaultString")),
				ServiceLifetime.Transient);

			//var jwtConfigurationSection = _configuration.GetSection("JwtConfiguration");
			services.Configure<JwtConfiguration>(_configuration.GetSection("JwtConfiguration"));

			//var jwtConfiguration = jwtConfigurationSection.Get<JwtConfiguration>();
			//var key = Encoding.ASCII.GetBytes(jwtConfiguration.Key);

			//services.AddAuthentication(options =>
			//	{
			//		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			//	})
			//	//.AddCookie(options =>
			//	//	{
			//	//		options.LoginPath = "/api/login";
			//	//		//options.AccessDeniedPath = "api/forbidden";
			//	//	}
			//	//)
			//	.AddJwtBearer(options =>
			//		{
			//			options.RequireHttpsMetadata = false;
			//			options.SaveToken = true;
			//			options.TokenValidationParameters = new TokenValidationParameters
			//			{
			//				ValidateIssuerSigningKey = true,
			//				IssuerSigningKey = new SymmetricSecurityKey(key),
			//				ValidateIssuer = false,
			//				ValidateAudience = false
			//			};
			//		}
			//	);

			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MainProfile());
			});

			var mapper = mapperConfig.CreateMapper();

			services.AddSingleton(mapper);
			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IProductService, ProductService>();
			services.AddTransient<IJwtUtils, JwtUtils>();
			services.AddScoped<IDbRepository, DbRepository>();
			//services.AddSwaggerGen(c =>
			//{
			//	c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineWatchShop", Version = "v1" });
			//});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage(); 
				//app.UseSwagger();
				//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineWatchShop v1"));
			}

			//app.UseHttpsRedirection();

			app.UseRouting();

			//app.UseAuthentication();
			//app.UseAuthorization();

			app.UseMiddleware<JwtMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
