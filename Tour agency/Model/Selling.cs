using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency.Model
{
    [Table("ПродажаВывод")]
    public class Selling
    {
        [Column("Номер")]
        public int Id { get; set; }
        [Column("ФИОМенеджера")]
        public string NameManager { get; set; }
        [Column("ФИОКлиента")]
        public string NameClient { get; set; }
        [Column("НазваниеТура")]
        public string NameTour { get; set; }
    }
}