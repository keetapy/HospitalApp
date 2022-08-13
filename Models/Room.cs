using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursADO.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int MaxPatients { get; set; }
        public int CurrPatient { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public Room()
        {
            Patients = new List<Patient>();
        }
        public override string ToString()
        {
            return "№" + Id+" "+CurrPatient+"/"+MaxPatients;
        }
    }
}
