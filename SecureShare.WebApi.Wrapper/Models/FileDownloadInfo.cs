using System;
using System.Collections.Generic;
using System.Text;

namespace SecureShare.WebApi.Wrapper.Models
{
    public class FileDownloadInfo
    {
        public string RootPath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
