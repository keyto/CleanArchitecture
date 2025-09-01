using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Reviews.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.Reviews
{
    public sealed class Review : Entity<ReviewId>
    {
        public ReviewId? Id { get; private set; }
        public VehiculoId? VehiculoId{ get; private set; }
        public AlquilerId? AlquilerId { get; private set; }
        public UserId? UserId { get; private set; }
        public Rating? Rating { get; private set; }                      /*Object Value, solo puede ser de 1-5 esa logica va al OV */

        public Comentario? Comentario{ get; private set; }              /*Object Value*/
        public DateTime? FechaCreacion{ get; private set; }

        private Review()
        {
        }
        private Review(ReviewId id, VehiculoId vehiculoId, AlquilerId alquilerId, UserId userId, Rating rating, Comentario? comentario, DateTime? fechaCreacion) 
            : base(id)
        {
            VehiculoId = vehiculoId;
            AlquilerId = alquilerId;
            UserId = userId;
            Rating = rating;
            Comentario = comentario;
            FechaCreacion = fechaCreacion;
        }

        public static Result<Review> Create(Alquiler alquiler, Rating rating, Comentario comentario,DateTime fechaCreacion) 
        {
            if (alquiler.Status != AlquilerStatus.Completado)
            {
                return Result.Failure<Review>(ReviewsErrors.NotElegible);
            }

            var review = new Review(ReviewId.New(),alquiler.VehiculoId!,alquiler.Id!,alquiler.UserId!,rating,comentario,fechaCreacion);

            review.RaiseDomainEvent(new ReviewCreateDomainEvent(review.Id!));
            return review;
        }
    }
}
