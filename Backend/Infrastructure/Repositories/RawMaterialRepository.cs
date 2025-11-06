using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RawMaterialRepository : GenericRepository<RawMaterial>, IRawMaterialRepository
    {
        private readonly FoodyDbContext _context;
        public RawMaterialRepository(FoodyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
