using Microsoft.EntityFrameworkCore;
using OrderAccumulator.Application.Commands;
using OrderAccumulator.Application.Interfaces;
using OrderAccumulator.Domain.Entities;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Models;
using Polly.Retry;
using System.Collections.Concurrent;

namespace OrderAccumulator.Services
{
    public class ExposureService : IExposureService
    {
        private readonly IExposureRepository _exposureRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ExposureService(IExposureRepository exposureRepository, IUnitOfWork unitOfWork, AsyncRetryPolicy retryPolicy)
        {
            _exposureRepository = exposureRepository;
            _unitOfWork = unitOfWork;
            _retryPolicy = retryPolicy;
        }

        public async Task<ExposureResult> ProcessAsync(OrderRequest order, CancellationToken cancellationToken)
        {
            bool exists = true; 
            var exposure = await _exposureRepository.GetExposureBySymbolAsync(order.Symbol, cancellationToken);
            if (exposure is null)
            {
                exposure = new Exposure(order.Symbol);
                exists = false;
            }

            var result = exposure.ApplyOrder(order);

            if (!result.Accepted)
                return result;
            if (exists)
                await _exposureRepository.Update(exposure);
            else
                await _exposureRepository.Add(exposure);

            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await _unitOfWork.SaveChangeAsync(cancellationToken);
                });
            }
            catch (DbUpdateConcurrencyException)
            {
               throw new InvalidOperationException("Exposure was modified by another process. Please retry.");
            }

            return result;
        }
    }
}
