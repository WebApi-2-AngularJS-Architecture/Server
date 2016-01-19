namespace CompanySystem.Server.DataTransferModels.BirthdayPresentEvent
{
    using Data.Models.Models;
    using Presents;
    using System;
    using System.Collections.Generic;
    using Votes;

    public class ActiveEventDataTransferModel
    {
        public int Id { get; set; }

        public DateTime BirthdayDate { get; set; }

        public bool IsActive { get; set; }

        public string CreatorUsername { get; set; }

        public string BirthdayGuyUsername { get; set; }

        public ICollection<VotesDetailedDataTransferModel> Votes { get; set; }
    }
}
