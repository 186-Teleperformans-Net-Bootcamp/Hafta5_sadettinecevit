using SocialNetwork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Response
{
    public class GetAllCommentQueryResponse
    {
        public string Id { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public bool IsPrivate { get; set; }
        public string CommentText { get; set; }
    }
}
