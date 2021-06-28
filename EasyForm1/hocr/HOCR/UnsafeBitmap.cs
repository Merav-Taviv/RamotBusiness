/*
* This file is Part of a HOCR
* LGPLv3 Licence:
*       Copyright (c) 2011 
*          Niv Maman [nivmaman@yahoo.com]
*          Maxim Drabkin [mdrabkin@gmail.com]
*          Hananel Hazan [hhazan01@CS.haifa.ac.il]
*          University of Haifa
* This Project is part of our B.Sc. Project course that under supervision
* of Hananel Hazan [hhazan01@CS.haifa.ac.il]
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without 
* modification, are permitted provided that the following conditions are met:
*
* 1. Redistributions of source code must retain the above copyright notice, this list of conditions 
*    and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright notice, this list of 
*    conditions and the following disclaimer in the documentation and/or other materials provided
*    with the distribution.
* 3. Neither the name of the <ORGANIZATION> nor the names of its contributors may be used to endorse
*    or promote products derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
* ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
* SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
* CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
* DAMAGE.
*/

using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace HOCR
{
    public unsafe class UnsafeBitmap
    {
        readonly Bitmap _bitmap;

        private int _localWidth;
        BitmapData _bitmapData;
        Byte* _pBase = null;

        public int Width { get { return _bitmap.Width; } }
        public int Height { get { return _bitmap.Height; } }

        public UnsafeBitmap(string path)
        {
            _bitmap = new Bitmap(path);
        }

        public UnsafeBitmap(Bitmap bitmap)
        {
            _bitmap = new Bitmap(bitmap);
        }

        public UnsafeBitmap(int width, int height)
        {
            _bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        public void Dispose()
        {
            _bitmap.Dispose();
        }

        public Bitmap Bitmap
        {
            get
            {
                return (_bitmap);
            }
        }

        private Point PixelSize
        {
            get
            {
                var unit = GraphicsUnit.Pixel;
                var bounds = _bitmap.GetBounds(ref unit);
                return new Point((int)bounds.Width, (int)bounds.Height);
            }
        }

        public PixelData GetPixel(int x, int y)
        {
            var returnValue = *PixelAt(x, y);
            return returnValue;
        }

        public void SetPixel(int x, int y, PixelData color)
        {
            var pixel = PixelAt(x, y);
            *pixel = color;
        }

        public void SetPixel(int x, int y, Color color)
        {
            var pixel = PixelAt(x, y);
            var pixelData = new PixelData {R = color.R, G = color.G, B = color.B};
            *pixel = pixelData;
        }

        private PixelData* PixelAt(int x, int y)
        {
            return (PixelData*)(_pBase + y * _localWidth + x * sizeof(PixelData));
        }

        public void LockBitmap()
        {
            var unit = GraphicsUnit.Pixel;
            var boundsF = _bitmap.GetBounds(ref unit);
            var bounds = new Rectangle((int)boundsF.X,
           (int)boundsF.Y,
           (int)boundsF.Width,
           (int)boundsF.Height);
            _localWidth = (int)boundsF.Width * sizeof(PixelData);
            if (_localWidth % 4 != 0)
            {
                _localWidth = 4 * (_localWidth / 4 + 1);
            }
            _bitmapData =
           _bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            _pBase = (Byte*)_bitmapData.Scan0.ToPointer();
        }

        public void UnlockBitmap()
        {
            _bitmap.UnlockBits(_bitmapData);
            _bitmapData = null;
            _pBase = null;
        }
    }

    public struct PixelData
    {
        public byte R;
        public byte G;
        public byte B;

        public double GetBrightness()
        {
            return (double)(R + G + B)/765;
        }
    }
}
