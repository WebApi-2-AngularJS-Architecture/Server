namespace CompanySystem.Server.DataTransferModels.BirthdayPresentEvent
{
    using Data.Models.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ActiveEventsWithPresentsDataTansferModel
    {
        public int Id { get; set; }

        public DateTime BirthdayDate { get; set; }

        public bool IsActive { get; set; }

        public User Creator { get; set; }

        public User BirthdayGuy { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<Present> AvailablePresents { get; set; }
    }
}
