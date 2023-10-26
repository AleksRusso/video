using Microsoft.EntityFrameworkCore;

namespace video.Models
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Video> Videos { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options)
		  : base(options)
		{
		}
	}

}
