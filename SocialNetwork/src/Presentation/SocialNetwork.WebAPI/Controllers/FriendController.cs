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
    public class FriendController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFriendRepository _friendRepository;
        public FriendController(IMediator mediator, IFriendRepository friendRepository)
        {
            _mediator = mediator;
            _friendRepository = friendRepository;
        }

        [HttpGet("GetFriendById/{id}")]
        public async Task<IActionResult> GetById([FromQuery]GetByIdFriendQueryRequest request)
        {
            IActionResult retVal = null;
            GetByIdFriendQueryResponse result = await _mediator.Send(request);

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

        [HttpGet("GetAllFriend")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllFriendQueryRequest request)
        {
            IActionResult retVal = null;
            List<GetAllFriendQueryResponse> result = await _mediator.Send(request);

            retVal = Ok(result);

            return retVal;
        }

        [HttpPost("AddFriend")]
        public async Task<IActionResult> Add([FromBody]CreateFriendCommandRequest request)
        {
            CreateFriendCommandResponse result = await _mediator.Send(request);

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

        [HttpPut("UpdateFriend")]
        public async Task<IActionResult> Update(Friend friend)
        {
            Friend result = await _friendRepository.Update(friend);

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

        [HttpDelete("DeleteFriend")]
        public async Task<IActionResult> Delete([FromQuery] DeleteFriendCommandRequest request)
        {
            DeleteFriendCommandResponse result = await _mediator.Send(request);

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
