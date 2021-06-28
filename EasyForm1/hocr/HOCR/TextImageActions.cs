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
using System.Collections.Generic;
using System.Drawing;

namespace HOCR
{
    /// <summary>
    /// Static class that contains methods to to deal with Bitmaps.
    /// like copying, getting specific line or letter and more.
    /// </summary>
    public static class TextImageActions
    {
        private const double MekademFixCoef = 0.6;
        private const double MekademLetterSpace = 1.5;

        #region Basic

        /// <summary>
        /// Get pixel color and return number that indicates 
        /// if it greater than threshold (0) or not (1)
        /// </summary>
        /// <param name="color">pixel color</param>
        /// <returns>indicates if it greater than threshold </returns>
        public static double IsGreaterThanThreshold(Color color)
        {
            return color.GetBrightness() > ImageFilter.ThresholdLoose ? 0 : 1;
        }

        /// <summary>
        /// Get Bitmap of text and his bounds and return the Bitmap
        /// separated into lines and letters.
        /// if failed, return null.
        /// </summary>
        /// <param name="bitmap">Bitmap of text</param>
        /// <param name="bounds">bounds of lines and letters</param>
        /// <param name="letterSize">size of letter</param>
        /// <returns>array of lines and letters</returns>
        public static Bitmap[][] RetrieveText(Bitmap bitmap, int[][][] bounds, int letterSize=0)
        {
            //declare text to return
            var textToReturn = new Bitmap[bounds.Length][];

            //for each line get his letters
            for (var i = 1; i <= bounds.Length; i++)
            {
                var line = GetLine(bitmap, ConvertToLinesBounds(bounds), i);
                var letterBounds = ConvertToLetterBounds(bounds, i);
                var numberOfLettersInLine = letterBounds.Length;
                var letters = new Bitmap[numberOfLettersInLine];

                //for each letter save into lines to return
                for (var j = 1; j <= numberOfLettersInLine; j++)
                {
                    try
                    {
                        var letter = (GetLetter(line, letterBounds, j));
                        if (letterSize != 0)
                            letters[j - 1] = ResizeBitmap(letter, letterSize, letterSize);
                        else
                            letters[j - 1] = letter;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                textToReturn[i - 1] = letters;
            }
            return textToReturn;
        }

        /// <summary>
        /// Get lettet Bitmap and return it's Bitmap centerized
        /// </summary>
        /// <param name="bitmap">letter Bitmap</param>
        /// <returns>centerized Bitmap</returns>
        public static Bitmap Centerize(Bitmap bitmap)
        {
            var firstRow = FindFirstRowIndex(bitmap);
            var lastRow = FindLastRowIndex(bitmap);
            var firstCol = FindFirstColIndex(bitmap);
            var lastCol = FindLastColIndex(bitmap);
            var colSize = lastCol - firstCol + 1;
            var rowSize = lastRow - firstRow + 1;
            
            //case of sapce
            if (firstRow == -1 || lastRow == -1 || firstCol == -1 || lastCol == -1)
                return bitmap;
            
            //build new Bitmap
            var newBitmap = new UnsafeBitmap(colSize, rowSize);
            newBitmap.LockBitmap();
            for (var i = 0; i < rowSize; i++)
                for (var j = 0; j < colSize; j++)
                    newBitmap.SetPixel(j, i, bitmap.GetPixel(j + firstCol, i + firstRow));
            newBitmap.UnlockBitmap();
            return newBitmap.Bitmap;
        }

        /// <summary>
        /// Get bitmap and return 3D int array of bounds.
        /// [line][letter][top,bottom,right,left]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static int[][][] GetAllLettersBounds(Bitmap bitmap)
        {
            var lineBounds = GetLinesBounds(bitmap);
            var allLetterBounds = new List<int[][]>();
            for (var i = 0; i < lineBounds.Length; i++)
            {
                var line = GetLine(bitmap, lineBounds, i+1);
                var letterBounds = GetLettersBounds(line, ImageFilter.ThresholdTight);
                if (letterBounds == null) continue;
                letterBounds = FixLetterBounds(line, letterBounds);

                //create the current letter bounds
                var currentLetterBounds = new int[letterBounds.Length][];
                for (var j = 0; j < letterBounds.Length; j++)
                    currentLetterBounds[j] = new[]
                    {
                        lineBounds[i][0],
                        lineBounds[i][1],
                        letterBounds[j][0],
                        letterBounds[j][1]
                    };
                allLetterBounds.Add(currentLetterBounds);
            }
            return allLetterBounds.ToArray();
        }

        /// <summary>
        /// Get bounds of bitmap in [line][letter][down,up,left,right] format
        /// and return lines bounds of it.
        /// </summary>
        /// <param name="bounds">bounds of bitmap</param>
        /// <returns>lines bounds</returns>
        private static int[][] ConvertToLinesBounds(int[][][] bounds)
        {
            var lineBounds = new int[bounds.Length][];
            for (var i = 0; i < bounds.Length; i++)
                lineBounds[i] = new[] { bounds[i][0][0], bounds[i][0][1] };
            return lineBounds;
        }

        /// <summary>
        /// Get bounds of bitmap in [line][letter][down,up,left,right] format
        /// and line number. and return letter bounds of the given line.
        /// </summary>
        /// <param name="bounds">bounds of bitmap</param>
        /// <param name="lineNumber">number of line</param>
        /// <returns>letters bounds</returns>
        private static int[][] ConvertToLetterBounds(int[][][] bounds, int lineNumber)
        {
            var lineBounds = bounds[lineNumber - 1];
            var letterBounds = new int[lineBounds.Length][];
            for (var i = 0; i < lineBounds.Length; i++)
                letterBounds[i] = new[] { lineBounds[i][2], lineBounds[i][3] };
            return letterBounds;
        }

        /// <summary>
        /// Get Bitmap and return new Bitmap, padded with zeros
        /// </summary>
        /// <param name="bitmap">image</param>
        /// <returns>padded image</returns>
        private static Bitmap PadImage(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;
            var size = width > height ? width : height;
            var newBitmap = new UnsafeBitmap(size, size);
            newBitmap.LockBitmap();
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    newBitmap.SetPixel(j, i, Color.White);
            for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                    newBitmap.SetPixel(j, i, bitmap.GetPixel(j, i));
            newBitmap.UnlockBitmap();
            return newBitmap.Bitmap;
        }

        /// <summary>
        /// Get Bitmap and return it with white frame.
        /// It used to avoid problems in recognition where a letter
        /// extend to the edge of frame.
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns>fixed Bitmap</returns>
        public static Bitmap PadImageFrame(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;
            var newBitmap = new UnsafeBitmap(width + 2, height + 2);
            newBitmap.LockBitmap();
            for (var i = 0; i < height + 2; i++)
                for (var j = 0; j < width + 2; j++)
                    newBitmap.SetPixel(j, i, Color.White);
            for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                    newBitmap.SetPixel(j + 1, i + 1, bitmap.GetPixel(j, i));
            newBitmap.UnlockBitmap();
            return newBitmap.Bitmap;
        }

        /// <summary>
        /// Get Bitmap and new Bitmap size,
        /// and return new Bitmap in that size.
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="width">new width</param>
        /// <param name="height">new height</param>
        /// <returns></returns>
        public static Bitmap ResizeBitmap(Bitmap bitmap, int width, int height)
        {
            bitmap = PadImage(Centerize(bitmap));
            var newImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(bitmap, 0, 0, width, height);
            return newImage;
        }

        /// <summary>
        /// Get Bitmap and return resized Bitmap with maximum size as parameter.
        /// the Bitmap stay in the current width:height ratio
        /// </summary>
        /// <param name="bitmap">Bitmap to resize</param>
        /// <param name="newWidth">maximum new width</param>
        /// <param name="newHeight">maximum new height</param>
        /// <returns>new Bitmap</returns>
        public static Bitmap ResizeImageUntillMax(Bitmap bitmap, int newWidth, int newHeight)
        {
            //bitmap = Centerize(bitmap);
            var ratio = (double)bitmap.Width / bitmap.Height;
            if (newHeight > newWidth / ratio)
                newHeight = (int)(newWidth / ratio);
            else
                newWidth = (int)(newHeight * ratio);
            var newBitmap = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newBitmap))
                graphics.DrawImage(bitmap, 0, 0, newWidth, newHeight);
            return newBitmap;
        }

