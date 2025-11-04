using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto
{
    public class LivsmedelListResponse
    {      
        public List<LivsmedelDto> Livsmedel { get; set; } = new List<LivsmedelDto>();
    }
}
