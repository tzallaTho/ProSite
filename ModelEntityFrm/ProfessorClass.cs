using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEntityFrm
{
    public  class ProfessorClass
    {
         [Key]
        public int Id { get; set; }
        public InformationClass info { get; set; }
        public bool enabled;

       [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [Display(Name = "Αριθμός τηλεφώνου")]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Μή έγκυρος αριθμός τηλεφώνου")]
        public string phoneNumber { get; set; }


       [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [Display(Name = "Αφμ")]
        [MaxLength(9)]
        [RegularExpression(@"^( )*\d{9}( )*$", ErrorMessage = "Λάθος ΑΦΜ")]
        public string AFM { get; set; }


       public State _state { get; set; }
      
         [Required(ErrorMessage = "Απαραίτητο πεδίο")]
         [Display(Name = "Περιοχή")]
       public string Region;
    }

}
