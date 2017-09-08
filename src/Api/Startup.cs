using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Runtime.Configuration;

namespace Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env) =>
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();

        IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var azureTableCS = "DefaultEndpointsProtocol=https;AccountName=cs44de867d736aax49dfxbd9;AccountKey=I9QAt4VtgU6UTeJNAtxVCjFijmmMiJNjC032iMUJe2CvVkPxaLWnJYb0AfJuR5AtMpUO6LQ3z43EXOcy4Db+hg==";
            var clientConfig = ClientConfiguration.LocalhostSilo();
            clientConfig.DataConnectionString = azureTableCS;
            clientConfig.DeploymentId = "OrleansTest";
            clientConfig.GatewayProvider = ClientConfiguration.GatewayProviderType.AzureTable;


            var builder = new ClientBuilder().UseConfiguration(clientConfig);
            var clusterClient = builder.Build();
            clusterClient.Connect().GetAwaiter().GetResult();

            services.AddSingleton(clusterClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}