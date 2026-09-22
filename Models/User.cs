using System.ComponentModel.DataAnnotations;

namespace UserApi.Models;

/// <summary>Пользователь системы. Открытый пароль не хранится — только его хеш.</summary>
public class User
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Login { get; set; } = null!;

    /// <summary>SHA-256 в виде 64 шестнадцатеричных символов.</summary>
    [Required, MaxLength(64)]
    public string PassHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
