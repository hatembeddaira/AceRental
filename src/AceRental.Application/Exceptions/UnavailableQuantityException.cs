using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceRental.Application.Exceptions
{
    public class UnavailableQuantityException : Exception
    {
        public UnavailableQuantityException(object key)
            : base($"Le matériel (ID: {key}) n'est pas disponible en quantité suffisante pour ces dates.")
        {
        }

        public UnavailableQuantityException() 
            : base("Un ou plusieurs matériels ne sont pas disponibles en quantité suffisante pour ces dates.")
        {
        }
    }
}