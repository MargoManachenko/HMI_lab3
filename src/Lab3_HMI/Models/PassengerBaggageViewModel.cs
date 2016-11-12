using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Lab3_HMI.Models
{
    public sealed class PassengerBaggageViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        public string Address { get; set; }

        public Flight Flight { get; set; }

        public List<Baggage> Baggage { get; set; }

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

        public PassengerBaggageViewModel()
        {
            Baggage = new List<Baggage>();
        }
    }
}
