
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class FormCommon
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public DateTime LastUsing { get; set; }
        public int AmountOfUsing { get; set; }
        public int UserId { get; set; }
        public bool Sharing { get; set; }
        public string ImagePath {get; set; }
        public string ImageSrc { get; set; }
    }
}
