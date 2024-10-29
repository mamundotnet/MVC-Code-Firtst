using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectMVC.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        [Required, Display(Name = "Client Name"), StringLength(50)]
        public string ClientName { get; set; }
        [Required, Display(Name = "Birth Date"),DataType(DataType.Date),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Monthly Income")]
        public int MonthlyIncome { get; set; }
        public string Picture { get; set; }
        public bool MaritalStatus { get; set; }
        //nev
        public ICollection<BookingEntry> BookingEntries { get; set; } = new List<BookingEntry>();
    }
}