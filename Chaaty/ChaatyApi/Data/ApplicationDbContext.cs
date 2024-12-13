using ChaatyApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChaatyApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<FriendShip> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Languages> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FriendShip>().HasKey(k => k.Id);

            builder.Entity<FriendShip>()
                 .HasOne(f=>f.Sender)
                 .WithMany(u => u.FriendshipsInitiated)
                 .HasForeignKey(u => u.SenderUserId)
                 .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FriendShip>().HasOne(f=>f.Receiver)
                 .WithMany(u => u.FriendshipsReceived)
                 .HasForeignKey(u => u.ReceiverUserId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>().HasOne(u => u.Sender)
                .WithMany(u => u.MessagesSent)
                 .HasForeignKey(u => u.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Message>().HasOne(u => u.Recipient)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(u=>u.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
            

        }
    }
}
