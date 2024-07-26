using EclipseworksTaskManager.Domain.Interfaces.Service;

namespace EclipseworksTaskManager.Domain.Services
{
    public class UserService : IUserService
    {
        private string _user;

        public string Get()
            => _user;

        public void Set(string user)
            => _user = user;
    }
}