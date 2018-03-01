namespace BeachRankings.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UpvotedReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AssociatedReviewId { get; set; }

        public virtual Review AssociatedReview { get; protected set; }

        [Required]
        public string UpvotingUserId { get; set; }

        public virtual User UpvotingUser { get; protected set; }

        [Required]
        public string VoteReceiverId { get; set; }

        public virtual User VoteReceiver { get; protected set; }
    }
}