using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Task4.UserAdmin.Application.Interfaces;

namespace Task4.UserAdmin.Application.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    public string GenerateSalt()
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 32);

        return Convert.ToBase64String(hash);
    }

    public bool Verify(string password, string salt, string hash)
        => HashPassword(password, salt) == hash;
}
