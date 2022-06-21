using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.UnitOfWork;
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
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMediator _mediator;

        public MessageController(IMessageRepository messageRepository, IMediator mediator)
        {
            _mediator = mediator;
            _messageRepository = messageRepository;
        }

        [HttpGet("GetMessageById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdMessageQueryRequest request)
        {
            IActionResult retVal = null;
            GetByIdMessageQueryResponse result = await _mediator.Send(request);

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

        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMessageQueryRequest request)
        {
            IActionResult retVal = null;
            GetAllMessageQueryResponse result = await _mediator.Send(request);

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(
                new
                {
                    result.MaxPage,
                    request.Page,
                    request.Limit,
                    HasPrevious = request.Page != 1,
                    HasNext = request.Page < result.MaxPage
                }));

            retVal = Ok(result.ListMessageQueryResponse);

            return retVal;
        }

        [HttpPost("AddMessage")]
        public async Task<IActionResult> Add([FromBody] CreateMessageCommandRequest request)
        {
            CreateMessageCommandResponse result = await _mediator.Send(request);

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

        [HttpPut("UpdateMessage")]
        public async Task<IActionResult> Update(Message message)
        {
            Message result = _messageRepository.Update(message).Result.Entity;

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

        [HttpDelete("DeleteMessage")]
        public async Task<IActionResult> Delete([FromQuery] DeleteMessageCommandRequest request)
        {
            DeleteMessageCommandResponse result = await _mediator.Send(request);

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
