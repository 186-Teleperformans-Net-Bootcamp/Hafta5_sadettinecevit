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
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMediator _mediator;
        public GroupController(IMediator mediator, IGroupRepository groupRepository)
        {
            _mediator = mediator;
            _groupRepository = groupRepository;
        }

        [HttpGet("GetGroupById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdGroupQueryRequest request)
        {
            IActionResult retVal = null;
            GetByIdGroupQueryResponse result = await _mediator.Send(request);

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

        [HttpGet("GetAllGroup")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllGroupQueryRequest request)
        {
            IActionResult retVal = null;
            List<GetAllGroupQueryResponse> result = await _mediator.Send(request);

            retVal = Ok(result);

            return retVal;
        }

        [HttpPost("AddGroup")]
        public async Task<IActionResult> Add([FromBody] CreateGroupCommandRequest request)
        {
            CreateGroupCommandResponse result = await _mediator.Send(request);

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

        [HttpPut("UpdateGroup")]
        public async Task<IActionResult> Update(Group group)
        {
            Group result = await _groupRepository.Update(group);

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

        [HttpDelete("DeleteGroup")]
        public async Task<IActionResult> Delete([FromQuery] DeleteGroupCommandRequest request)
        {
            DeleteGroupCommandResponse result = await _mediator.Send(request);

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
