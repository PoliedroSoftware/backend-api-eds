using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.FileUploadS3
{
    public class DocumentEvent
    {
        public string BucketName { get; set; } = "";
        public string FolderName { get; set; } = "";
        public string TempPath { get; set; } = "";
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "";
    }
}
