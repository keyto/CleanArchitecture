using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Shared
{
    // IEquatable : Evalua cada elemento que se añade a la coleccion
    /// <summary>
    /// Se encarga de administrar una coleccion de tipo dictionary
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {
        private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();
        public int Id{ get; protected init; }
        public string Name{ get; protected init; } = string.Empty;

        public Enumeration(int id,string name)
        {
            Id = id;
            Name = name;
        }


   

        /// <summary>
        /// busca en el dictionario el objeto que tenga ese id  y lo devuelve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TEnum? FromValue(int id)
        {             
            return Enumerations.TryGetValue(id, out TEnum? enumeration) 
                ? enumeration 
                : default;
         }

        /// <summary>
        /// busca en el dictionario el objeto que tenga ese value y lo devuelve
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TEnum? FromName(string name)
        {            
            return Enumerations.Values.SingleOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Devuelve el dictionario como lista
        /// </summary>
        /// <returns></returns>
        public static List<TEnum> GetValues()
        {
            return Enumerations.Values.ToList();
        }

        /// <summary>
        /// Compara 2 objetos para evitar que este duplicado
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Equals(Enumeration<TEnum>? other)
        {
            if (other is null)
            {
                return false;
            }

            // comparo por el tipo de objeto y si tienen el mismo id
            return GetType() == other.GetType() && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<TEnum> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }



        public override string ToString()
        {
            return Name;
        }
        public static Dictionary<int, TEnum> CreateEnumerations()
        {
            // crear un dictionario con reflection.
            // para ello evaluar que tipo de dato es el TEnum.
            var enumerationType = typeof(TEnum);

            // devuelve todas las propiedades de la clase para ver si son publicas ,staticas   o heredadas
            var fieldsForType = enumerationType.GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.FlattenHierarchy
                ).Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldinfo => (TEnum)fieldinfo.GetValue(default)!);

            return fieldsForType.ToDictionary(x => x.Id);

        }
    }
}
