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
                .WithMessage("Simbolo inválido.");
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThan(100000)
                .WithMessage("Quandidade deve ser maior que 0 e menor que 100000");
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThan(1000)
                .WithMessage("Preço deve ser maior que 0 e menor que 1000");
            RuleFor(x => x.Price)
                .Must(x =>
                    decimal.Round(x * 100, 0) == x * 100)
                .WithMessage("Preço deve conter, no máximo, somente duas casas decimais");
        }
    }
}
