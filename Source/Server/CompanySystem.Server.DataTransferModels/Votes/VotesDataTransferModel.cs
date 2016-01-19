namespace CompanySystem.Server.DataTransferModels.Votes
{
    using System.ComponentModel.DataAnnotations;

    public class VotesDataTransferModel
    {
        [Required]
        public int PresentId { get; set; }

        [Required]
        public string UserVotedUsername { get; set; }

        [Required]
        public int BirthdayPresentEventId { get; set; }
    }
}
