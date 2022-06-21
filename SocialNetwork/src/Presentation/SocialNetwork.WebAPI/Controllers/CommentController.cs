using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.DAL.CQRS.Queries;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System.Text.Json;

namespace SocialNetwork.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //    [Authorize]
    public class CommentController : ControllerBase
    {
        //private readonly ICommentRepository _commentRepository;
        private readonly IMediator _mediator;
        public CommentController(IMediator mediator)
        {
            //_commentRepository = commentRepository;
            _mediator = mediator;

        }

        [HttpGet("GetAllComment")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCommentQueryRequest request)
        {
            IActionResult retVal = null;
            //PaginingResponse<List<GetAllCommentQueryResponse>> result = await _mediator.Send(request);
            var result = await _mediator.Send(request);

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(
                new
                {
                    result.MaxPage,
                    request.Page,
                    request.Limit,
                    HasPrevious = request.Page != 1,
                    HasNext = request.Page < result.MaxPage
                }));

            retVal = Ok(result.ListCommentQueryResponse);
            return retVal;
        }

        [HttpGet("GetCommentById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdCommentQueryRequest request)
        {
            IActionResult retVal = null;
            GetByIdCommentQueryResponse result = await _mediator.Send(request);

            if (result == null)
            {
                retVal = BadRequest();
            }
            else
            {
                retVal = Ok(result);
            }

            return retVal;
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> Add([FromBody] CreateCommentCommandRequest request)
        {
            CreateCommentCommandResponse result = await _mediator.Send(request);

            IActionResult retVal = null;
            if (result.IsSuccess)
            {
                retVal = Ok(result);
            }
            else
            {
                retVal = BadRequest(result);
            }

            return retVal;
        }

        [HttpPut("UpdateComment")]
        public async Task<IActionResult> Update(Comment comment)
        {
            Comment result = null; // _commentRepository.Update(comment).Result.Entity;

            IActionResult retVal = null;
            if (result != null)
            {
                retVal = Ok(result);
            }
            else
            {
                retVal = BadRequest(result);
            }

            return retVal;
        }

        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> Delete([FromQuery] DeleteCommentCommandRequest request)
        {
            DeleteCommentCommandResponse result = await _mediator.Send(request);

            IActionResult retVal = null;
            if (result.IsSuccess)
            {
                retVal = Ok(result);
            }
            else
            {
                retVal = BadRequest(result);
            }

            return retVal;
        }


    }
}
