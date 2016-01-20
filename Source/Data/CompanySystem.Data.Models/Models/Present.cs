namespace CompanySystem.Data.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Present
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal PriceInEuro { get; set; }
    }
}
