using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class GroupMemberController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IGroupMemberRepository _groupMemberRepository;
        public GroupMemberController(IGroupMemberRepository groupMemberRepository, IMediator mediator)
        {
            _mediator = mediator;
            _groupMemberRepository = groupMemberRepository;
        }

        [HttpGet("GetGroupMemberById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdGroupMemberQueryRequest request)
        {

            IActionResult retVal = null;
            GetByIdGroupMemberQueryResponse result = await _mediator.Send(request);

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

        [HttpGet("GetAllGroupMember")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllGroupMemberQueryRequest request)
        {
            IActionResult retVal = null;

            GetAllGroupMemberQueryResponse result = await _mediator.Send(request);

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(
                new
                {
                    result.MaxPage,
                    request.Page,
                    request.Limit,
                    HasPrevious = request.Page != 1,
                    HasNext = request.Page < result.MaxPage
                }));

            retVal = Ok(result.ListGroupMemberQueryResponse);

            return retVal;
        }

        [HttpPost("AddGroupMember")]
        public async Task<IActionResult> Add([FromBody] CreateGroupMemberCommandRequest request)
        {
            CreateGroupMemberCommandResponse result = await _mediator.Send(request);

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

        [HttpPut("UpdateGroupMember")]
        public async Task<IActionResult> Update(GroupMember groupMember)
        {
            GroupMember result = _groupMemberRepository.Update(groupMember).Result.Entity;

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

        [HttpDelete("DeleteGroupMember")]
        public async Task<IActionResult> Delete([FromQuery] DeleteGroupMemberCommandRequest request)
        {
            DeleteGroupMemberCommandResponse result = await _mediator.Send(request);

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
