using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opg_201910_interview.Models
{
    public class FilesMdl
    {
        public string Word { get; set; }
        public string StartDate { get; set; }
    }

    public class ClientInfoMdl
    {
        public string ClientId { get; set; }
        public string FileDirectoryPath { get; set; }
        public List<FilesMdl> Files { get; set; }
    }

    public class ClientMdl
    {
        public ClientInfoMdl ClientInfoMdl { get; set; }
    }

    public class ClientsMdl
    {
        public ClientMdl ClientA { get; set; }
        public ClientMdl ClientB { get; set; }
    }
}
