using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Interfaces.Context;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext, IApplicationContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<UpdatedMessage> UpdatedMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>(user => 
            //{
            //    user.HasOne(u => u.Id).WithMany();
            //});

            modelBuilder.Entity<MessageType>().HasData(
                new MessageType { Id = Guid.NewGuid().ToString(), Type = "Personal" }
                , new MessageType { Id = Guid.NewGuid().ToString(), Type = "Group" }
                , new MessageType { Id = Guid.NewGuid().ToString(), Type = "Public" }
                );

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //modelBuilder.Entity<User>().HasOne<User>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Comment>().HasOne<Comment>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Friend>().HasOne<Friend>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<FriendRequest>().HasOne<FriendRequest>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Group>().HasOne<Group>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<GroupMember>().HasOne<GroupMember>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Message>().HasOne<Message>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<MessageType>().HasOne<MessageType>().WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<UpdatedMessage>().HasOne<UpdatedMessage>().WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
