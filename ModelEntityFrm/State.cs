using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEntityFrm
{

    public class State
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [Display(Name = "Νομός")]
        public string description{get;set;}

       // [NotMapped]
       // public bool isSelected { get; set; }

    }
}
