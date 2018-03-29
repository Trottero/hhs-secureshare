using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.Webapp.Models
{
	public class UserViewModel
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public Guid UserId { get; set; }

		public string DisplayName { get; set; }

		//public IEnumerable<UserFile> Files { get; set; }
		public ICollection<Users_UserFiles> FilesSharedWithUser { get; set; }
	}
}