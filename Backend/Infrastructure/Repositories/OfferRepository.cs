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
    public class OfferRepository : GenericRepository<Offer>, IOfferRepository
    {
        private readonly FoodyDbContext _context;

        public OfferRepository(FoodyDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
