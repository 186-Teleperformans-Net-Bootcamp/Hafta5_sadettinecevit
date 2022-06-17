using MediatR;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Persistence.DAL.CQRS.Commands.Request
{
    public class CreateCommentCommandRequest : IRequest<CreateCommentCommandResponse>
    {
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public bool IsPrivate { get; set; }
        public string CommentText { get; set; }
    }
}
