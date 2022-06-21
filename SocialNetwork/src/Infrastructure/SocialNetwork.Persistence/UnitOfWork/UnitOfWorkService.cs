using Microsoft.EntityFrameworkCore.Storage;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
using SocialNetwork.Persistence.Context;

namespace SocialNetwork.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository UserRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public IFriendRequestRepository FriendRequestRepository { get; }
        public IFriendRepository FriendRepository { get; }
        public IGroupMemberRepository GroupMemberRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IMessageTypeRepository MessageTypeRepository { get; }
        public IUpdatedMessageRepository UpdatedMessageRepository { get; }

        public UnitOfWork(ApplicationDbContext context , IUserRepository userRepository
            ,ICommentRepository commentRepository, IFriendRequestRepository friendRequestRepository
            , IFriendRepository friendRepository, IGroupMemberRepository groupMemberRepository
            , IGroupRepository groupRepository, IMessageRepository messageRepository
            , IMessageTypeRepository messageTypeRepository, IUpdatedMessageRepository updatedMessageRepository)
        {
            _context = context;
            UserRepository = userRepository;
            CommentRepository = commentRepository;
            FriendRequestRepository = friendRequestRepository;
            FriendRepository = friendRepository;
            GroupMemberRepository = groupMemberRepository;
            GroupRepository = groupRepository;
            MessageRepository = messageRepository;
            MessageTypeRepository = messageTypeRepository;
            UpdatedMessageRepository = updatedMessageRepository;
        }

        public async Task<IDbContextTransaction> BeginTansactionAsync() => await _context.Database.BeginTransactionAsync();

        public async ValueTask DisposeAsync() { }
    }
}
