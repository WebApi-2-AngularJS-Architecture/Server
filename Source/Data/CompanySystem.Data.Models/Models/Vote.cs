namespace CompanySystem.Data.Models.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Vote
    {
        public int PresentId { get; set; }

        [ForeignKey("PresentId")]
        public virtual Present Present { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UserVotedId { get; set; }

        [ForeignKey("UserVotedId")]
        public virtual User UserVoted { get; set; }

        [Key]
        [Column(Order = 3)]
        public int BirthdayPresentEventId { get; set; }

        [ForeignKey("BirthdayPresentEventId")]
        public virtual BirthdayPresentEvent BirthdayPresentEvent { get; set; }
    }
}