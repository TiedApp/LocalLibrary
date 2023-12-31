using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalShared
{
    public class RequestCode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AppId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public bool Used { get; set; }
        [NotMapped]
        public bool Success { get; set; }
        [NotMapped]
        public string Message { get; set; }
    }
}
