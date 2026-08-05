using System.Security.Cryptography;
using System.Text;

namespace Selenium.Data;

public static class PasswordHelper
{
    public static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public static bool Verify(string password, string hash)
    {
        return Hash(password) == hash;
    }
}