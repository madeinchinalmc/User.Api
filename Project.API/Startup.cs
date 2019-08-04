using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediatR;
using Project.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Project.API.Applications.Service;
using Project.API.Applications.Queries;
using Project.API.Dtos;
using Consul;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Project.Domain.AggregatesModel;
using Project.Infrastructure.Repositories;

namespace Project.API
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
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContextPool<ProjectContext>(
                options => options.UseMySql(Configuration.GetConnectionString("MysqlProject"), // replace with your Connection String
                    mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly(migrationAssembly);
                        mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql); // replace with your Server Version and Type
                    }
            ));

            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));

            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "project_api";
                    options.Authority = "http://localhost";
                });


            services.AddScoped<IRecommendService, TestRecommendService>();
            services.AddScoped<IProjectQueries, ProjectQueries>(sp => {
                return new ProjectQueries(Configuration.GetConnectionString("MysqlProject"));
            });
            services.AddScoped<IProjectRepository, ProjectRepository>(sp => {
                var projectContext = sp.GetRequiredService<ProjectContext>();
                return new ProjectRepository(projectContext);
            });

            services.AddCap(x =>
            {
                // If you are using EF, you need to add the configuration：
                x.UseEntityFramework<ProjectContext>() //Options, Notice: You don't need to config x.UseSqlServer(""") again! CAP can autodiscovery.
                .UseDashboard(); 

                x.UseMySql(Configuration.GetConnectionString("MysqlProject"));
                // CAP support RabbitMQ,Kafka,AzureService as the MQ, choose to add configuration you needed：
                x.UseRabbitMQ("ConnectionString");

                x.UseDiscovery(d => {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5800;
                    d.NodeId = "1";
                    d.NodeName = "CAP NO 1 Node";
                });
            });

            services.AddMediatR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            lifetime.ApplicationStarted.Register(() => {
                Register(app, lifetime, serviceOptions, consul);
            });
            lifetime.ApplicationStopped.Register(() => {
                DeRegister(app, serviceOptions, consul);
            });
            //app.UseCap();
            app.UseAuthentication();
            app.UseMvc();
        }

        private void Register(IApplicationBuilder app,
            IApplicationLifetime lifetime,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };
                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };
                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
        }
        private void DeRegister(IApplicationBuilder app,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            //todo => 重构单独注册方法
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
        }
    }
}
