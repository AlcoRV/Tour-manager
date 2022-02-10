using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency.Model
{
    [Table("ДопУслуга")]
    public class Service
    {
        [Column("Номер")]
        public int Id { get; set; }
        [Column("Название")]
        public string Name { get; set; }
        [Column("Стоимость")]
        public decimal Price { get; set; }
    }
}
