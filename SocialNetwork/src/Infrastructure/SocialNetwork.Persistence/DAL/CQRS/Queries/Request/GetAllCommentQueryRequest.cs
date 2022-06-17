using MediatR;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Persistence.DAL.CQRS.Queries.Request
{
    public class GetAllCommentQueryRequest : IRequest<List<GetAllCommentQueryResponse>>
    {
    }
}
