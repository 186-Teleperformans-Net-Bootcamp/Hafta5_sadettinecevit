﻿using MediatR;
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
    public class UpdatedMessageController : ControllerBase
    {
        readonly IUpdatedMessageRepository _updatedMessageRepository;
        readonly IMediator _mediator;
        public UpdatedMessageController(IUpdatedMessageRepository updatedMessageRepository, IMediator mediator)
        {
            _mediator = mediator;
            _updatedMessageRepository = updatedMessageRepository;
        }

        [HttpGet("GetUpdatedMessagesById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdUpdatedMessageQueryRequest request)
        {
            IActionResult retVal = null;

            GetByIdUpdatedMessageQueryResponse result = await _mediator.Send(request);

            retVal = Ok(result);

            return retVal;
        }

        [HttpGet("GetAllUpdatedMessages")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUpdatedMessageQueryRequest request)
        {
            IActionResult retVal = null;
            GetAllUpdatedMessageQueryResponse result = await _mediator.Send(request);

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(
                new
                {
                    result.MaxPage,
                    request.Page,
                    request.Limit,
                    HasPrevious = request.Page != 1,
                    HasNext = request.Page < result.MaxPage
                }));

            retVal = Ok(result.ListUpdatedMessageQueryResponse);

            return retVal;
        }

        [HttpPost("AddUpdatedMessage")]
        public async Task<IActionResult> Add([FromBody] CreateUpdatedMessageCommandRequest request)
        {
            CreateUpdatedMessageCommandResponse result = await _mediator.Send(request);

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

        [HttpPut("UpdateUpdatedMessage")]
        public async Task<IActionResult> Update(UpdatedMessage updatedMessage)
        {
            UpdatedMessage result = _updatedMessageRepository.Update(updatedMessage).Result.Entity;

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

        [HttpDelete("DeleteUpdatedMessage")]
        public async Task<IActionResult> Delete([FromQuery] DeleteUpdatedMessageCommandRequest request)
        {
            DeleteUpdatedMessageCommandResponse result = await _mediator.Send(request);

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
