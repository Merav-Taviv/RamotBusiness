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
using System.Drawing.Drawing2D;
using System.Drawing;

namespace HOCR
{
    public static class ImageFilter
    {
        #region Privates

        //colors
        static readonly PixelData Black = new PixelData { B = 0, R = 0, G = 0 };
        static readonly PixelData White = new PixelData { B = 255, R = 255, G = 255 };

        //threshold
        public const double ThresholdLoose = 0.3; //threshold that more close to black (loose)
        public const double ThresholdTight = 0.8; //threshold that more close to white (tight)

        // the range of angles to search for lines
        private const double MinimalRotationAngle = 0.0001;
        private const double CAlphaStart = -20;
        private const double CAlphaStep = 0.001;
        private const int CSteps = 40 * 1000;

        // pre-calculation of sin and cos
        private static double[] _cSinA;
        private static double[] _cCosA;

        // range of d
        private static double _cDMin;
        private const double CdStep = 1.0;
        private static int _cDCount;

        // count of points that fit in a line
        private static int[] _cHMatrix;

        #endregion

        /// <summary>
        /// Get image bitmap and return new image after thresholding.
        /// </summary>
        /// <param name="bitmap">image bitmap</param>
        /// <returns>image after threshold</returns>
        public static Bitmap PerformThreshold(Bitmap bitmap)
        {
            var newBitmap = new UnsafeBitmap(bitmap);
            newBitmap.LockBitmap();

            for (var i = 0; i < bitmap.Height; i++)
            {
                for (var j = 0; j < bitmap.Width; j++)
                {
                    if (newBitmap.GetPixel(j, i).GetBrightness() > ThresholdTight) newBitmap.SetPixel(j, i, White);
                    if (newBitmap.GetPixel(j, i).GetBrightness() < ThresholdLoose) newBitmap.SetPixel(j, i, Black);
                }
            }

            newBitmap.UnlockBitmap();
            return newBitmap.Bitmap;
        }

