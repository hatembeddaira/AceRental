using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceRental.Application.Common
{
    public interface DateExtention
    {
        public static int GetTotalDays(DateTime? startDate, DateTime? endDate)
        {
            if (endDate == null || startDate == null)
                return 0;

            if (endDate <= startDate)
            {
                return 0; // Sécurité si les dates sont inversées ou identiques
            }
            TimeSpan difference = (DateTime)endDate - (DateTime)startDate;
            return (int)Math.Ceiling(difference.TotalDays);
        }
    }
}