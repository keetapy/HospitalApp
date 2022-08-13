using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursADO.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public Category()
        {
            Diseases = new List<Disease>();
            Doctors = new List<Doctor>();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
