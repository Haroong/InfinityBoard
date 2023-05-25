using InfinityBoard.Model.Config;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;

using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

using System.Reflection;

namespace InfinityBoard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            RepoDb.MySqlBootstrap.Initialize();
            RepoDb.MySqlConnectorBootstrap.Initialize();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Enable Cors
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin().AllowAnyMethod()
                    .AllowAnyHeader());
            });


            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')\n" +
                    "스웨거 실행용 토큰은 Certification API > tokenOnlySwagger 통해 발급",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);

                option.EnableAnnotations();
            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            // Url 접근 설정 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews();
            // 캐시 설정
            services.AddMemoryCache();

            // WEB API JObject 
            services.AddControllers().AddNewtonsoftJson();

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile
                {
                    Duration = 300,
                    Location = ResponseCacheLocation.Any
                });
            });

            services.AddSingleton(
                new CAppConfig(
                    Configuration.GetSection("AppSetting").Get<AppSetting>(),
                    Configuration.GetSection("Connection").Get<CConnectionConfig>()
                )
            );

            var redisConfiguration = new RedisConfiguration();
            if (CAppConfig.GetAPP_SETTING().APP_MODE.Equals("TEST"))
            {
                redisConfiguration = Configuration.GetSection("Redis:TEST").Get<RedisConfiguration>();
            }
            else
            {
                redisConfiguration = Configuration.GetSection("Redis:REAL").Get<RedisConfiguration>();
            }
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

            ServiceAutoConfig.Configure(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment()) // 개발 환경
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();//HTTP -> HTTPS

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(
            options =>
            {
                // build a swagger endpoint for each discovered API version  
                //foreach (var description in provider.ApiVersionDescriptions)
                //{
                //    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                //}
                options.RoutePrefix = string.Empty;
                options.DocExpansion(DocExpansion.None);
            });
            app.UseStaticFiles();

            app.UseRouting();

            // 인증 사용용
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }
            //});

            AppDomain.CurrentDomain.SetData("ContentRootPath", env.ContentRootPath);
            AppDomain.CurrentDomain.SetData("WebRootPath", env.WebRootPath);

            //logger.AddLog4Net("log4net.config");

            app.UseRedisInformation();
        }
    }
}