        /// <summary>
        /// Get image bitmap and return cleaned image with median filter
        /// </summary>
        /// <param name="bitmap">image bitmap</param>
        /// <returns>clean image</returns>
        public static Bitmap PerformClean(Bitmap bitmap)
        {
            var newBitmap = new UnsafeBitmap(bitmap);
            newBitmap.LockBitmap();

            PixelData color;
            var mask = new int[9];
            for (var i = 0; i < 9; i++)
                mask[i] = 1;

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        color = newBitmap.GetPixel(i - 1, j - 1);
                        mask[0] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[0] = 255;

                    if (j - 1 >= 0 && i + 1 < bitmap.Width)
                    {
                        color = newBitmap.GetPixel(i + 1, j - 1);
                        mask[1] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[1] = 255;

                    if (j - 1 >= 0)
                    {
                        color = newBitmap.GetPixel(i, j - 1);
                        mask[2] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[2] = 255;

                    if (i + 1 < bitmap.Width)
                    {
                        color = newBitmap.GetPixel(i + 1, j);
                        mask[3] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[3] = 255;

                    if (i - 1 >= 0)
                    {
                        color = newBitmap.GetPixel(i - 1, j);
                        mask[4] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[4] = 255;

                    if (i - 1 >= 0 && j + 1 < bitmap.Height)
                    {
                        color = newBitmap.GetPixel(i - 1, j + 1);
                        mask[5] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[5] = 255;

                    if (j + 1 < bitmap.Height)
                    {
                        color = newBitmap.GetPixel(i, j + 1);
                        mask[6] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[6] = 255;

                    if (i + 1 < bitmap.Width && j + 1 < bitmap.Height)
                    {
                        color = newBitmap.GetPixel(i + 1, j + 1);
                        mask[7] = Convert.ToInt16(color.GetBrightness() * 255);
                    }
                    else
                        mask[7] = 255;

                    color = newBitmap.GetPixel(i, j);
                    mask[8] = Convert.ToInt16(color.GetBrightness() * 255);

                    Array.Sort(mask);
                    var mid = mask[4];
                    newBitmap.SetPixel(i, j, new PixelData {B = (byte) mid, R = (byte) mid, G = (byte) mid});
                }
            }

            newBitmap.UnlockBitmap();
            return newBitmap.Bitmap;
        }

        /// <summary>
        /// Get image bitmap and rotate it to the right angle that it's straight
        /// </summary>
        /// <param name="bitmap">image bitmap</param>
        /// <returns>straight image</returns>
        public static Bitmap PerformRotation(Bitmap bitmap)
        {
            var unsafeBitmap = new UnsafeBitmap(bitmap);
            Bitmap result;

            unsafeBitmap.LockBitmap();
            var rotationAngle = GetRotationAngle(unsafeBitmap);
            unsafeBitmap.UnlockBitmap();

            if ((rotationAngle > MinimalRotationAngle || rotationAngle < -(MinimalRotationAngle)))
                result = Rotate(unsafeBitmap.Bitmap, -rotationAngle, bitmap.Width / 2, bitmap.Height / 2);
            else
                result = unsafeBitmap.Bitmap;
            return result;
        }

        private static double GetRotationAngle(UnsafeBitmap bitmap)
        {
            var sum = 0.0;
	        var count = 0.0;

	        // perform Hough Transformation
            CalcHoughTransformation(bitmap);

	        // top 50 of the detected lines in the image
	        var hl = GetTopLines(50);

            if (hl.Length < 50)
            {
                return 0; // no rotation will be made
            }
            // average angle of the lines
            for (var i = 0; i < 50; i++)
            {
                sum += hl[i].Alpha;
                count += 1.0;
            }
            return (sum/count);
        }

        private static Bitmap Rotate(Bitmap bitmap, double rotationAngle, int midWidth, int midHeight)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            int minX = 0, minY = 0, maxX = 0, maxY = 0;

            int[] corners = { 0, 0, width, 0, width, height, 0, height };

            var theta = (float)(Math.PI * rotationAngle / 180.0);
            for (var i = 0; i < corners.Length; i += 2)
            {
                var x = (int)(Math.Cos(theta) * (corners[i] - midWidth)
                    - Math.Sin(theta) * (corners[i + 1] - midHeight) + midWidth);
                var y = (int)(Math.Sin(theta) * (corners[i] - midWidth)
                    + Math.Cos(theta) * (corners[i + 1] - midHeight) + midHeight);

                if (x > maxX)
                {
                    maxX = x;
                }

                if (x < minX)
                {
                    minX = x;
                }

                if (y > maxY)
                {
                    maxY = y;
                }

                if (y < minY)
                {
                    minY = y;
                }
            }

            midWidth = (midWidth - minX);
            midHeight = (midHeight - minY);

            var newBitmap = new Bitmap((maxX - minX), (maxY - minY));
            var graphics = Graphics.FromImage(newBitmap);
            graphics.Clear(Color.White);
            graphics.TranslateTransform(-midWidth, -midHeight);
            graphics.RotateTransform((float)(rotationAngle), MatrixOrder.Append);
            graphics.TranslateTransform(midWidth, midHeight, MatrixOrder.Append);
            graphics.DrawImage(bitmap, new Point(0, 0));

            return newBitmap;
        }

        private static HoughLine[] GetTopLines(int linesNum)
        {
            var hl = new HoughLine[linesNum];
            for (var i = 0; i < linesNum; i++)
                hl[i] = new HoughLine();

            for (var i = 0; i < (_cHMatrix.Length - 1); i++)
            {
                if (_cHMatrix[i] <= hl[linesNum - 1].Count) continue;
                hl[linesNum - 1].Count = _cHMatrix[i];
                hl[linesNum - 1].Index = i;
                var j = linesNum - 1;
                while ((j > 0) && (hl[j].Count > hl[j - 1].Count))
                {
                    var tmp = hl[j];
                    hl[j] = hl[j - 1];
                    hl[j - 1] = tmp;
                    j -= 1;
                }
            }

            for (var i = 0; i < (linesNum - 1); i++)
            {
                var dIndex = hl[i].Index / CSteps;
                // remainder
                var alphaIndex = hl[i].Index - dIndex * CSteps;
                hl[i].Alpha = GetAlpha(alphaIndex);
            }

            return hl;
        }

        private static void CalcHoughTransformation(UnsafeBitmap bitmap)
        {
            var hMin = (int)(bitmap.Bitmap.Height / 4.0);
            var hMax = (int)(bitmap.Bitmap.Height * 3.0 / 4.0);

            InitializeHoughCalc(bitmap.Bitmap);

            for (var y = hMin; y < hMax; y++)
            {
                for (var x = 1; x < (bitmap.Bitmap.Width - 2); x++)
                {
                    // only lower edges are considered
                    if (!IsBlack(bitmap, x, y)) continue;
                    if (!IsBlack(bitmap, x, y + 1))
                        CalcAllLinesThroughPoint(x, y);
                }
            }
        }

        /// <summary>
        /// calculate all lines through the point (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void CalcAllLinesThroughPoint(int x, int y)
        {
            for (var alpha = 0; alpha < (CSteps - 1); alpha++) 
            {
	            var d = y * _cCosA[alpha] - x * _cSinA[alpha];
	            var dIndex = (int) (d - _cDMin);
	            var index = dIndex * CSteps + alpha;
		        _cHMatrix[index] += 1;
            }
            return;
        }

        private static void InitializeHoughCalc(Bitmap bitmap)
        {
            // pre-calculation of sin and cos
            _cSinA = new double[CSteps - 1];
            _cCosA = new double[CSteps - 1];

            for (var i = 0; i < (CSteps - 1); i++)
            {
                var angle = GetAlpha(i) * Math.PI / 180.0;
                _cSinA[i] = Math.Sin(angle);
                _cCosA[i] = Math.Cos(angle);
            }

            // range of d
            _cDMin = -bitmap.Width;
            _cDCount = (int) (2.0*((bitmap.Width + bitmap.Height))/CdStep);
            _cHMatrix = new int[_cDCount * CSteps];
        }

        private static double GetAlpha(int i)
        {
            return CAlphaStart + (i * CAlphaStep);
        }

        private static bool IsBlack(UnsafeBitmap bitmap, int x, int y)
        {
            var pixel = bitmap.GetPixel(x, y);
            var luminance = (pixel.R * 0.299) + (pixel.G * 0.587) + (pixel.B * 0.114);
            return luminance < 140;
        }
    }

    /// <summary>
    /// representation of a line in the image
    /// </summary>
    public class HoughLine
    {
        // count of points in the line
        public int Count;
        // index in matrix.
        public int Index;
        // the line is represented as all x, y that solve y * cos(alpha) - x * sin(alpha) = d
        public double Alpha;
    }
}

