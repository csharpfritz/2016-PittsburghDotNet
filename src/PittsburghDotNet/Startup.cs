using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PittsburghDotNet.Models;

namespace PittsburghDotNet
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();

      Configuration = builder.Build();

      if (string.IsNullOrEmpty(Configuration["HOSTNAME"]))
      {
        Configuration["HOSTNAME"] = System.Environment.MachineName;
      }

    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddSingleton(Configuration);

      switch (Configuration.GetSection("ConnectionStrings")["Type"])
      {
        case "Postgres":
          services
            .AddEntityFrameworkNpgsql()
            .AddDbContext<Models.SpeakerContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PgConnection")));
          break;
        default:
          services.AddDbContext<Models.SpeakerContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
          break;
      }

      // Add framework services.
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      // Seed the database
      using (var ctx = app.ApplicationServices.GetRequiredService<Models.SpeakerContext>())
      {
        ctx.EnsureSeedData();
      }

      app.UseStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
