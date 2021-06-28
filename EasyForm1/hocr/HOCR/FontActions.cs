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
using System.Linq;

namespace HOCR
{
    /// <summary>
    /// Static class that provides actions to deal with font data.
    /// creation of dataset for font network and other accessories.
    /// </summary>
    public static class FontActions
    {
        public const int LetterSize = 20; //size of letter after resizing
        public const string LettersInNetwork = "אבגדהוזחטיכלמנסעפצקרשתךםןףץ0123456789.,;:?!-'\""; //letters
        public const string LettersOfInitial = "אבגדהוזחטיכלמנסעפצקרשתךםןףץ0123456789.,;:?!-'"; //letters
        private const string LettersPrint1 = "א ב ג ד ה ו ז ח ט י כ ל מ נ ס ע פ צ ק ר ש ת ך ם ן ף ץ";
        private const string LettersPrint2 = "' - ! ? : ; , . 9 8 7 6 5 4 3 2 1 0";
        public const string FontsFolderPath = @"Fonts\"; //path of font networks

        /// <summary>
        /// Get array of letters and return array of font data.
        /// [input\output][sample number][bitmap vector].
        /// </summary>
        /// <param name="letters">array of letters</param>
        /// <param name="func">function</param>
        /// <returns>array of font data</returns>
        public static double[][][] CreateFontData(Bitmap[][] letters, Func<IEnumerable<Bitmap[]>, int, double[][]> func)
        {
            var size = NumberOfElements(letters);

            //get letters parameters
            var lettersParameters = new double[size][];
            var count = 0;
            foreach (var letter in letters.SelectMany(line => line))
            {
                lettersParameters[count] = TextImageActions.GetLetterParameters(letter);
                count++;
            }

            //resizing letters
            var lettersResized = new Bitmap[letters.Length][];
            for (var i = 0; i < letters.Length; i++)
            {
                lettersResized[i] = new Bitmap[letters[i].Length];
                for (var j = 0; j < letters[i].Length; j++)
                    lettersResized[i][j] = TextImageActions.ResizeBitmap(letters[i][j], LetterSize, LetterSize);
            }

            //union letters parameters to input
            var input = func(lettersResized, size);
            input = Union2DArray(input, lettersParameters);

            //set output
            var output = new double[size][];
            for (var i = 0; i < size; i++)
            {
                output[i] = new double[LettersInNetwork.Length];
                for (var j = 0; j < LettersInNetwork.Length; j++)
                    output[i][j] = j == i % LettersOfInitial.Length ? 1 : 0;
            }

            //set font data and return it
            return new[] { input, output };
        }

        /// <summary>
        /// Get two 2D arrays and unify their inner level array.
        /// the array must be of the same size in the first level array,
        /// otherwise return null.
        /// </summary>
        /// <typeparam name="T">parameter type of arrays</typeparam>
        /// <param name="array1">first array</param>
        /// <param name="array2">second array</param>
        /// <returns>unify of 2 arrays</returns>
        private static T[][] Union2DArray<T>(T[][] array1, T[][] array2)
        {
            if (array1.Length != array2.Length) return null;
            var newArray = new T[array1.Length][];
            for (var i = 0; i < newArray.Length; i++)
            {
                var count = 0;
                newArray[i] = new T[array1[i].Length + array2[i].Length];
                foreach (var item in array1[i])
                {
                    newArray[i][count] = item;
                    count++;
                }
                foreach (var item in array2[i])
                {
                    newArray[i][count] = item;
                    count++;
                }
            }
            return newArray;
        }

        /// <summary>
        /// Get letter bitmap, letter number, and total number of letters in font
        /// and return letter data includes input and output of this letter
        /// </summary>
        /// <param name="letter">letter bitmap</param>
        /// <param name="letterNumber">letter number</param>
        /// <param name="numberOfLetters">total number of letters</param>
        /// <param name="func">function</param>
        /// <returns>letter data</returns>
        public static double[][] CreateLetterData(Bitmap letter, int letterNumber, int numberOfLetters,
            Func<IEnumerable<Bitmap[]>, int, double[][]> func)
        {
            var lettersParameters = new[] { TextImageActions.GetLetterParameters(letter) };
            var tempInput = Union2DArray(func(new[] { new[] { letter } }, 1), lettersParameters);
            var input = tempInput[0];
            var output = new double[numberOfLetters];

            for (var i = 0; i < numberOfLetters; i++)
                output[i] = i == letterNumber ? 1 : 0;
            return new[] { input, output };
        }

