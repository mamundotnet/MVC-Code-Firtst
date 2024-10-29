using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectMVC.Models.ViewModels
{
    public class ClientVM
    {
        public ClientVM()
        {
            this.SpotList = new List<int>();
        }
        public int ClientId { get; set; }
        [Required, Display(Name = "Client Name"), StringLength(50)]
        public string ClientName { get; set; }
        [Required, Display(Name = "Birth Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Monthly Income")]
        public int MonthlyIncome { get; set; }
        public string Picture { get; set; }
        [Display(Name ="Picture")]
        public HttpPostedFileBase PictureFile { get; set; }
        public bool MaritalStatus { get; set; }
        public List<int> SpotList { get; set; }
    }
}