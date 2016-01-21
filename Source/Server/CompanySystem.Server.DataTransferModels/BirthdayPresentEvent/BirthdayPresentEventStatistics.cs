namespace CompanySystem.Server.DataTransferModels.BirthdayPresentEvent
{
    using Common.Mappings.Contracts;
    using System.Collections.Generic;
    using AutoMapper;
    using System;

    public class BirthdayPresentEventStatistics
    {
        public BirthdayPresentEventStatistics()
        {
            this.Votes = new Dictionary<string, List<string>>();
        }

        public int EventId { get; set; }

        public DateTime BirthdayDate { get; set; }

        public string CreatorUsername { get; set; }

        public string BirthdayGuyUsername { get; set; }

        public IDictionary<string, List<string>> Votes { get; set; }

        public IEnumerable<string> UsersNotVoted { get; set; }
    }
}