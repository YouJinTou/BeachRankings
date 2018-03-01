namespace App.Code.Users
{
    using BeachRankings.Data.UnitOfWork;
    using System.Linq;

    public class UserLevelCalculator : IUserLevelCalculator
    {
        private IBeachRankingsData data;

        public UserLevelCalculator(IBeachRankingsData data)
        {
            this.data = data;
        }

        public void RecalculateUserLevels()
        {
            var users = this.data.Users.All().ToList();
            var weights = this.data.ScoreWeights.All().ToList();

            foreach (var user in users)
            {
                user.RecalculateLevel(weights);
            }

            this.data.Users.SaveChanges();
        }
    }
}