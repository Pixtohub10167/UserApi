using System.ComponentModel.DataAnnotations;

namespace UserApi.DTO;

/// <summary>Тело запроса POST /user.</summary>
public class CreateUserDto
{
    [Required(ErrorMessage = "Поле Login обязательно.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Login должен содержать от 3 до 50 символов.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Поле PassHash обязательно.")]
    [RegularExpression("^[a-fA-F0-9]{64}$",
        ErrorMessage = "PassHash должен быть SHA-256: ровно 64 шестнадцатеричных символа.")]
    public string PassHash { get; set; } = null!;
}

/// <summary>Тело запроса PUT /user/{id}. Любое поле можно не передавать.</summary>
public class UpdateUserDto
{
    [StringLength(50, MinimumLength = 3)]
    public string? Login { get; set; }

    [RegularExpression("^[a-fA-F0-9]{64}$",
        ErrorMessage = "PassHash должен быть SHA-256: ровно 64 шестнадцатеричных символа.")]
    public string? PassHash { get; set; }
}

/// <summary>Ответ сервера. Хеш пароля наружу не отдаётся.</summary>
public record UserResponseDto(int Id, string Login, DateTime CreatedAt);

/// <summary>Единый формат сообщения об ошибке.</summary>
public record ErrorResponseDto(string Error, string? Details = null);

/// <summary>Единый формат сообщения об успехе операции.</summary>
public record MessageDto(string Message);
