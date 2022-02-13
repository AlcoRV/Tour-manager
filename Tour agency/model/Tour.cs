using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency.Model
{
    [Table("Тур")]
    public class Tour
    {
        [Column("Номер")]
        [Key]
        public int Id { get; set; }
        [Column("Название")]
        public string Name { get; set; }
        [Column("Страна")]
        public string State { get; set; }
        [Column("Город")]
        public string City { get; set; }
        [Column("КоличествоНочей")]
        public int Nights { get; set; }
        [Column("КоличествоЧеловек")]
        public int Men { get; set; }
        [Column("Описание")]
        public string Description { get; set; }
        [Column("Стоимость")]
        public decimal Price { get; set; }
        [Column("КрайняяДатаПокупки")]
        public DateTime LastData { get; set; }

        //    [ForeignKey("НомерТура")]
        public virtual ICollection<Selling> Sellings { get; set; }
    }
}
