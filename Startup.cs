using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;

namespace core_api
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "weather-forecast_api", Version = "v1" });
            });

            services.AddEndpointsApiExplorer();

            services.AddOpenTelemetryMetrics(builder =>
            {
                builder.AddPrometheusExporter(c => c.ScrapeEndpointPath = "/metrics");
                builder.AddAspNetCoreInstrumentation();
                builder.AddHttpClientInstrumentation();
                builder
                    .AddMeter("WeatherForecastApi")
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri("http://localhost:4317");
                        o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "core_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseOpenTelemetryPrometheusScrapingEndpoint();
        }
    }
}
