using Application.Livsmedel.Dto;
using Application.Livsmedel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Service
{
    public class LivsmedelService : ILivsmedelService
    {
        private readonly ILivsmedelImportService _import; 
        public LivsmedelService(ILivsmedelImportService import)
        {
            _import = import;
        }

        

    }
}
