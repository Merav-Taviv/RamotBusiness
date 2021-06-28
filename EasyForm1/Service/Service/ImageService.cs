using Repository;
using Repository.Models;
using Service.Iservice;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Service.Service
{
    class ImageService : IImageService
    {

        private IFileRepository repository;
        public ImageService(IFileRepository repository)
        {
            this.repository = repository;
        }



        static RectangleF rectangle;
        static Bitmap img;
        static Bitmap smallImg;

        public void CreateSubImage(string path, int formId)
        {
            int counter = 1;
            img = (Bitmap)Image.FromFile(path);
            ChangeImageToBlackAndWhite();
            List<Files> FilesList = new List<Files>();
         //   FilesList = repository.GetFilesByForm(formId);
            foreach (Files item in FilesList)
            {
                rectangle = new RectangleF(item.LocalX, item.LocalY, item.Width, item.Height);
                smallImg = img.Clone(rectangle, img.PixelFormat);
                smallImg.Save(@"F:\" + counter + ".png");
                counter++;
            }
        }

        public void ChangeImageToBlackAndWhite()
        {
            int darkestPixel = 0;
            for (int y = 0; y < img.Width; y++)
            {
                for (int x = 0; x < img.Height; x++)
                {
                    if ((img.GetPixel(y,x).ToArgb()) > darkestPixel)
                        darkestPixel = img.GetPixel(y, x).ToArgb();
                }
            }

            for (int y = 0; y < img.Width - 1; y++)
            {
                for (int x = 0; x < img.Height - 1; x++)
                {
                    if (img.GetPixel(y, x).ToArgb() <= darkestPixel - 100)
                        img.SetPixel(y, x, Color.Black);
                    else
                        img.SetPixel(y, x, Color.White);
                }

            }

        }
    }

}

