using Microsoft.AspNetCore.Mvc;
using ChatApi.Application.Requests;
using MediatR;

namespace ChatApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        if (string.IsNullOrEmpty(request.SenderName) || string.IsNullOrEmpty(request.Message))
        {
            return BadRequest("SenderName và Message là bắt buộc.");
        }

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("create-group")]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        if (string.IsNullOrEmpty(request.GroupName) || request.UserNames == null || !request.UserNames.Any())
        {
            return BadRequest("GroupName và UserNames là bắt buộc.");
        }

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("send-group-message")]
    public async Task<IActionResult> SendGroupMessage([FromBody] SendGroupMessageRequest request)
    {
        if (string.IsNullOrEmpty(request.SenderName) || string.IsNullOrEmpty(request.Message) || request.GroupId <= 0)
        {
            return BadRequest("SenderName, Message và GroupId là bắt buộc.");
        }

        var result = await _mediator.Send(request);
        return Ok(result);
    }
}