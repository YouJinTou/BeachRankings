namespace BeachRankings.Services.Aggregation
{
    using BeachRankings.Extensions;
    using BeachRankings.Models.Enums;
    using System;
    using System.Collections.Generic;

    public class BeachFilterDescriptor
    {
        private IDictionary<BeachFilterType, ICollection<string>> descriptionsByFilter;

        public BeachFilterDescriptor()
        {
            this.descriptionsByFilter = new Dictionary<BeachFilterType, ICollection<string>>
            {
                { BeachFilterType.AllCriteria, new List<string> { $"All {Enum.GetNames(typeof(Criterion)).Length} criteria" } },
                { BeachFilterType.BestBeach, new List<string>
                    {
                        Criterion.SandQuality.GetDescription(),
                        Criterion.BeachCleanliness.GetDescription(),
                        Criterion.BeautifulScenery.GetDescription(),
                        Criterion.WaterVisibility.GetDescription(),
                        Criterion.LitterFreeWater.GetDescription(),
                        Criterion.FeetFriendlyBottom.GetDescription()
                    }
                },
                { BeachFilterType.Camping, new List<string>
                    {
                        Criterion.SandQuality.GetDescription(),
                        Criterion.BeachCleanliness.GetDescription(),
                        Criterion.BeautifulScenery.GetDescription(),
                        Criterion.WaterVisibility.GetDescription(),
                        Criterion.LitterFreeWater.GetDescription(),
                        Criterion.Snorkeling.GetDescription(),
                        Criterion.Kayaking.GetDescription(),
                        Criterion.Walking.GetDescription(),
                        Criterion.Camping.GetDescription()
                    }
                },
                { BeachFilterType.HolidayWithKids, new List<string>
                    {
                        Criterion.SandQuality.GetDescription(),
                        Criterion.BeachCleanliness.GetDescription(),
                        Criterion.BeautifulScenery.GetDescription(),
                        Criterion.CrowdFree.GetDescription(),
                        Criterion.Infrastructure.GetDescription(),
                        Criterion.WaterVisibility.GetDescription(),
                        Criterion.LitterFreeWater.GetDescription(),
                        Criterion.FeetFriendlyBottom.GetDescription()
                    }
                },
                { BeachFilterType.Kayaking, new List<string>
                    {
                    Criterion.BeautifulScenery.GetDescription(),
                    Criterion.Kayaking.GetDescription()
                    }
                },
                { BeachFilterType.LongTermStay, new List<string>
                    {
                        Criterion.SandQuality.GetDescription(),
                        Criterion.BeachCleanliness.GetDescription(),
                        Criterion.LitterFreeWater.GetDescription(),
                        Criterion.FeetFriendlyBottom.GetDescription(),
                        Criterion.Walking.GetDescription(),
                        Criterion.LongTermStay.GetDescription()
                    }
                },
                { BeachFilterType.Snorkeling, new List<string>
                    {
                        Criterion.WaterVisibility.GetDescription(),
                        Criterion.LitterFreeWater.GetDescription(),
                        Criterion.SeaLifeDiversity.GetDescription(),
                        Criterion.CoralReef.GetDescription(),
                        Criterion.Snorkeling.GetDescription()
                    }
                },
                { BeachFilterType.Walking, new List<string>
                    {
                        Criterion.SandQuality.GetDescription(),
                        Criterion.BeachCleanliness.GetDescription(),
                        Criterion.BeautifulScenery.GetDescription(),
                        Criterion.Walking.GetDescription()
                    }
                }
            };
        }

        public ICollection<string> GetFilterDescription(BeachFilterType filterType)
        {
            return this.descriptionsByFilter[filterType];
        }
    }
}
