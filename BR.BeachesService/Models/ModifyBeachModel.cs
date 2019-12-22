using System.ComponentModel.DataAnnotations;

namespace BR.BeachesService.Models
{
    public class ModifyBeachModel
    {
        [Required]
        public string ModifiedBy { get; set; }

        [Required]
        public string OldName { get; set; }

        [Required]
        public string OldCountry { get; set; }

        public string OldL1 { get; set; }

        public string OldL2 { get; set; }

        public string OldL3 { get; set; }

        public string OldL4 { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }
    }
}
