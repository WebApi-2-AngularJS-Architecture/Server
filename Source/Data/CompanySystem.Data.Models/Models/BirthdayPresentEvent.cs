namespace CompanySystem.Data.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BirthdayPresentEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BirthdayDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public virtual User Creator { get; set; }

        [Required]
        public virtual User BirthdayGuy { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
