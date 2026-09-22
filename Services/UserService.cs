using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.DTO;
using UserApi.Models;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly UserDbContext _db;

    public UserService(UserDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Users.AsNoTracking().OrderBy(u => u.Id).ToListAsync(ct);

    public async Task<User> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var login = dto.Login.Trim();

        if (await _db.Users.AnyAsync(u => u.Login == login, ct))
            throw new LoginAlreadyExistsException(login);

        var user = new User
        {
            Login = login,
            PassHash = dto.PassHash.ToLowerInvariant(),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user;
    }

    public async Task<User> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
            ?? throw new UserNotFoundException(id);

        if (!string.IsNullOrWhiteSpace(dto.Login))
        {
            var login = dto.Login.Trim();
            if (await _db.Users.AnyAsync(u => u.Login == login && u.Id != id, ct))
                throw new LoginAlreadyExistsException(login);
            user.Login = login;
        }

        if (!string.IsNullOrWhiteSpace(dto.PassHash))
            user.PassHash = dto.PassHash.ToLowerInvariant();

        await _db.SaveChangesAsync(ct);
        return user;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
            ?? throw new UserNotFoundException(id);

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
    }
}
