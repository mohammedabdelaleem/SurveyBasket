
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace SurveyBasket.API.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public DbSet<Poll> Polls { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}


	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// get all tracked entries which inherite from AuditableEntity
		var entries = ChangeTracker.Entries<AuditableEntity>();

		// from jwt token 
		var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		foreach (var entityEntry in entries)
		{
			if (entityEntry.State == EntityState.Added)
			{
				entityEntry.Property(e => e.CreatedById).CurrentValue = currentUserId!;
			}
			else if (entityEntry.State == EntityState.Modified)
			{
				entityEntry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
				entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
			}
		}

		return base.SaveChangesAsync(cancellationToken);

	}

}
