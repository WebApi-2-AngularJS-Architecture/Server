namespace CompanySystem.Data.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Common.Constants;
    using System.Linq;

    public class User : IdentityUser
    {
        [Required]
        [MaxLength(ValidationConstants.FullNameMaxLength, ErrorMessage = ValidationConstants.FullNameMaxLengthErrorMessage)]
        [MinLength(ValidationConstants.FullNameMinLength, ErrorMessage = ValidationConstants.FullNameMinLengthErrorMessage)]
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }
    }
}