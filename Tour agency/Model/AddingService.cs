using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency.Model
{
    [Table("ДобавлениеУслуг")]
    public class AddingService
    {
        [Column("Номер")]
        public int Id { get; set; }

        [Column("НомерКлиента")]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Column("НомерТура")]
        public int TourId { get; set; }
        [ForeignKey("TourId")]
        public Tour Tour { get; set; }

        [Column("НомерУслуги")]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

    }
}
