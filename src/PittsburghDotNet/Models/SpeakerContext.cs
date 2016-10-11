using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PittsburghDotNet.Models
{

  public class SpeakerContext : DbContext
  {

    public SpeakerContext(DbContextOptions<SpeakerContext> options) : base(options) { }

    public DbSet<Speaker> Speakers { get; set; }

  }

  public static class SpeakerDbContextExtensions
  {

    public static void EnsureSeedData(this SpeakerContext context)
    {

      if (!context.Speakers.Any())
      {

        context.Speakers.AddRange(

          new Speaker { Name = "Jeff Fritz", Email="jefritz@microsoft.com" },
          new Speaker { Name="Rich Dudley", Email="rich@dudleysoftware.com"},
          new Speaker { Name="Ben Rothlisberger", Email="bigben@steelers.nfl.com"},
          new Speaker { Name="Sydney Crosby", Email="sydney@penguins.nhl.com"}
        );

        context.SaveChanges();

      }

    }

    public static bool AllMigrationsApplied(this DbContext context)
    {
      var applied = context.GetService<IHistoryRepository>()
          .GetAppliedMigrations()
          .Select(m => m.MigrationId);


      var total = context.GetService<IMigrationsAssembly>()
          .Migrations
          .Select(m => m.Key);


      return !total.Except(applied).Any();

    }


  }

}