using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_HMI.Models
{
    public class SearchBySurnameViewModel
    {
        public string SurnameToSearch { get; set; }

        public List<Flight> Flights { get; set; }
    }
}
