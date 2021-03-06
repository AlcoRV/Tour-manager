using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency.Model
{
    [Table("Продажа")]
    public class Selling
    {
        [Column("Номер")]
        public int Id { get; set; }

        [Column("НомерМенеджера")]
        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }

        [Column("НомерКлиента")]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Column("НомерТура")]
        [ForeignKey("Tour")]
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }

        [Column("Дата")]
        public DateTime Date { get; set; }

    }
}