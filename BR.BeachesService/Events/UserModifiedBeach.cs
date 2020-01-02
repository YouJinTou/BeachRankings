using BR.Core.Models;

namespace BR.BeachesService.Events
{
    public class UserModifiedBeach : EventBase
    {
        public UserModifiedBeach(string userId, int offset, string beachId)
            : base(userId, offset, beachId)
        {
        }
    }
}
