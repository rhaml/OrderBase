using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Infrastructure.UnitOfWork
{
    public  class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _context;
        public UnitOfWork(OrderDbContext context)
        {
            _context = context;
        }
        public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
