using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursADO.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EndDate { get; set; }
        public byte[] Photo { get; set; }
        public string Sex { get; set; }
        public int DiseaseId { get; set; }
        public virtual Disease Disease { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}
