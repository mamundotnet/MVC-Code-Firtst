using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectMVC.Models
{
    public class Spot
    {
        public int SpotId { get; set; }
        [Required,Display(Name = "Spot Name"),StringLength(50)]
        public string SpotName { get; set; }

        //nev
        public ICollection<BookingEntry> BookingEntries { get; set; }= new List<BookingEntry>();

    }
}