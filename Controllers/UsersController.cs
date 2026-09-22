using Microsoft.AspNetCore.Mvc;
using UserApi.DTO;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("user")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users) => _users = users;

    /// <summary>POST /user — создание нового пользователя.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct)
    {
        try
        {
            var user = await _users.CreateAsync(dto, ct);
            var response = new UserResponseDto(user.Id, user.Login, user.CreatedAt);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }
        catch (LoginAlreadyExistsException ex)
        {
            return Conflict(new ErrorResponseDto("LOGIN_ALREADY_EXISTS", ex.Message));
        }
    }

    /// <summary>GET /user/{id} — получение пользователя по идентификатору.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(id, ct);
        if (user is null)
            return NotFound(new ErrorResponseDto("USER_NOT_FOUND", $"Пользователь с Id={id} не найден."));

        return Ok(new UserResponseDto(user.Id, user.Login, user.CreatedAt));
    }

    /// <summary>GET /user — список всех пользователей (вспомогательный метод).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var users = await _users.GetAllAsync(ct);
        return Ok(users.Select(u => new UserResponseDto(u.Id, u.Login, u.CreatedAt)));
    }

    /// <summary>PUT /user/{id} — обновление данных пользователя.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken ct)
    {
        try
        {
            var user = await _users.UpdateAsync(id, dto, ct);
            return Ok(new UserResponseDto(user.Id, user.Login, user.CreatedAt));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ErrorResponseDto("USER_NOT_FOUND", ex.Message));
        }
        catch (LoginAlreadyExistsException ex)
        {
            return Conflict(new ErrorResponseDto("LOGIN_ALREADY_EXISTS", ex.Message));
        }
    }

    /// <summary>DELETE /user/{id} — удаление пользователя.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _users.DeleteAsync(id, ct);
            return Ok(new MessageDto($"Пользователь с Id={id} удалён."));
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ErrorResponseDto("USER_NOT_FOUND", ex.Message));
        }
    }
}
