﻿using System;
using System.Collections.Generic;

namespace BR.Core.Models
{
    public class GetBeachModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime AddedOn { get; set; }

        public string AddedBy { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string CountryId { get; set; }

        public string L1 { get; set; }

        public string L1Id { get; set; }

        public string L2 { get; set; }

        public string L2Id { get; set; }

        public string L3 { get; set; }

        public string L3Id { get; set; }

        public string L4 { get; set; }

        public string L4Id { get; set; }

        public string WaterBody { get; set; }

        public string Location { get; set; }

        public string Coordinates { get; set; }

        public double? Score { get; set; }

        public double? SandQuality { get; set; }

        public double? BeachCleanliness { get; set; }

        public double? BeautifulScenery { get; set; }

        public double? CrowdFree { get; set; }

        public double? Infrastructure { get; set; }

        public double? WaterVisibility { get; set; }

        public double? LitterFree { get; set; }

        public double? FeetFriendlyBottom { get; set; }

        public double? SeaLifeDiversity { get; set; }

        public double? CoralReef { get; set; }

        public double? Snorkeling { get; set; }

        public double? Kayaking { get; set; }

        public double? Walking { get; set; }

        public double? Camping { get; set; }

        public double? LongTermStay { get; set; }

        public IEnumerable<GetReviewModel> Reviews { get; set; }
    }
}