        /// <summary>
        /// Get width and height and return white Bitmap in that size
        /// </summary>
        /// <param name="width">width of Bitmap</param>
        /// <param name="height">height of Bitmap</param>
        /// <returns>new white Bitmap</returns>
        public static Bitmap WhiteBitmap(int width, int height)
        {
            var bitmap = new UnsafeBitmap(width, height);
            bitmap.LockBitmap();
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    bitmap.SetPixel(i, j, Color.White);
            bitmap.UnlockBitmap();
            return bitmap.Bitmap;
        }
        
        /// <summary>
        /// Get Bitmap of letter and return array of letter parameters:
        /// relative location of upper and lower pixel and length of letter.
        /// </summary>
        /// <param name="bitmap">letter Bitmap</param>
        /// <returns>letter parameters</returns>
        public static double[] GetLetterParameters(Bitmap bitmap)
        {
            //Upper,Lower,Length
            var parameters = new double[3];
            parameters[0] = Math.Round((double)FindFirstRowIndex(bitmap) / bitmap.Height);
            parameters[1] = Math.Round((double)FindLastRowIndex(bitmap) / bitmap.Height);
            parameters[2] = parameters[1] - parameters[0];
            return parameters;
        }

        #endregion

        #region Centerize

        private static int FindFirstRowIndex(Bitmap bitmap)
        {
            var newBitmap = new UnsafeBitmap(bitmap);
            newBitmap.LockBitmap();
            var width = newBitmap.Width;
            var height = newBitmap.Height;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (newBitmap.GetPixel(j, i).GetBrightness() >= ImageFilter.ThresholdTight) continue;
                    newBitmap.UnlockBitmap();
                    return i;
                }
            }
            newBitmap.UnlockBitmap();
            return -1;
        }

        private static int FindLastRowIndex(Bitmap bitmap)
        {
            var newBitmap = new UnsafeBitmap(bitmap);
            newBitmap.LockBitmap();
            var width = newBitmap.Width;
            var height = newBitmap.Height;
            for (var i = height - 1; i >= 0; i--)
            {
                for (var j = 1; j < width; j++)
                {
                    if (newBitmap.GetPixel(j, i).GetBrightness() >= ImageFilter.ThresholdTight) continue;
                    newBitmap.UnlockBitmap();
                    return i;
                }
            }
            newBitmap.UnlockBitmap();
            return -1;
        }

        private static int FindFirstColIndex(Bitmap bitmap)
        {
            var newBitmap = new UnsafeBitmap(bitmap);
            newBitmap.LockBitmap();
            var width = newBitmap.Width;
            var height = newBitmap.Height;
            for (var j = 0; j < width; j++)
            {
                for (var i = 0; i < height; i++)
                {
                    if (newBitmap.GetPixel(j, i).GetBrightness() >= ImageFilter.ThresholdTight) continue;
                    newBitmap.UnlockBitmap();
                    return j;
                }
            }
            newBitmap.UnlockBitmap();
            return -1;
        }

        private static int FindLastColIndex(Bitmap bitmap)
        {
            var newBitmap = new UnsafeBitmap(bitmap);
            newBitmap.LockBitmap();
            var width = newBitmap.Width;
            var height = newBitmap.Height;
            for (var j = width - 1; j >= 0; j--)
            {
                for (var i = 0; i < height; i++)
                {
                    if (newBitmap.GetPixel(j, i).GetBrightness() >= ImageFilter.ThresholdTight) continue;
                    newBitmap.UnlockBitmap();
                    return j;
                }
            }
            return -1;
        }

        #endregion

        #region Lines

        /// <summary>
        /// Get Bitmap, bounds of lines and number of line we want,
        /// and return a Bitmap of the line
        /// </summary>
        /// <param name="bitmap">Bitmap of full text</param>
        /// <param name="bounds">bounds of lines</param>
        /// <param name="number">number of line we want</param>
        /// <returns>Bitmap of the line</returns>
        private static Bitmap GetLine(Bitmap bitmap,IList<int[]> bounds, int number)
        {
            var width = bitmap.Width;
            var start = bounds[number - 1][0];
            var end = bounds[number - 1][1];
            var newLine = new UnsafeBitmap(width, end - start + 1);
            newLine.LockBitmap();
            for (var i = start; i <= end; i++)
                for (var j = 0; j < width; j++)
                    newLine.SetPixel(j, i - start, i != bitmap.Height ? bitmap.GetPixel(j, i) : Color.White);
            newLine.UnlockBitmap();
            return newLine.Bitmap;
        }

        /// <summary>
        /// Get Bitmap of full text and return array of int
        /// that indicates if the Bitmap line is part of a line (1) or space (0)
        /// </summary>
        /// <param name="bitmap">full text Bitmap</param>
        /// <returns>array of int</returns>
        private static int[] CreateLinesBounds(Bitmap bitmap)
        {
            var newBitmap = new UnsafeBitmap(bitmap);
            newBitmap.LockBitmap();
            var width = newBitmap.Width;
            var height = newBitmap.Height;
            var lineOrSpace = new int[height];
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (newBitmap.GetPixel(j, i).GetBrightness() > ImageFilter.ThresholdTight)
                    {
                        lineOrSpace[i] = 0;
                        continue;
                    }
                    lineOrSpace[i] = 1;
                    break;
                }
            }
            newBitmap.UnlockBitmap();
            return lineOrSpace;
        }

        /// <summary>
        /// Get array of int and return the number of lines
        /// </summary>
        /// <param name="lineOrSpace">array of int</param>
        /// <returns>number of lines</returns>
        private static int CountLines(IList<int> lineOrSpace)
        {
            var linesNumber = 0;
            var length = lineOrSpace.Count;
            for (var i = 0; i < length; i++)
            {
                if (lineOrSpace[i] == 0) continue;
                while (i < length && lineOrSpace[i] == 1)
                {
                    i++;
                }
                linesNumber++;
            }
            return linesNumber;
        }

        /// <summary>
        /// Get array of int and the number of lines in Bitmap,
        /// and return array of bound pairs (begin and end)
        /// </summary>
        /// <param name="lineOrSpace">array of int</param>
        /// <param name="numberOfLines">number of lines</param>
        /// <returns>pairs of bounds</returns>
        private static int[][] CalcLinesBounds(IList<int> lineOrSpace, int numberOfLines)
        {
            var linesBounds = new int[numberOfLines][];
            var length = lineOrSpace.Count;
            var currentLine = 0;
            for (var i = 0; i < length; i++)
            {
                if (lineOrSpace[i] == 0) continue;
                var start = i;
                while (i < length && lineOrSpace[i] == 1)
                {
                    i++;
                }
                var end = i - 1;
                int[] bound;
                if (start>0 && end<length)
                {
                    bound = new[] { start-1, end+1 };
                }
                else
                {
                    bound = new[] { start, end };
                }
                linesBounds[currentLine] = bound;
                currentLine++;
            }
            return linesBounds;
        }

        /// <summary>
        /// Get Bitmap, calc the bounds and return it
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns>pairs of bounds</returns>
        private static int[][] GetLinesBounds(Bitmap bitmap)
        {
            var lineOrSpace = CreateLinesBounds(bitmap);
            var numberOfLines =  CountLines(lineOrSpace);
            return CalcLinesBounds(lineOrSpace, numberOfLines);
        }

        #endregion

        #region Letters

        /// <summary>
        /// Get line Bitmap and return number of letters in it
        /// </summary>
        /// <param name="line">line Bitmap</param>
        /// <param name="threshold">threshold value</param>
        /// <returns>number of letters in line</returns>
        private static int CountLetters(Bitmap line, double threshold)
        {
            var letterOrSpace = CalcLetterOrSpace(line, threshold);
            var lettersNumber = 0;
            var length = letterOrSpace.Length;
            for (var i = 0; i < length; i++)
            {
                if (letterOrSpace[i] == 0) continue;
                while (i < length && letterOrSpace[i] == 1)
                {
                    i++;
                }
                lettersNumber++;
            }
            return lettersNumber;
        }

        /// <summary>
        /// Get line Bitmap, array of bounds pairs, number of line,
        /// and return the letter we asked in the line
        /// </summary>
        /// <param name="line">Bitmap of line</param>
        /// <param name="bounds">array of bounds pairs</param>
        /// <param name="number">number of letter we want</param>
        /// <returns>Bitmap of letter</returns>
        private static Bitmap GetLetter(Bitmap line, IList<int[]> bounds, int number)
        {
            var newLine = new UnsafeBitmap(line);
            newLine.LockBitmap();
            var boundsNumber = bounds.Count;
            var height = newLine.Height;
            var start = bounds[boundsNumber - number][0];
            var end = bounds[boundsNumber - number][1];
            var letter = new UnsafeBitmap(end - start + 1, height);
            letter.LockBitmap();
            for (var i = 0; i < height; i++)
                for (var j = start; j <= end; j++)
                    letter.SetPixel(j - start, i, newLine.GetPixel(j, i));
            letter.UnlockBitmap();
            newLine.UnlockBitmap();
            return letter.Bitmap;
        }

        /// <summary>
        /// Get line and number of letters and return
        /// array of letters bounds pairs
        /// </summary>
        /// <param name="line">Bitmap of line</param>
        /// <param name="threshold">threshold value</param>
        /// <returns>array of letters bounds pairs</returns>
        private static int[][] CalcLettersBounds(Bitmap line, double threshold)
        {
            var letterOrSpace = CalcLetterOrSpace(line, threshold);
            var bounds = new List<int[]>();
            var length = letterOrSpace.Length;
            for (var i = 0; i < length; i++)
            {
                if (letterOrSpace[i] == 0) continue;
                var start = i;
                while (i < length && letterOrSpace[i] == 1)
                {
                    i++;
                }
                var end = i - 1;
                if (start >= end) continue;
                int[] bound;
                if (start > 0 && end < length)
                    bound = new[] {start - 1, end};
                else
                    bound = new[] {start, end};
                bounds.Add(bound);
            }
            return bounds.Count==0 ? null : bounds.ToArray();
        }

        /// <summary>
        /// Get line Bitmap and return array of int that
        /// indicates if the Bitmap row is part of a letter (1) or space (0)
        /// </summary>
        /// <param name="line">line Bitmap</param>
        /// <param name="threshold">threshold value</param>
        /// <returns>line with letters marked in black and space in white</returns>
        private static int[] CalcLetterOrSpace(Bitmap line, double threshold)
        {
            var newLine = new UnsafeBitmap(line);
            newLine.LockBitmap();
            var width = newLine.Width;
            var height = newLine.Height;
            var letterOrSpace = new int[width];
            for (var i = 0; i < width; i++)
            {
                letterOrSpace[i] = 0;
                for (var j = 0; j < height; j++)
                {
                    if (newLine.GetPixel(i, j).GetBrightness() > threshold) continue;
                    letterOrSpace[i] = 1;
                }
            }
            newLine.UnlockBitmap();
            return letterOrSpace;
        }

        /// <summary>
        /// Get line bounds and letter or space of a line, and return
        /// new bounds of line letters with spaces.
        /// </summary>
        /// <param name="bounds">bounds of line</param>
        /// <param name="letterOrSpace">letter or space of a line</param>
        /// <returns>new bounds of line letters with spaces</returns>
        private static int[][] CalcSpaces(int[][] bounds, IList<int> letterOrSpace)
        {
            //calc start and end of line
            var start = 0;
            var end = 0;
            for (var i = 0; i < letterOrSpace.Count; i++)
            {
                if (letterOrSpace[i] == 0) continue;
                start = i;
                break;
            }
            for (var i = letterOrSpace.Count - 1; i > 0; i--)
            {
                if (letterOrSpace[i] == 0) continue;
                end = i;
                break;
            }
            // calc average space length
            var totalSpaces = 0;
            for (var i = start; i < end; i++)
            {
                if (letterOrSpace[i] == 1) continue;
                totalSpaces++;
            }
            var averageSpace = ((double)totalSpaces / (bounds.Length - 1)) * MekademLetterSpace;

            //check if space greater than average space
            var spaces = new List<int[]>();
            for (var i = start; i < end; i++)
            {
                if (letterOrSpace[i] == 1) continue;
                var spaceLength = 0;
                while (letterOrSpace[i] == 0)
                {
                    spaceLength++;
                    i++;
                }
                //add space to spaces if it's greater than average space
                if (spaceLength <= averageSpace) continue;
                var spaceStart = i - spaceLength + 3;
                var spaceEnd = i - 1;
                if (spaceStart >= spaceEnd)
                {
                    spaceStart = i - 1;
                    spaceEnd = i - 1;
                }
                spaces.Insert(spaces.Count, new[] {spaceStart, spaceEnd});
            }

            //do nothing in case of no spaces
            if (spaces.Count == 0) return bounds;

            //build new bounds
            var newBounds = new int[bounds.Length + spaces.Count][];
            var currentBounds = 0;
            var currentSpaces = 0;
            var currentNewBounds = 0;
            while (bounds.Length > currentBounds && spaces.Count > currentSpaces)
            {
                if (bounds[currentBounds][0] < spaces[currentSpaces][0])
                {
                    newBounds[currentNewBounds] = bounds[currentBounds];
                    currentBounds++;
                }
                else
                {
                    newBounds[currentNewBounds] = spaces[currentSpaces];
                    currentSpaces++;
                }
                currentNewBounds++;
                if (spaces.Count > currentSpaces) continue;
                while (bounds.Length > currentBounds)
                {
                    newBounds[currentNewBounds] = bounds[currentBounds];
                    currentBounds++;
                    currentNewBounds++;
                }
            }
            return newBounds;
        }

        /// <summary>
        /// Get line and return array of letters bounds pairs
        /// </summary>
        /// <param name="line">line Bitmap</param>
        /// <param name="threshold">threshold value</param>
        /// <returns>array of letters bounds pairs</returns>
        private static int[][] GetLettersBounds(Bitmap line, double threshold)
        {
            var bounds = CalcLettersBounds(line, threshold);
            var letterOrSpace = CalcLetterOrSpace(line, threshold);
            return bounds == null ? null : CalcSpaces(bounds, letterOrSpace);
        }

        /// <summary>
        /// Get line and letter bounds and return fixed letter bounds.
        /// sometimes letters combined and this method separate it.
        /// </summary>
        /// <param name="line">Bitmap line</param>
        /// <param name="letterBounds">letter bounds</param>
        /// <returns>fixed letter bounds</returns>
        private static int[][] FixLetterBounds(Bitmap line, int[][] letterBounds)
        {
            var separates = new List<int>();
            var letterBoundsList = new List<int[]>(letterBounds);

            //indentify corrupted bounds
            for (var i = 0; i < letterBounds.Length; i++)
            {
                if (letterBounds[i][1] - letterBounds[i][0] > line.Height * MekademFixCoef)
                    separates.Add(i);
            }
            if (separates.Count == 0) return letterBounds;

            //fix and save into new bounds
            var shift = 0;
            foreach (var s in separates)
            {
                //prepare values
                var separate = s + shift;
                var realIndex = letterBoundsList.Count - separate;
                var currentLetter = GetLetter(line, letterBoundsList, realIndex);
                var numberOfCombined = CountLetters(currentLetter, ImageFilter.ThresholdLoose);
                if (numberOfCombined <= 1) continue;

                //calc in bounds of combined 
                var tempBounds = CalcLettersBounds(currentLetter, ImageFilter.ThresholdLoose);
                foreach (var bound in tempBounds)
                {
                    bound[0] += letterBoundsList[separate][0];
                    bound[1] += letterBoundsList[separate][0];
                }

                //update letter bounds list
                letterBoundsList.RemoveAt(separate);
                letterBoundsList.InsertRange(separate, tempBounds);
                shift += numberOfCombined - 1;
            }
            return letterBoundsList.ToArray();
        }

        #endregion
    }
}