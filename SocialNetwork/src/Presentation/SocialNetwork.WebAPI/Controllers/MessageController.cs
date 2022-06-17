using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;

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
            List<GetAllMessageQueryResponse> result = await _mediator.Send(request);

            retVal = Ok(result);

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
            Message result = await _messageRepository.Update(message);

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
