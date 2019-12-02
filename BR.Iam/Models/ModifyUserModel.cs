using System.ComponentModel.DataAnnotations;

namespace BR.Iam.Models
{
    public class ModifyUserModel
    {
        [Required]
        public string Id { get; set; }
    }
}
