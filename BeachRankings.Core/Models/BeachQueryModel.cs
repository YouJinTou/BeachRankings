using BeachRankings.Core.Tools;

namespace BeachRankings.Core.Models
{
    public class BeachQueryModel
    {
        public string PF { get; set; }

        public string CT { get; set; }

        public string CY { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }

        public string WB { get; set; }

        public string OB { get; set; }

        public string OD { get; set; }

        public bool IsValid()
        {
            return !InputValidator.AllNullOrWhiteSpace(
                this.PF,
                this.CT,
                this.CY,
                this.L1,
                this.L2,
                this.L3,
                this.L4,
                this.WB,
                this.OB,
                this.OD);
        }
    }
}
