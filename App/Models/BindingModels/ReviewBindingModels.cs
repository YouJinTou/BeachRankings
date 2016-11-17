﻿namespace BeachRankings.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class PostReviewBindingModel : CriteriaBaseModel
    {
        [Required]
        public int BeachId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        public string Content { get; set; }

        [RegularExpression(
            @"(?:(?:https?:\/\/)(?:www\.)[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-Z0-9@:%_\+.~#?&\/\/=]*),?)+|(?:(?:https?:\/\/)?(?:www\.)[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-Z0-9@:%_\+.~#?&\/\/=]*),?)+|(?:(?:https?:\/\/)?(?:www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b(?:[-a-zA-Z0-9@:%_\+.~#?&\/\/=]*),?)+",
            ErrorMessage = "We couldn't process the URLs provided.")]
        public string ArticleLinks { get; set; }
    }

    public class EditReviewBindingModel : CriteriaBaseModel
    {
        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(150, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        public string Content { get; set; }
    }
}