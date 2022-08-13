using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursADO.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }

        public Department()
        {
            Patients = new List<Patient>();
            Categories = new List<Category>();
            Rooms = new List<Room>();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
