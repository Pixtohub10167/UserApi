using UserApi.DTO;
using UserApi.Models;

namespace UserApi.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    Task<User> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
    Task<User> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

/// <summary>Логин уже занят другим пользователем.</summary>
public class LoginAlreadyExistsException : Exception
{
    public LoginAlreadyExistsException(string login)
        : base($"Пользователь с логином «{login}» уже существует.") { }
}

/// <summary>Пользователь не найден.</summary>
public class UserNotFoundException : Exception
{
    public UserNotFoundException(int id)
        : base($"Пользователь с Id={id} не найден.") { }
}
