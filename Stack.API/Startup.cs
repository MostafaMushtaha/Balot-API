using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stack.API.AutoMapperConfig;
using Stack.API.Extensions;
using Stack.Core;
using Stack.Core.Managers.Modules.Auth;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.ServiceLayer.Firebase;
using Stack.Entities.DatabaseEntities.User;
using System.Text;
using Stack.Entities.DomainEntities.Auth;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Stack.API.Hubs;
using Serilog;

namespace Stack.API
{
    public class Startup
    {

        readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<APIUrlModel>(Configuration.GetSection("API"));
            services.Configure<FcmNotificationSetting>(Configuration.GetSection("FcmNotification"));

            //Form data size limit
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 50_000_000; // Set the limit to 50MB.
                options.ValueLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            //Add identity framework and configure password
            services.AddIdentity<ApplicationUser, ApplicationRole>(u =>
            {
                u.Password.RequireDigit = false;
                u.Password.RequireLowercase = false;
                u.Password.RequireUppercase = false;
                u.Password.RequireNonAlphanumeric = false;
                u.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleManager<ApplicationRoleManager>();

            services.AddHttpContextAccessor();

            // CORS Configuration 
            // IConfigurationSection originsSection = Configuration.GetSection("AllowedOrigins");
            // string[] origns = originsSection.AsEnumerable().Where(s => s.Value != null).Select(a => a.Value).ToArray();

            // services.AddCors(options =>
            // {
            //     options.AddPolicy(name: AllowSpecificOrigins,
            //                  builder =>
            //                  {
            //                      builder.WithOrigins(origns)
            //                         .AllowAnyMethod()
            //                         .AllowAnyHeader()
            //                         .AllowCredentials();
            //                  });
            // });

            //Allow any origin
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins,
                             builder =>
                             {
                                 builder.SetIsOriginAllowed(origin => true)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials();
                             });
            });

            //Configure Auto Mapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //Register unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Register domain events, service, and primitive methods
            services.AddDomainEvents();
            services.AddServiceMethods();
            // services.AddModerationServiceMethods();
            services.AddPrimitives();

            //Add and configure JWT Bearer Token Authentication . 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                        .AddJwtBearer(options =>
                        {
                            // options.SaveToken = true;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Token:Key").Value)),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                            };

                            //Chat Hub
                            options.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    var accessToken = context.Request.Query["access_token"];

                                    // If the request is for our hub...
                                    var path = context.HttpContext.Request.Path;
                                    if (!string.IsNullOrEmpty(accessToken) &&
                                        (path.StartsWithSegments("/authHub")))
                                    {
                                        // Read the token out of the query string
                                        context.Token = accessToken;
                                    }

                                    return Task.CompletedTask;
                                },

                                OnChallenge = context =>
                                {
                                    context.HandleResponse();
                                    context.Response.StatusCode = 401;
                                    context.Response.Headers.Add("WWW-Authenticate", $"Bearer error=\"{context.Error}\", error_description=\"{context.ErrorDescription}\"");
                                    return Task.CompletedTask;
                                }
                            };

                            services.AddAuthentication(options =>
                            {
                                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                            });
                        });

            ///Use Swagger .
            ConfigureSwagger(services);

            services.AddControllers();

            services.AddSignalR();

            services.AddMediatR(typeof(IMediator));

            // services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
            // services.AddSingleton<IEventBus, RabbitMQEventBus>();

            services.AddLogging(loggingBuilder =>
                    loggingBuilder.AddSerilog(dispose: true));
        }

        //Configure Swagger .
        private static void ConfigureSwagger(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Balot",
                    Version = "v1 - Dev"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "'Bearer ' + token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

            });


            //Use CORS 
            app.UseCors(AllowSpecificOrigins);

            app.UseRouting();

            // using authentication middleware

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AuthHub>("/authHub");
            });

            app.UseHangfireDashboard("/mydashboard");


        }

    }
}
