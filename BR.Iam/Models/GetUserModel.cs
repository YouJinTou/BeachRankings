using System;

namespace BR.Iam.Models
{
    public class GetUserModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
