using Microsoft.EntityFrameworkCore;

namespace PittsburghDotNet.Models
{

  public class SpeakerContext : DbContext
  {

    public DbSet<Speaker> Speakers { get; set; }

  }

}