using FluentValidation;
using OrderAccumulator.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Application.Validators
{
    public  class ProcessOrderCommandValidator : AbstractValidator<ProcessOrderCommand>
    {
        public ProcessOrderCommandValidator() { 
            RuleFor(x => x.Symbol)
                .Must(x =>
                    x == "PETR4" ||
                    x == "VALE3" ||
                    x == "VIIA4"
                )
                .WithMessage("Invalid stock symbol.");
           
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThan(100000)
                .WithMessage("Invalid order quantity. Must be a positive number or less than 100000.");
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThan(1000)
                .WithMessage("Invalid order price. Must be a positive number or less than 1000.");
            RuleFor(x => x.Price)
                .Must(x =>
                    decimal.Round(x * 100, 0) == x * 100)
                .WithMessage("Invalid order price. Should only have up to two decimal places.."); ;
        }
    }
}
