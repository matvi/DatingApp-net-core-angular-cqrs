using System;
using System.Collections.Generic;

namespace Common.Entity
{
    public class AppUser
    {
        public int AppUserId { get; set; }
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string NickName { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; }  = DateTime.Now;

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }


        // This method is very usefull when using AutoMapper
        // because Automapper will map The Age property with the method GetAge
        // BUT
        // For flatten object where almost all properties are equal going to the database for 1000 properties
        // and only using 100 in the DTO is not efficient.
        // Instead we will go to the database and retreive only used properties.
        // In order to retreive only used properties given for the DTO we need to delete the GetAge method.
        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }
        
    }

}