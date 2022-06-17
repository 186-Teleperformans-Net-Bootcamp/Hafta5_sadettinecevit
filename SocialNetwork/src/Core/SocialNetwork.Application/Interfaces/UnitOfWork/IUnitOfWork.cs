using Microsoft.EntityFrameworkCore.Storage;
using SocialNetwork.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<IDbContextTransaction> BeginTansactionAsync();
        public IUserRepository UserRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public IFriendRequestRepository FriendRequestRepository { get; }
        public IFriendRepository FriendRepository { get; }
        public IGroupMemberRepository GroupMemberRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IMessageRepository MessageRepository{ get; }
        public IMessageTypeRepository MessageTypeRepository { get; }
        public IUpdatedMessageRepository UpdatedMessageRepository { get; }

    }
}
