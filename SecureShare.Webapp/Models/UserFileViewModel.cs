using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.Website.Models;
using Users_UserFiles = SecureShare.WebApi.Wrapper.Models.Users_UserFiles;

namespace SecureShare.Website.ViewModels
{
    public class UserFileViewModel: Entity
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
