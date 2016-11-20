using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Lab3_HMI.Models
{
    public sealed class FlightsMonitoringViewModel
    {
       
        public List<Flight> Flights { get; set; }
        
        public DateTime CuttenDateTime { get; set; }
       
    }
}
