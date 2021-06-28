using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class Files
    {
        public int FileId { get; set; }
        public int FormId { get; set; }
        public string FileName { get; set; }
        public int LocalX { get; set; }
        public int LocalY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Forms Form { get; set; }
    }
}
