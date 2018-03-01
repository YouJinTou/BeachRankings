using BeachRankings.Models;

namespace Tests.Models
{
    internal class ReviewTestable : Review
    {
        public ReviewTestable(
            int beachId,
            string content,
            double? sandQuality,
            double? beachCleanliness,
            double? beautifulScenery,
            double? crowdFree,
            double? infrastructure,
            double? waterVisibility,
            double? litterFree,
            double? feetFriendlyBottom,
            double? seaLifeDiversity,
            double? coralReef,
            double? snorkeling,
            double? kayaking,
            double? walking,
            double? camping,
            double? longTermStay
            )
        {
            this.SandQuality = sandQuality;
            this.BeachCleanliness = beachCleanliness;
            this.BeautifulScenery = beautifulScenery;
            this.CrowdFree = crowdFree;
            this.Infrastructure = infrastructure;

            this.WaterVisibility = waterVisibility;
            this.LitterFree = litterFree;
            this.FeetFriendlyBottom = feetFriendlyBottom;
            this.SeaLifeDiversity = seaLifeDiversity;
            this.CoralReef = coralReef;

            this.Snorkeling = snorkeling;
            this.Kayaking = kayaking;
            this.Walking = walking;
            this.Camping = camping;
            this.LongTermStay = longTermStay;

            this.UpdateScores();
        }

        public void SetAuthor(User author)
        {
            this.Author = author;
        }
    }
}
