using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace SecureShare.WebApi.Wrapper.Models
{
	public class UserFile : Entity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public Guid UserFileId { get; set; }

		[Required]
		public string FileName { get; set; }

		[Required]
		public Guid OwnerId { get; set; }

		public User Owner { get; set; }

		public ICollection<Users_UserFiles> SharedWith { get; set; }

		public DateTime UploadDateTime { get; set; }
		public DateTime DownloadDateTime { get; set; }
		public string Description { get; set; }

		[Required]
		public Guid BlobId { get; set; }

		public FileStream FileStream { get; set; }
	}
}