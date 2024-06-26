﻿using System;

namespace POS.Helper
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset,
          DateTimeOffset? dateOfDeath)
        {
           // var dateToCalculateTo = DateTime.UtcNow;
            var dateToCalculateTo = DateTime.Now;

            if (dateOfDeath != null)
            {
                dateToCalculateTo = dateOfDeath.Value.UtcDateTime;
            }

            int age = dateToCalculateTo.Year - dateTimeOffset.Year;

            if (dateToCalculateTo < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
