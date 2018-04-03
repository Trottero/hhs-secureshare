using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.Webapp.Models
{
	public class ShareFileViewModel : Entity
	{
		public Guid FileToShare { get; set; }
        public string Filename { get; set; }
        [Required]
        public Guid UserToShareWith { get; set; }
	}
}