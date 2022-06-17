using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.Context;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Handlers.CommandHandlers
{
    public class DeleteFriendRequestCommandHandler : IRequestHandler<DeleteFriendRequestCommandRequest, DeleteFriendRequestCommandResponse>
    {
        private readonly ApplicationDbContext _context;
        public DeleteFriendRequestCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteFriendRequestCommandResponse> Handle(DeleteFriendRequestCommandRequest deleteFriendRequestCommandRequest, CancellationToken cancellationToken)
        {
            DeleteFriendRequestCommandResponse deleteFriendRequestCommandResponse = new DeleteFriendRequestCommandResponse();

            FriendRequest friendRequest = _context.FriendRequests.FirstOrDefault<FriendRequest>(f => f.Id == deleteFriendRequestCommandRequest.Id);
            EntityState result = _context.FriendRequests.Remove(friendRequest).State;
            deleteFriendRequestCommandResponse.IsSuccess = result == EntityState.Deleted;

            return deleteFriendRequestCommandResponse;
        }
    }
}
