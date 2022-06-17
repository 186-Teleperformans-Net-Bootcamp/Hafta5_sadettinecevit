﻿using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class DeleteUpdatedMessageCommandRequest : IRequest<DeleteUpdatedMessageCommandResponse>
    {
        public string Id { get; set; }
    }
}
