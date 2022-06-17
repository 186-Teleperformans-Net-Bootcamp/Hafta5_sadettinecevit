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
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IMediator _mediator;
        public FriendRequestController(IFriendRequestRepository friendRequestRepository,
            IFriendRepository friendRepository, IMediator mediator)
        {
            _mediator = mediator;
            _friendRepository = friendRepository;
            _friendRequestRepository = friendRequestRepository;
        }

        [HttpGet("GetFriendRequestById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdFriendRequestQueryRequest request)
        {
            IActionResult retVal = null;
            GetByIdFriendRequestQueryResponse result = await _mediator.Send(request);

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

        [HttpGet("GetAllFrienRequest")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllFriendRequestQueryRequest request)
        {
            IActionResult retVal = null;
            List<GetAllFriendRequestQueryResponse> result = await _mediator.Send(request);

            retVal = Ok(result);

            return retVal;
        }

        [HttpPost("AddFriendRequest")]
        public async Task<IActionResult> Add([FromBody] CreateFriendRequestCommandRequest request)
        {
            CreateFriendRequestCommandResponse result = await _mediator.Send(request);

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

        [HttpPut("UpdateFriendRequest")]
        public async Task<IActionResult> Update(FriendRequest friendRequest)
        {
            IActionResult retVal = null;
            Friend addResult = null;
            FriendRequest result = null;

            if (friendRequest.Response)
            {
                addResult = await _friendRepository.Add(
                    new Friend
                    {
                        FriendUser = friendRequest.FromUser,
                        User = friendRequest.ToUser,
                        TimeToBeFriend = friendRequest.ResponseTime
                    });

                addResult = await _friendRepository.Add(
                    new Friend
                    {
                        FriendUser = friendRequest.ToUser,
                        User = friendRequest.FromUser,
                        TimeToBeFriend = friendRequest.ResponseTime
                    });
            }

            if (addResult != null)
            {
                result = await _friendRequestRepository.Delete(friendRequest);
            }
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

        [HttpDelete("DeleteFriendRequest")]
        public async Task<IActionResult> Delete([FromQuery] DeleteFriendRequestCommandRequest request)
        {
            DeleteFriendRequestCommandResponse result = await _mediator.Send(request);

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
