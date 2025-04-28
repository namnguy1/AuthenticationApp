using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace NutritionApp.Application.Commands
{
    public record VerifyEmailCommand(string Token) : IRequest<string>;
}