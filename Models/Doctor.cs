using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursADO.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        public byte[] Photo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public Doctor()
        {
            Patients = new List<Patient>();
        }
        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}
