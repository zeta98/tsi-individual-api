using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using UdelarOnlineApi.Entities;
using AutoMapper;

namespace UdelarOnlineApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public IConfiguration _configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<UdelarOnlineContext>(opt =>
        opt.UseSqlServer(_configuration.GetConnectionString("Default")));

      services.AddCors(options =>
        {
          options.AddPolicy("corspolicy",
                            builder =>
                            {
                              builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                            });
        });

      services.AddControllers().AddNewtonsoftJson();
      services.AddAutoMapper(typeof(Startup));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors("corspolicy");

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
