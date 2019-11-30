using BR.Core.Abstractions;
using BR.Iam.Models;
using Newtonsoft.Json;

namespace BR.Iam.Events
{
    public class UserCreated : EventBase
    {
        public UserCreated(User user)
            : base(user.Id, 0, JsonConvert.SerializeObject(user))
        {
        }
    }
}
