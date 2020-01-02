using BR.Core.Models;
using BR.Iam.Models;

namespace BR.Iam.Events
{
    public class UserCreated : EventBase
    {
        public UserCreated(User user)
            : base(user.Id, 0, user)
        {
        }
    }
}
