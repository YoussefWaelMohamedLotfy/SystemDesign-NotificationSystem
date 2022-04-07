using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.API.Models;

namespace Notifications.API.Data.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.EmailAddress).IsUnique();
        builder.HasIndex(u => u.PhoneNumber).IsUnique();

        builder.HasOne(s => s.Settings)
            .WithOne(s => s.User)
            .HasForeignKey<NotificationSetting>(s => s.UserID);
    }
}
