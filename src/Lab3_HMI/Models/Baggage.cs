using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_HMI.Models
{
    public class Baggage
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Вес")]
        public double Weight { get; set; }

        [Required]
        [Display(Name = "Ширина")]
        public double Width { get; set; }

        [Required]
        [Display(Name = "Высота")]
        public double Height { get; set; }

        [Required]
        [Display(Name = "Глубина")]
        public double Depth { get; set; }

        [Required]
        [Display(Name = "Тип")]
        public string Type { get; set; }

        public virtual Passenger Passenger { get; set; }
    }
   
}
