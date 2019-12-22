using BR.Core.Events;

namespace BR.BeachesService.Models
{
    public class UserModifiedBeach : EventBase
    {
        public UserModifiedBeach(string userId, int offset, string beachId)
            : base(userId, offset, beachId)
        {
        }
    }
}
