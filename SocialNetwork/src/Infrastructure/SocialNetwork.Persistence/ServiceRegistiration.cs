using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace SocialNetwork.Persistence
{
    public static class ServiceRegistiration
    {
        public static void AddPersistenceService(this IServiceCollection serviceCollection, IConfiguration configuration = null)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration?.GetConnectionString("Default")));

            serviceCollection.AddMediatR(typeof(ApplicationDbContext));

            serviceCollection.AddTransient<ICommentRepository, CommentRepository>();
            serviceCollection.AddTransient<IFriendRepository, FriendRepository>();
            serviceCollection.AddTransient<IFriendRequestRepository, FriendRequestRepository>();
            serviceCollection.AddTransient<IGroupRepository, GroupRepository>();
            serviceCollection.AddTransient<IGroupMemberRepository, GroupMemberRepository>();
            serviceCollection.AddTransient<IMessageRepository, MessageRepository>();
            serviceCollection.AddTransient<IMessageTypeRepository, MessageTypeRepository>();
            serviceCollection.AddTransient<IUpdatedMessageRepository, UpdatedMessageRepository>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

        }
    }
}
