using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_agency.Model.Base;

namespace Tour_agency.Model
{
    [Table("Менеджер")]
    public class Manager : Man
    {
        [Column("Номер")]
        public override int Id { get; set; }

        [Column("ФИО")]
        public override string Name { get; set; }

        [Column("Телефон")]
        public override string Phone { get; set; }

        public List<Selling> Sellings { get; set; }
    }
}
