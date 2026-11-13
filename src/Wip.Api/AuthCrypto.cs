using System.Security.Cryptography;
using System.Text;

internal static class AuthCrypto
{
    public static string HashRefresh(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
