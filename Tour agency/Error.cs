using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency
{
    class Error : Exception
    {
        private readonly string description;

        public Error(string description)
        {
            this.description = description;
        }

        public override string ToString()
        {
            return description;
        }
    }
}
