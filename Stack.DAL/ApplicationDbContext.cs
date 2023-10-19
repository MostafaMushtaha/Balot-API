using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Entities.DatabaseEntities.User;
using Stack.Entities.DatabaseEntities.UserProfile;
using System.Threading;
using System.Threading.Tasks;
using Stack.Entities.DatabaseEntities.Notifications;
using Stack.Entities.DatabaseEntities.Modules.User;
using Stack.Entities.Models.Modules.Common;
using Stack.Entities.DatabaseEntities.Modules.System;
using Stack.Entities.DatabaseEntities.Games;
using Stack.Entities.DatabaseEntities.Groups;
using Stack.Entities.DatabaseEntities.GroupMedia;

namespace Stack.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            // Dispatch Domain Events collection.

            // await _mediator.DispatchDomainEventsAsync(this);

            OnBeforeSaving();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships
            modelBuilder
                .Entity<Group>()
                .HasMany(g => g.Members)
                .WithOne(gm => gm.Group)
                .HasForeignKey(gm => gm.GroupID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Group>()
                .HasMany(g => g.Media)
                .WithOne(m => m.Group)
                .HasForeignKey(m => m.GroupID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Group>()
                .HasMany(g => g.Games)
                .WithOne(m => m.Group)
                .HasForeignKey(m => m.GroupID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<UserDevice>()
                .HasOne(pr => pr.User)
                .WithMany(p => p.Devices)
                .HasForeignKey(pr => pr.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppVersion>().HasNoKey();

            // modelBuilder.Entity<UserConnectionID>()
            // .HasOne(pr => pr.User)
            // .WithMany(p => p.ConnectionIDs)
            // .HasForeignKey(pr => pr.UserID)
            // .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Friends>()
                .HasOne(f => f.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserID);
            // .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Friends>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Media>()
                .HasOne(m => m.Group)
                .WithMany(t => t.Media)
                .HasForeignKey(m => m.GroupID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Media>()
                .HasOne(m => m.Creator)
                .WithMany(gm => gm.Media)
                .HasForeignKey(m => m.CreatorID);

            // GameRound
            modelBuilder
                .Entity<GameRound>()
                .HasOne(gr => gr.Game)
                .WithMany(g => g.Rounds)
                .HasForeignKey(gr => gr.GameID);

            // Game
            modelBuilder
                .Entity<Game>()
                .HasMany(g => g.Rounds)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Game>()
                .HasMany(g => g.GameMembers)
                .WithOne(m => m.Game)
                .HasForeignKey(r => r.GameID)
                .OnDelete(DeleteBehavior.NoAction);

            // Group_Member
            modelBuilder
                .Entity<Group_Member>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMember)
                .HasForeignKey(gm => gm.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Group_Member>()
                .HasOne(gm => gm.Group)
                .WithMany(t => t.Members)
                .HasForeignKey(gm => gm.GroupID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<ApplicationUser>()
                .HasMany(u => u.GroupMember)
                .WithOne(gm => gm.User)
                .HasForeignKey(gm => gm.UserID);
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<RegistrationRequest> RegistrationRequests { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<ProfileSettings> ProfileSettings { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<UserDevice> UserDevices { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Group_Member> Group_Member { get; set; }

        // public virtual DbSet<OTPRequest> OTPRequests { get; set; }

        // public virtual DbSet<UserConnectionID> UserConnectionIDs { get; set; }
        public virtual DbSet<AppVersion> AppVersions { get; set; }
    }
}
