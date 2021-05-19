using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockApi.Services;
using StockApi.Services.Impl;
using StockApi.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

using StockApi.Infrastructure;
using StockApi.Infrastructure.Impl;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Prometheus;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using StockApi.Infrastructure.Receiving;




namespace StockApi{


    public class Startup
    {
         public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment env,IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            //DI services
            services.AddSingleton<StockService, StockServiceImpl>();

            //DI Repos
            services.AddSingleton<StockRepository, StockMongoDbRepository>();

            services.AddControllersWithViews();
            services.AddControllers();

            ConfigureDataBaseSettings(services);

            ConfigureAuthentification(services);

            services.AddHostedService<BeerCreatedReceicer>();

        }

        private void ConfigureAuthentification(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                 string JwtAudience = System.Environment.GetEnvironmentVariable("JWT_AUDIENCE");
                string JwtAuthority = System.Environment.GetEnvironmentVariable("JWT_AUTHORITY");

                o.Authority = JwtAuthority;
                o.Audience = JwtAudience;
                o.RequireHttpsMetadata = false;
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        
                        return c.Response.WriteAsync(c.Exception.ToString());
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("reader", policy => policy.RequireClaim("user_roles", "reader"));
                options.AddPolicy("contributor", policy => policy.RequireClaim("user_roles", "contributor"));
            });
        }

        private void ConfigureDataBaseSettings(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<StockstoreDatabaseSettings>(
                Configuration.GetSection(nameof(StockstoreDatabaseSettings)));

            services.AddSingleton<IStockstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<StockstoreDatabaseSettings>>().Value);

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

            app.UseAuthentication();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "stock-service v" + ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch);
            });
        }

        private static void ConfigurePrometheus(IApplicationBuilder app)
        {
            var counter = Metrics.CreateCounter("call_api_count", "Counts requests to the API endpoints", new CounterConfiguration{
            LabelNames = new[] { "method", "endpoint" }
            });
            
            app.Use((context, next) =>{
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
        }
    }
}
