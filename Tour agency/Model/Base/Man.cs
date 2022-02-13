using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency.Model.Base
{
    public abstract class Man
    {
        public abstract int Id { get; set; }

        public abstract string Name { get; set; }

        public abstract string Phone { get; set; }
    }
}
