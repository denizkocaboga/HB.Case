using FluentValidation.AspNetCore;
using HB.Case.Api.DbRepositories;
using HB.Case.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using StackExchange.Redis;

namespace HB.Case
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HB.Case", Version = "v1" });
        });

            services.AddControllers()
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                    //s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            services.AddScoped<IMongoClient, MongoClient>(p => new MongoClient(Configuration.GetConnectionString("MongoDb")));
            services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));
            services.AddScoped<ICacheRepository, RedisRepository>();


            services.AddAutoMapper(typeof(Startup));

            services.AddScoped(typeof(IServiceBase<,>), typeof(ServiceBase<,>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HB.Case v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }



}
