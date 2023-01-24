using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Languages.Add(new Domain.Language
            {
                Name = "en",
            });
            context.Languages.Add(new Domain.Language
            {
                Name = "ua",
            });
            context.Languages.Add(new Domain.Language
            {
                Name = "ru",
            });

            context.SaveChanges();
        }
    }
}
