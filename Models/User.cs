namespace BeachRankings.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class User : IdentityUser
    {
        private ICollection<Review> reviews;
        private ICollection<BeachPhoto> photos;

        public User()
        {
            this.reviews = new HashSet<Review>();
            this.photos = new HashSet<BeachPhoto>();
        }

        public string AvatarPath { get; set; }

        public virtual ICollection<Review> Reviews
        {
            get
            {
                return this.reviews;
            }
        }

        public virtual ICollection<BeachPhoto> Photos
        {
            get
            {
                return this.photos;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}