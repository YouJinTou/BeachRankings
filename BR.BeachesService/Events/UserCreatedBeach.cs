using BR.Core.Models;

namespace BR.BeachesService.Events
{
    public class UserCreatedBeach : EventBase
    {
        public UserCreatedBeach(string userId, int offset, string beachId)
            : base(userId, offset, beachId)
        {
        }
    }
}
