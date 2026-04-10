namespace Task4.UserAdmin.Application.Interfaces;

public interface IPasswordHasher
{
    string GenerateSalt();
    string HashPassword(string password, string salt);
    bool Verify(string password, string salt, string hash);
}
