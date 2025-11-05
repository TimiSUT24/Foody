using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto.Request
{
    public class IngrediensDto
    {
        public int Nummer { get; set; }
        public string Namn { get; set; } = string.Empty;
        public decimal VattenFaktor { get; set; }
        public decimal FettFaktor { get; set; }
        public decimal ViktForeTillagning { get; set; }
        public decimal ViktEfterTillagning { get; set; }
        public string TillangingsFaktor { get; set; } = string.Empty; 

    }
}
