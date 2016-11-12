using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_HMI.Models
{
    public class Flight
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название компании")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Тип самолета")]
        public string AircraftType { get; set; }

        [Required]
        [Display(Name = "Дата отправки")]
        public DateTime DateOfStart { get; set; }

        [Required]
        [Display(Name = "Дата прибытия")]
        public DateTime DateOfFinish { get; set; }

        [Required]
        [Display(Name = "Точка отправки")]
        public string DepaturePoint { get; set; }

        [Required]
        [Display(Name = "Точка прибытия")]
        public string ArrivalPoint { get; set; }

        public virtual List<Passenger> Passengers { get; set; }
    }
}
