using System.ComponentModel.DataAnnotations;

namespace BR.Core.Models
{
    public class ModifyUserModel
    {
        [Required]
        public string Id { get; set; }
    }
}
