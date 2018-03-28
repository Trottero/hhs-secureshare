using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureShare.WebApi.Wrapper.Models
{
    public class Users_UserFiles : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PermissionId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserFileId { get; set; }

        public User User { get; set; }
        public UserFile UserFile { get; set; }

        public DateTime ExperingDate { get; set; }
    }
}
