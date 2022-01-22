using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verbum.Identity.Models;

namespace Verbum.Identity.Data
{
    public class AppUserConfiguration: IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder) {
            builder.HasKey(x => x.Id);
        }
    }
}