        /// <summary>
        /// Get array of lines and number of total letters,
        /// and return a deployed array of it.
        /// </summary>
        /// <param name="lines">array of lines</param>
        /// <param name="size">number of letters</param>
        /// <returns></returns>
        public static IEnumerable<Bitmap> DeployArray(IEnumerable<Bitmap[]> lines, int size)
        {
            var vector = new Bitmap[size];
            var i = 0;
            foreach (var letter in lines.SelectMany(line => line))
            {
                vector[i] = letter;
                i++;
            }
            return vector;
        }

        /// <summary>
        /// Get letter and function and return letter parameters using
        /// that function.
        /// </summary>
        /// <param name="letter">letter bitmap</param>
        /// <param name="func">function that get bitmaps and returns parameters</param>
        /// <returns>letter parameters</returns>
        public static double[][] LetterParameters(Bitmap letter, Func<IEnumerable<Bitmap[]>, int, double[][]> func)
        {
            var lettersParameters = new[] {TextImageActions.GetLetterParameters(letter)};
            return Union2DArray(func(new[] { new[] { letter } }, 1), lettersParameters);
        }

        /// <summary>
        /// Get bitmap array and size, and return vector array
        /// </summary>
        /// <param name="letters">bitmap array</param>
        /// <param name="size">size of bitmap array</param>
        /// <returns>vector array</returns>
        public static double[][] Pixels(IEnumerable<Bitmap[]> letters, int size)
        {
            var bitmaps = DeployArray(letters, size);
            var vectors = new double[size][];
            var number = 0;
            foreach (var bitmap in bitmaps)
            {
                var vector = new double[bitmap.Height * bitmap.Width];
                for (var i = 0; i < bitmap.Height; i++)
                {
                    for (var j = 0; j < bitmap.Width; j++)
                        vector[j + i*bitmap.Width] = TextImageActions.IsGreaterThanThreshold(bitmap.GetPixel(j, i));
                }
                vectors[number] = vector;
                number++;
            }
            return vectors;
        }

        /// <summary>
        /// Get array of letters and return number of elements in it
        /// </summary>
        /// <param name="letters">array of letters</param>
        /// <returns>number of elements</returns>
        public static int NumberOfElements(IEnumerable<Bitmap[]> letters)
        {
            return letters.SelectMany(lines => lines).Count();
        }

        /// <summary>
        /// Get letter bitmap and return bool value
        /// indicates if the letter is space or not.
        /// </summary>
        /// <param name="letter">letter bitmap</param>
        /// <returns>bool value indicates if the letter is space or not</returns>
        public static bool IsSpace(Bitmap letter)
        {
            for (var i = 0; i < letter.Height; i++)
                for (var j = 0; j < letter.Width; j++)
                {
                    var color = letter.GetPixel(i, j);
                    if (TextImageActions.IsGreaterThanThreshold(color) == 1)
                        return false;
                }
            return true;
        }

        /// <summary>
        /// Get font name and style, draw font image from it
        /// and save it on Fonts\ folder
        /// </summary>
        /// <param name="font">font</param>
        public static void MakeFont(FontFamily font)
        {
            var fontImage = TextImageActions.WhiteBitmap(1200, 500);
            var graphics = Graphics.FromImage(fontImage);

            var font24 = new Font(font, 24, FontStyle.Regular);
            graphics.DrawString(LettersPrint1 + Environment.NewLine + LettersPrint2, font24, Brushes.Black, 0, 0);
            var font28 = new Font(font, 28, FontStyle.Regular);
            graphics.DrawString(LettersPrint1 + Environment.NewLine + LettersPrint2, font28, Brushes.Black, 0, 125);
            var font24B = new Font(font, 24, FontStyle.Bold);
            graphics.DrawString(LettersPrint1 + Environment.NewLine + LettersPrint2, font24B, Brushes.Black, 0, 250);
            var font28B = new Font(font, 28, FontStyle.Bold);
            graphics.DrawString(LettersPrint1 + Environment.NewLine + LettersPrint2, font28B, Brushes.Black, 0, 375);

            fontImage.Save(FontsFolderPath + font.Name + ".png");
        }
    }
}
