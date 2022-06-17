using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Application.Interfaces.Context
{
    public interface IApplicationContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Friend> Friends { get; set; }
        DbSet<FriendRequest> FriendRequests { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<GroupMember> GroupMembers { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<MessageType> MessageTypes { get; set; }
        DbSet<UpdatedMessage> UpdatedMessages { get; set; }
    }
}
