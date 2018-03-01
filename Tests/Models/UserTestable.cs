using BeachRankings.Models;

namespace Tests.Models
{
    internal class UserTestable : User
    {
        public void SetLevel(int level)
        {
            this.Level = level;
        }
    }
}
