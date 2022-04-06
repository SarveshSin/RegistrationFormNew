using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RegistrationFormNew.Models
{
    public class StudentData
    {
        public int id { get; set; }
        [Required(ErrorMessage ="This field is Required")]

        public string FirstName { get; set; }
        [Required ( ErrorMessage ="This field is Required")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }
        public byte[] Resume { get; set; }
        [Required]
        public string JobExprience { get; set; }
        
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
    }
}