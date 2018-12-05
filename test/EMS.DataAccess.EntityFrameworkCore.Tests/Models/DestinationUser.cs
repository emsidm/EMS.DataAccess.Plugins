using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.DataAccess.EntityFrameworkCore.Tests.Models
{
    public class DestinationUser
    {
        [Key] public Guid Id { get; set; }

        [Required] public string SamAccountName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }
    }
}