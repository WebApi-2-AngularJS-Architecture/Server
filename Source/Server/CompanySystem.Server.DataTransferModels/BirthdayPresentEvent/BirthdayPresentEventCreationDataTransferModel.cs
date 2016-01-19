namespace CompanySystem.Server.DataTransferModels.BirthdayPresentEvent
{
    using System.ComponentModel.DataAnnotations;
    using Data.Common.Constants;

    public class BirthdayPresentEventCreationDataTransferModel
    {
        [Required]
        [MaxLength(ValidationConstants.UsernameMaxLength, ErrorMessage = ValidationConstants.UsernameMaxLengthErrorMessage)]
        [MinLength(ValidationConstants.UsernameMinLength, ErrorMessage = ValidationConstants.UsernameMinLengthErrorMessage)]
        public string CreatorUsername { get; set; }

        [Required]
        [MaxLength(ValidationConstants.UsernameMaxLength, ErrorMessage = ValidationConstants.UsernameMaxLengthErrorMessage)]
        [MinLength(ValidationConstants.UsernameMinLength, ErrorMessage = ValidationConstants.UsernameMinLengthErrorMessage)]
        public string BirthdayGuyUsername { get; set; }

        [Required]
        public string BirthdayDate { get; set; }
    }
}