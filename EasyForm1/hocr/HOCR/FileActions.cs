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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace HOCR
{
    /// <summary>
    /// Static class that provides methods to deal with files.
    /// Loading and saving network into files.
    /// </summary>
    public static class FileActions
    {
        public const string ResultsFolderPath = @"Results\";

        /// <summary>
        /// Get path of network file and return the network
        /// </summary>
        /// <param name="path">path of network file</param>
        /// <returns>network</returns>
        public static NeuralNetwork LoadNetwork(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    var network = bf.Deserialize(stream) as NeuralNetwork;
                    return network;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get path of file to store network and network.
        /// stores the network in that file.
        /// </summary>
        /// <param name="path">path of file</param>
        /// <param name="network">network</param>
        public static string SaveNetwork(string path, NeuralNetwork network)
        {
            try
            {
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(stream, network);
                    return null;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Get text and path and write the text into that path
        /// </summary>
        /// <param name="text">text to write</param>
        /// <param name="path">path to write to</param>
        /// <returns>exception message or null if ok</returns>
        public static string SaveTextIntoFile(string text, string path)
        {
            try
            {
                var newText = "";
                foreach (var letter in text)
                {
                    if (letter=='\n')
                    {
                        newText += Environment.NewLine;
                        continue;
                    }
                    newText += letter;
                }
                File.WriteAllText(path, newText);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        /// <summary>
        /// Get path of pdf file, create tif image from it,
        /// and return the path of the new tif file.
        /// 
        /// Assumption:
        /// GhostScript\gswin32c.exe is exists
        /// </summary>
        /// <param name="path">path of pdf file</param>
        /// <returns>path of new tif file</returns>
        public static string ConvertPdfToTiff(string path)
        {
            var newPath = Path.ChangeExtension(path, "tif");
            var convert = new Process();
            try
            {
                convert.StartInfo.UseShellExecute = false;
                convert.StartInfo.FileName = @"GhostScript\gswin32c.exe";
                convert.StartInfo.Arguments = String.Format("-q -dNOPAUSE -sDEVICE=tiffg4 -sOutputFile=\"{0}\" \"{1}\" -c quit", newPath, path);
                convert.StartInfo.CreateNoWindow = true;
                convert.Start();
                convert.WaitForExit();
                return newPath;
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Format("Exception while converting to pdf: {0}", e.Message));
                return null;
            }
        }

        /// <summary>
        /// Get font name and return value indicates if the name is
        /// available (true) or already in use (false).
        /// </summary>
        /// <param name="fontName">font name</param>
        /// <returns>boolean indicates if it available</returns>
        public static bool IsFontNameAvailable(string fontName)
        {
            var directoryInfo = new DirectoryInfo(FontActions.FontsFolderPath);
            var files = directoryInfo.GetFiles();
            if (files.Select(file => file.Name.Remove(file.Name.Length - 4)).Any(temp => temp == fontName))
            {
                MessageBox.Show(@"Font name is already in use");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get array of objects array and return 
        /// the size of the longest array in it.
        /// </summary>
        /// <param name="items">array of arrays</param>
        /// <returns>size of longest array</returns>
        public static int LongestArray(IEnumerable<object[]> items)
        {
            var max = 0;
            foreach (var item in items)
            {
                var number = ((ICollection)item).Count;
                if (number > max) max = number;
            }
            return max;
        }
    }
}