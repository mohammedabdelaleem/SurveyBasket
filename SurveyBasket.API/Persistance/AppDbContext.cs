
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace SurveyBasket.API.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public DbSet<Answer> Answers { get; set; }
	public DbSet<Poll> Polls { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Vote> Votes { get; set; }
	public DbSet<VoteAnswer> VoteAnswers { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


		// change on delete behaviour
 		var cascadeFKs = modelBuilder.Model
			.GetEntityTypes().SelectMany(e => e.GetForeignKeys())
			.Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

		foreach (var relationship in cascadeFKs)
		{
			relationship.DeleteBehavior = DeleteBehavior.Restrict;
		}

		base.OnModelCreating(modelBuilder);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// get all tracked entries which are inherited from AuditableEntity
		var entries = ChangeTracker.Entries<AuditableEntity>();

		// from jwt token 
		var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

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
