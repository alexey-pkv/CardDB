using System.Threading.Tasks;
using CardDB.API.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace CardDB.API
{
	public class APIApp
	{
		private APIStartupData m_data;
		
		
		public async Task Run(APIStartupData startup, string[] args)
		{
			m_data = startup;
			
			await CreateHostBuilder(args).Build().RunAsync();
		}

		public IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host
				.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<APIApp>(); });
		}
		
		
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("/", StatusController.IndexAction);
				endpoints.MapGet("/card", StatusController.IndexAction);
			});
		}
	}
}