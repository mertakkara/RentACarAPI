using FluentValidation;
using RentACarAPI.Application.ViewModels.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Validators.Cars
{
    public class CreateCarValidator:AbstractValidator<VM_Create_Car>
    {
        public CreateCarValidator()
        {
            RuleFor(c => c.Name).NotEmpty().NotNull().WithMessage("Please Enter Product Name").MaximumLength(150).MinimumLength(2).WithMessage("Producy Name can be between 5 and 150 character");
            RuleFor(c => c.Stock).NotEmpty().NotNull().WithMessage("Please Enter Stock");
            RuleFor(c => c.Stock).NotEmpty().NotNull().WithMessage("Please Enter Stock").Must(p => p >= 0).WithMessage("Stock cannot be negative");
            RuleFor(c => c.Price).NotEmpty().NotNull().WithMessage("Please Enter Price").Must(p => p >= 0).WithMessage("Price cannot be negative");

        }
    }
}
