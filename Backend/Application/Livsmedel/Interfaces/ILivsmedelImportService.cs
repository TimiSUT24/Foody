using Application.Livsmedel.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Interfaces
{
    public interface ILivsmedelImportService
    {
        Task<LivsmedelListResponse> GetLivsmedelListAsync(int offset, int limit, int sprak);
        Task<int> ImportLivsmedelBatchAsync(int offset, int limit, int sprak);
    }
}
