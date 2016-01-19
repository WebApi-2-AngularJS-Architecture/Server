namespace CompanySystem.Server.DataTransferModels.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common.Mappings.Contracts;
    using Data.Common.Constants;
    using Data.Models.Models;

    public class UserDetailedDataTransferModel : IMapFrom<User>
    {
        [MaxLength(ValidationConstants.FullNameMaxLength, ErrorMessage = ValidationConstants.FullNameMaxLengthErrorMessage)]
        [MinLength(ValidationConstants.FullNameMinLength, ErrorMessage = ValidationConstants.FullNameMinLengthErrorMessage)]
        public string FullName { get; set; }

        [MaxLength(ValidationConstants.UsernameMaxLength, ErrorMessage = ValidationConstants.UsernameMaxLengthErrorMessage)]
        [MinLength(ValidationConstants.UsernameMinLength, ErrorMessage = ValidationConstants.UsernameMinLengthErrorMessage)]
        public string UserName { get; set; }

        [MinLength(ValidationConstants.EmailMinLength, ErrorMessage = ValidationConstants.EmailMinLengthErrorMessage)]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
