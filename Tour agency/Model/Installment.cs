using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency.Model
{
    [Table("Рассрочка")]
    public class Installment
    {

        [Column("НомерКлиента")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Column("НомерТура")]
        public int TourId { get; set; }
        public Tour Tour { get; set; }

        [Column("Дата")]
        public DateTime Date { get; set; }

        [Column("Срок")]
        public int Period { get; set; }
    }
}
