using BeachRankings.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.Models
{
    [TestFixture]
    internal class BeachTests
    {
        [Test]
        public void UpdateScores_3Reviews_ScoresUpdated()
        {
            var beach = new BeachTestable();
            var user1 = new UserTestable();
            var user2 = new UserTestable();
            var user3 = new UserTestable();
            var review1 = new ReviewTestable(0, null, 7.5, 6.6, 2, 1, 8.1, 3, 2.1, 8.1, 1.5, 0, 0, 1.6, 9.1, 0, 10);
            var review2 = new ReviewTestable(0, null, 4.2, 3.9, 1.5, 0.7, 4.5, 4.2, 4.7, 4.2, 0.7, 0, 0, 0, 2.4, 0, 6.6);
            var review3 = new ReviewTestable(0, null, 7.5, 6, 4, 1, 8, 3, 8, 7, 1, 0, 0, 1, 4, 0, 9);
            var expectedScores = new Score
            {
                SandQuality = 7.1,
                BeachCleanliness = 5.8,
                BeautifulScenery = 3.5,
                CrowdFree = 1.0,
                Infrastructure = 7.6,
                WaterVisibility = 3.1,
                LitterFree = 7.0,
                FeetFriendlyBottom = 6.8,
                SeaLifeDiversity = 1.0,
                CoralReef = 0.0,
                Snorkeling = 0.0,
                Kayaking = 1.0,
                Walking = 4.4,
                Camping = 0.0,
                LongTermStay = 8.8,
                TotalScore = 3.8
            };

            user1.SetLevel(1);
            user2.SetLevel(1);
            user3.SetLevel(7);
            review1.SetAuthor(user1);
            review2.SetAuthor(user2);
            review3.SetAuthor(user3);

            var reviews = new List<Review>
            {
                review1,
                review2,
                review3
            };

            beach.SetReviews(reviews);

            beach.UpdateScores();

            Assert.AreEqual(expectedScores.SandQuality, beach.SandQuality);
            Assert.AreEqual(expectedScores.BeachCleanliness, beach.BeachCleanliness);
            Assert.AreEqual(expectedScores.BeautifulScenery, beach.BeautifulScenery);
            Assert.AreEqual(expectedScores.CrowdFree, beach.CrowdFree);
            Assert.AreEqual(expectedScores.Infrastructure, beach.Infrastructure);
            Assert.AreEqual(expectedScores.WaterVisibility, beach.WaterVisibility);
            Assert.AreEqual(expectedScores.LitterFree, beach.LitterFree);
            Assert.AreEqual(expectedScores.FeetFriendlyBottom, beach.FeetFriendlyBottom);
            Assert.AreEqual(expectedScores.SeaLifeDiversity, beach.SeaLifeDiversity);
            Assert.AreEqual(expectedScores.CoralReef, beach.CoralReef);
            Assert.AreEqual(expectedScores.Snorkeling, beach.Snorkeling);
            Assert.AreEqual(expectedScores.Kayaking, beach.Kayaking);
            Assert.AreEqual(expectedScores.Walking, beach.Walking);
            Assert.AreEqual(expectedScores.Camping, beach.Camping);
            Assert.AreEqual(expectedScores.LongTermStay, beach.LongTermStay);
            Assert.AreEqual(expectedScores.TotalScore, beach.TotalScore);
        }

        [Test]
        public void UpdateScores_3ReviewsWithSomeNulls_ScoresUpdated()
        {
            var beach = new BeachTestable();
            var user1 = new UserTestable();
            var user2 = new UserTestable();
            var user3 = new UserTestable();
            var user4 = new UserTestable();
            var review1 = new ReviewTestable(0, null, null, 8.7, null, null, null, null, null, null, null, null, null, null, null, null, null);
            var review2 = new ReviewTestable(0, null, null, null, null, null, null, null, null, 6.6, null, null, null, null, null, null, null);
            var review3 = new ReviewTestable(0, null, null, 9.3, null, null, null, null, null, null, null, null, 1.7, null, null, null, null);
            var review4 = new ReviewTestable(0, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            var expectedScores = new Score
            {
                BeachCleanliness = 9.2,
                Snorkeling = 1.7,
                LongTermStay = null,
                FeetFriendlyBottom = 6.6,
                TotalScore = 6.0
            };

            user1.SetLevel(1);
            user2.SetLevel(1);
            user3.SetLevel(7);
            user4.SetLevel(1);
            review1.SetAuthor(user1);
            review2.SetAuthor(user2);
            review3.SetAuthor(user3);
            review4.SetAuthor(user4);

            var reviews = new List<Review>
            {
                review1,
                review2,
                review3,
                review4
            };

            beach.SetReviews(reviews);

            beach.UpdateScores();

            Assert.AreEqual(expectedScores.BeachCleanliness, beach.BeachCleanliness);
            Assert.AreEqual(expectedScores.Snorkeling, beach.Snorkeling);
            Assert.AreEqual(expectedScores.LongTermStay, beach.LongTermStay);
            Assert.AreEqual(expectedScores.FeetFriendlyBottom, beach.FeetFriendlyBottom);
            Assert.AreEqual(expectedScores.TotalScore, beach.TotalScore);
        }
    }
}
