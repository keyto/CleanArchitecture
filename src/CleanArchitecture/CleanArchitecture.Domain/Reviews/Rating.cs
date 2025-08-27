using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Reviews
{ 
    /*Object Value, solo puede ser de 1-5 esa logica se escribe aqui */
    public sealed record Rating 
    {
        public static readonly Error Invalid = new("Rating.Invalid", "El rating es invalido");

        public int Value { get; init; }

        private Rating(int value) => Value = value;

        public static Result<Rating> Create(int value) {
            if (value<1 || value >5)
            {
                return Result.Failure<Rating>(Invalid);
            }

            var rating = new Rating(value);
            return rating;
        }
    }
}
