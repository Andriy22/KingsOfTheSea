using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IDataContext
{
    DbSet<Player> Users { get; set; }
    DbSet<Domain.RefreshToken> RefreshTokens { get; set; }
    DbSet<Domain.Stats> Stats { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}