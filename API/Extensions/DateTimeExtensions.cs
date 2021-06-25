using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if(dateOfBirth.Date > DateTime.Today.Date)
            {
                age --;
            }
            return age;
        }
    }
}