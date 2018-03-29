using System;
using System.ComponentModel.DataAnnotations.Schema;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.Website.ViewModels;

namespace SecureShare.Webapp.Models
{
	public class Users_UserFiles : Entity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid PermissionId { get; set; }

		public Guid UserId { get; set; }
		public Guid UserFileId { get; set; }

		public UserViewModel User { get; set; }
		public UserFileViewModel UserFile { get; set; }

		public DateTime ExpiringDate { get; set; }
	}
}