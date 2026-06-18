using Microsoft.EntityFrameworkCore;
using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Domain.Entities;
using OrderAccumulator.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Infrastructure.Repositories
{
    public  class ExposureRepository : IExposureRepository
    {
        private readonly OrderDbContext _context;

        public ExposureRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Exposure> GetExposureBySymbolAsync(string symbol, CancellationToken cancellationToken)
        {
            return await _context.Exposures.SingleOrDefaultAsync(e => e.Symbol == symbol, cancellationToken);
        }

        public Task Add(Exposure exposure)
        {
            _context.Exposures.Add(exposure);
            return Task.CompletedTask;
        }

        public Task Update(Exposure exposure)
        {
            _context.Exposures.Update(exposure);
            return Task.CompletedTask;
        }
    }
}
