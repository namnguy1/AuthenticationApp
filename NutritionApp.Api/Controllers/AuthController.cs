using MediatR;
using Microsoft.AspNetCore.Mvc;
using NutritionApp.Application.Commands;
using NutritionApp.Application.Queries;

namespace NutritionApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            // Trả về 400 nếu email đã tồn tại hoặc lỗi validation
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var result = await _mediator.Send(new VerifyEmailCommand(token));
        return Ok(new { Message = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        var jwt = await _mediator.Send(query);
        return Ok(new { Token = jwt });
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommand command)
    {
        var jwt = await _mediator.Send(command);
        return Ok(new { Token = jwt });
    }
}