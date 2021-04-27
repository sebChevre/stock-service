using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BeerApi.Services;
using BeerApi.Services.Impl;
using BeerApi.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using BeerApi.Infrastructure;
using BeerApi.Infrastructure.Impl;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Prometheus;


namespace BeerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI services
            services.AddSingleton<BeerService, BeerServiceImpl>();

            //DI Repos
            services.AddSingleton<BeerRepository, BeerMongDBRepository>();

            services.AddControllersWithViews();
            services.AddControllers();
            
            ConfigureDataBaseSettings(services);

        }

        private void ConfigureDataBaseSettings(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<BeerstoreDatabaseSettings>(
                Configuration.GetSection(nameof(BeerstoreDatabaseSettings)));

            services.AddSingleton<IBeerstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BeerstoreDatabaseSettings>>().Value);

            services.AddSwaggerGen();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureSwagger(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //prometeheus endpoint
            app.UseMetricServer();
            app.UseHttpMetrics();
            ConfigurePrometheus(app);

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseRouting();

            app.UseAuthorization();

           app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "beer-service v" + ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch);
            });
        }

        private static void ConfigurePrometheus(IApplicationBuilder app)
        {
            var counter = Metrics.CreateCounter("pipedream_api_count", "Counts requests to the Pipedream API endpoints", new CounterConfiguration{
            LabelNames = new[] { "method", "endpoint" }
            });
            
            app.Use((context, next) =>{
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
        }
    }
}
