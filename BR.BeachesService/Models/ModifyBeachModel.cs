using System.ComponentModel.DataAnnotations;

namespace BR.BeachesService.Models
{
    public class ModifyBeachModel
    {
        [Required]
        public string Name { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }
    }
}
