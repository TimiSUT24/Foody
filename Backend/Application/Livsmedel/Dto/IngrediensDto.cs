using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto
{
    public class IngrediensDto
    {
        public string Namn { get; set; } = string.Empty;
        public decimal VattenFaktor { get; set; }
    }
}
