using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Dto;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Request;
using SocialNetwork.Persistence.DAL.CQRS.Commands.Response;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Request;
using SocialNetwork.Persistence.DAL.CQRS.Queries.Response;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _distributedCache;
        private readonly IMediator _mediator;
        public UserController(IUserRepository userRepository,
            UserManager<User> userManager, IDistributedCache distributedCache
            , IMediator mediator)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _distributedCache = distributedCache;
            _mediator = mediator;
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdUserQueryRequest request)
        {
            IActionResult retVal = null;
            GetByIdUserQueryResponse result = await _mediator.Send(request);

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

        [HttpGet("GetAllUsers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUserQueryRequest request)
        {
            IActionResult retVal = null;
            List<GetAllUserQueryResponse> result = null;

            byte[] cachedBytes = _distributedCache.Get("GetAllUsers");

            if (cachedBytes == null)
            {
                result = await _mediator.Send(request);
            }
            else
            {
                string jsonData = Encoding.UTF8.GetString(cachedBytes);
                result = JsonSerializer.Deserialize<List<GetAllUserQueryResponse>>(jsonData);
            }

            if (result == null)
            {
                retVal = BadRequest();
            }
            else
            {
                if (cachedBytes == null)
                {
                    string jsonUsers = JsonSerializer.Serialize(result);
                    _distributedCache.Set("GetAllUsers", Encoding.UTF8.GetBytes(jsonUsers));
                }
                retVal = Ok(result);
            }

            return retVal;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] CreateUserCommandRequest request)
        {
            IActionResult retVal = null;

            CreateUserCommandResponse result = _mediator.Send(request).Result;

            if (result.IsSuccess)
            {
                _distributedCache.RemoveAsync("GetAllUsers");
                retVal = Ok(result);
            }
            else
            {
                retVal = BadRequest(result);
            }

            return retVal;
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            User result = await _userRepository.Update(user);

            IActionResult retVal = null;
            if (result != null)
            {
                _distributedCache.RemoveAsync("GetAllUsers");
                retVal = Ok(result);
            }
            else
            {
                retVal = BadRequest(result);
            }

            return retVal;
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> Delete([FromQuery] DeleteUserCommandRequest request)
        {
            DeleteUserCommandResponse result = await _mediator.Send(request);

            IActionResult retVal;

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

