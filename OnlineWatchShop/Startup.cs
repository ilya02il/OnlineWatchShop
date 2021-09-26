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
using VueCliMiddleware;

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

			//services.AddSpaStaticFiles(configuration =>
			//{
			//	configuration.RootPath = "ClientApp";
			//});

			services.AddDbContext<DataContext>(options =>
				options.UseSqlServer(_configuration.GetConnectionString("LocalHostConnection")),
				ServiceLifetime.Transient);

			services.Configure<JwtConfiguration>(_configuration.GetSection("JwtConfiguration"));

			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MainProfile());
			});

			var mapper = mapperConfig.CreateMapper();

			services.AddSingleton(mapper);
			services.AddTransient<IJwtUtils, JwtUtils>();
			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IProductService, ProductService>();
			services.AddTransient<ICartService, CartService>();
			services.AddTransient<IOrderService, OrderService>();
			services.AddTransient<IUserService, UserService>();
			services.AddScoped<IDbRepository, DbRepository>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseHsts();

			//app.UseSpa(spa =>
			//{
			//	if (env.IsDevelopment())
			//		spa.Options.SourcePath = "ClientApp/";
			//	else
			//		spa.Options.SourcePath = "dist";

			//	if (env.IsDevelopment())
			//		spa.UseVueCli();
			//});

			app.UseRouting();

			app.UseMiddleware<JwtMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
