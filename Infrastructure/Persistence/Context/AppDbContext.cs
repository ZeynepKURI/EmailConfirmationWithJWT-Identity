
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
	public class AppDbContext : IdentityDbContext
    {
		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) { }
	}
}

