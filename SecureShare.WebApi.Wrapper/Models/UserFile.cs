using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureShare.WebApi.Wrapper.Models
{
    public class UserFile: Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid UserFileId { get; set; }

        public string FileName { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<Users_UserFiles> SharedWith { get; set; }
    }
}
