using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Common.Models.Request
{
    public class LocationRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class LocationRequestValidator : AbstractValidator<LocationRequest>
    {
        public LocationRequestValidator()
        {
            RuleFor(col => col.X).NotNull();
            RuleFor(col => col.Y).NotNull();
        }
    }
}
