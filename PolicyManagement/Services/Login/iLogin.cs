public interface ILogin
{
    Task<string?> AuthenticateAsync(string email, string password);
}