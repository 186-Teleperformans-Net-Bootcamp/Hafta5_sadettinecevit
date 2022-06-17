using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
    //    [Authorize]
    public class MessageTypeController : ControllerBase
    {
        private readonly IMessageTypeRepository _messageTypeRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IMediator _mediator;

        public MessageTypeController(IMessageTypeRepository messageTypeRepository,
            IMemoryCache memoryCache, IMediator mediator)
        {
            _messageTypeRepository = messageTypeRepository;
            _memoryCache = memoryCache;
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
            List<GetAllMessageTypeQueryResponse> result = null;
            IActionResult retVal = null;

            if (!_memoryCache.TryGetValue("messageTypes", out result))
            {
                result = await _mediator.Send(request);

                MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions();
                memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                memoryCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(30);
                memoryCacheEntryOptions.Priority = CacheItemPriority.Normal;

                _memoryCache.Set("messageTypes", result, memoryCacheEntryOptions);
            }

            retVal = Ok(result);

            return retVal;
        }

        [HttpPost("AddMessageType")]
        public async Task<IActionResult> Add([FromBody] CreateMessageTypeCommandRequest request)
        {
            CreateMessageTypeCommandResponse result = await _mediator.Send(request);

            IActionResult retVal = null;
            if (result.IsSuccess)
            {
                _memoryCache.Remove("messageTypes");
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
            MessageType result = await _messageTypeRepository.Update(messageType);

            IActionResult retVal = null;
            if (result != null)
            {
                _memoryCache.Remove("messageTypes");
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
