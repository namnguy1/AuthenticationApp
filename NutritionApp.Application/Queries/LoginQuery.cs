using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace NutritionApp.Application.Queries
{
    public record LoginQuery(string Email, string Password) : IRequest<string>;
}