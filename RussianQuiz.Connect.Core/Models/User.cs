using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RussianQuiz.Connect.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress, StringLength(250)]
        public string EmailAddress { get; set; }

        [Required, StringLength(200)]
        public string Username { get; set; }

        [StringLength(400)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// See <see cref="Enums.UserIdentityProvider"/>
        /// </summary>
        public string UserIdentityProvider { get; set; }

        public ICollection<ServicePlan> ServicePlans { get; set; }
    }
}