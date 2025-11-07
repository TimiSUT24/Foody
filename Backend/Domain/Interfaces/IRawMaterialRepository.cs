using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRawMaterialRepository : IGenericRepository<RawMaterial>
    {
        Task<IEnumerable<RawMaterial>> GetRawMaterialsByFoodIdAsync(int foodId, CancellationToken ct);
        
    }
}
