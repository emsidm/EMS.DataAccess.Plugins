using System.ComponentModel.DataAnnotations;

namespace EMS.DataAccess.EntityFrameworkCore.Tests.Models
{
    public class SourceUser
    {
        [Key] public int Id { get; set; }

        [Required] public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }
    }
}