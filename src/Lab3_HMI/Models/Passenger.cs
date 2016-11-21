using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_HMI.Models
{
    public sealed class Passenger
    {
        public Passenger()
        {
            Baggage = new List<Baggage>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required]
        public Flight Flight { get; set; }

        public List<Baggage> Baggage { get; set; }


    }
}
