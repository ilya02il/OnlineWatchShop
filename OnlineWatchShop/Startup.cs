using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OnlineWatchShop.DAL.Contracts;
using OnlineWatchShop.DAL.Implementations;
using OnlineWatchShop.Profiles;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Helpers;
using OnlineWatchShop.Web.Implementations;

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
			services.AddDbContext<DataContext>(options =>
				options.UseSqlServer(_configuration.GetConnectionString("DefaultString")),
				ServiceLifetime.Transient);

			//var appSettingsSection = _configuration.GetSection("AuthSettings");
			//services.Configure<AuthSettings>(appSettingsSection);

			// configure JWT authentication
			//var authSettings = appSettingsSection.Get<AuthSettings>();
			//var key = Encoding.ASCII.GetBytes(authSettings.Key);

			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("efefefefefefefefefefefsafdasdfadsfasdfadsfasdfasdfdas")),
						//ValidateIssuer = true,
						//ValidIssuer = authSettings.Issuer,
						//ValidateAudience = false,
						//ValidAudience = authSettings.Audience,
						ValidateLifetime = true
					};
				}
			);

			services.AddControllers();

			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MainProfile());
			});

			var mapper = mapperConfig.CreateMapper();

			services.AddSingleton(mapper);
			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IProductService, ProductService>();
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

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
