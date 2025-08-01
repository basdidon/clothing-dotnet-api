using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Products.List
{
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0");

            RuleFor(x => x.CategoryName)
                .Empty()
                .When(x => x.CategoryId.HasValue)
                .WithMessage("CategoryName must be empty when CategoryId is provided");
        }
    }
}
