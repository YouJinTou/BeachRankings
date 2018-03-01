namespace BeachRankings.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ScoreWeight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index("IX_Name", IsUnique = true)]
        [MaxLength(100, ErrorMessage = "The weight's name cannot be more than a hundred symbols long.")]
        public string Name { get; set; }

        [Required]
        public double Value { get; set; }
    }
}