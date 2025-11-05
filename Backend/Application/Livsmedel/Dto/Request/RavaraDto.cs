using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto.Request
{
    public class RavaraDto
    {
        public string Namn { get; set; } = string.Empty;
        public string Tillagning { get; set; } = string.Empty;
        public string FoodEx2 { get; set; } = string.Empty;
        public decimal Andel { get; set; }
        public decimal Faktor { get; set; }
        public decimal OmraknadTillRa { get; set; }
    }
}
