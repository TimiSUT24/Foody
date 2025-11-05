using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto
{
    public class KlassificeringDto
    {
        public string Typ { get; set; } = string.Empty;
        public string Namn { get; set; } = string.Empty;
        public string Fasett { get; set; } = string.Empty;
        public string FasettKod { get; set; } = string.Empty;
        public string Kod { get; set; } = string.Empty;
        public string LangualId { get; set; } = string.Empty;
    }
}
