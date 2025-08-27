using CleanArchitecture.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Reviews
{
    public static class ReviewsErrors
    {
        public static readonly Error NotElegible = new Error(
        "Review.NotElegible",
        "No se puede completar el review porque el alquiler no ha finalizado"
    );
    }
}
