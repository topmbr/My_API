namespace Minimal_API.Auth
{
    public interface IUserRepository
    {
        UserDto GetUser(UserDto userModel);
    }
}
