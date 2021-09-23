using System;
using System.ComponentModel.DataAnnotations;
using RussianQuiz.Connect.Core.Models.Enums;


namespace RussianQuiz.Connect.Core.Models
{
    public class ServicePlan
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public ServicePlanTypes ServicePlanType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? LastRenewalDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public User User { get; set; }
    }
}