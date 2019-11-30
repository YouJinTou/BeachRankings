using BR.Core.Events;
using BR.Iam.Models;
using Newtonsoft.Json;

namespace BR.Iam.Events
{
    public class CreateUserFailed : EventBase
    {
        public CreateUserFailed(User user)
            : base(user.Id, 0, JsonConvert.SerializeObject(user))
        {
        }
    }
}
