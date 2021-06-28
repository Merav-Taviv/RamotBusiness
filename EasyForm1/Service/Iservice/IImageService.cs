using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Service.Iservice
{
    public interface IImageService
    {
        void CreateSubImage(string img, int formId);
        
    }
}
