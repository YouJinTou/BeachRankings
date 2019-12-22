using BR.Core.Events;

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
