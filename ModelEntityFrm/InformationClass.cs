using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.ComponentModel;


namespace ModelEntityFrm
{
    [PropertiesMustMatch("Password", "ConfirmPassword")]
    public class InformationClass
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [Display(Name = "Όνομα χρήστη")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Μή έγκυρο email")]
        [MinLength(5)]
        [MaxLength(30)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [StringLength(100, ErrorMessage = "Ο {0} θα πρέπει να είναι τουλάχιστον {2} χαρακτήρες.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Κωδικός")]
        public string Password { get; set; }

        
        //[Compare("Password", ErrorMessage = "Η επιβεβαίωση κωδικού απέτυχε.")]
        //[NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Επιβεβαιώστε κωδικό")]
        [NotMapped]
         public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [Display(Name = "Όνομα")]
        [MaxLength(10)]
        public string name { get; set; }

        [Required(ErrorMessage = "Απαραίτητο πεδίο")]
        [Display(Name = "Επώνυμο")]
        [MaxLength(10)]
        public string surname { get; set; }
    }


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public sealed class PropertiesMustMatchAttribute : ValidationAttribute
{
    private const string _defaultErrorMessage = "'{0}' δεν '{1}' ταιριάζουν ";
    private readonly object _typeId = new object();

    public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
        : base(_defaultErrorMessage)
    {
        OriginalProperty = originalProperty;
        ConfirmProperty = confirmProperty;
    }

    public string ConfirmProperty { get; private set; }
    public string OriginalProperty { get; private set; }

    public override object TypeId
    {
        get
        {
            return _typeId;
        }
    }

    public override string FormatErrorMessage(string name)
    {
        return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
            OriginalProperty, ConfirmProperty);
    }

    public override bool IsValid(object value)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
        object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
        object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
        if (confirmValue == null)
            return true;
        return Object.Equals(originalValue, confirmValue);
    }
}
}
