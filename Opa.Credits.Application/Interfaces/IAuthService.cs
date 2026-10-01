namespace Opa.Credits.Application.Interfaces;

public interface IAuthService
{
    string Authenticate(string username, string password);
}
