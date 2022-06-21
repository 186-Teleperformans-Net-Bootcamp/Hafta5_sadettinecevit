using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
    public class MessageTypeController : ControllerBase
    {
        private readonly IMessageTypeRepository _messageTypeRepository;
        private readonly IMediator _mediator;

        public MessageTypeController(IMessageTypeRepository messageTypeRepository, IMediator mediator)
        {
            _messageTypeRepository = messageTypeRepository;
            _mediator = mediator;
        }

        [HttpGet("GetMessageTypeById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdMessageTypeQueryRequest request)
        {
            IActionResult retVal = null;
            GetByIdMessageTypeQueryResponse result = await _mediator.Send(request);

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

        [HttpGet("GetAllMessageType")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMessageTypeQueryRequest request)
        {
            IActionResult retVal = null;
            GetAllMessageTypeQueryResponse result = await _mediator.Send(request);

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(
                new
                {
                    result.MaxPage,
                    request.Page,
                    request.Limit,
                    HasPrevious = request.Page != 1,
                    HasNext = request.Page < result.MaxPage
                }));

            retVal = Ok(result.ListMessageTypeQueryResponse);

            return retVal;
        }

        [HttpPost("AddMessageType")]
        public async Task<IActionResult> Add([FromBody] CreateMessageTypeCommandRequest request)
        {
            CreateMessageTypeCommandResponse result = await _mediator.Send(request);

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

        [HttpPut("UpdateMessageType")]
        public async Task<IActionResult> Update(MessageType messageType)
        {
            MessageType result = _messageTypeRepository.Update(messageType).Result.Entity;

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

        [HttpDelete("DeleteMessageType")]
        public async Task<IActionResult> Delete(DeleteMessageCommandRequest request)
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
