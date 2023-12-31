using System;
using System.ComponentModel.DataAnnotations;

namespace GlobalShared
{
    public class ServerLibLog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Companyname { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string Activity { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
