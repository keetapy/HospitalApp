using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursADO.Models
{
    public class Admin
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
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
