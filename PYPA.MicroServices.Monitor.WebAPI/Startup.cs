using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PYPA.MicroServices.Infra.Config;
using PYPA.MicroServices.RabbitMQACL;
using PYPA.MicroServices.RabbitMQACL.Config;

namespace PYPA.MicroServices.Monitor.WebAPI
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
            services.AddOptions();
            services.Configure<RabbitMQConfiguration>(Configuration.GetSection("RabbitMQ"));
            services.Configure<RabbitMQQueueConfiguration>(Configuration.GetSection("RabbitMQ.Queue")); 
            services.Configure<CouchbaseConfigModel>(Configuration.GetSection("Couchbase")); 

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<CouchbaseConfig>();
            services.AddSingleton<IPublisher, Publisher>(); 



            services.AddCors(o => o.AddPolicy("AllowPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CouchbaseConfig couchbase)
        {
            app.UseCors("AllowPolicy");

            couchbase.Register();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
