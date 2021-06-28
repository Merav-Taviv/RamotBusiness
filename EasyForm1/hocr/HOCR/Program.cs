///*
//* This file is Part of a HOCR
//* LGPLv3 Licence:
//*       Copyright (c) 2011 
//*          Niv Maman [nivmaman@yahoo.com]
//*          Maxim Drabkin [mdrabkin@gmail.com]
//*          Hananel Hazan [hhazan01@CS.haifa.ac.il]
//*          University of Haifa
//* This Project is part of our B.Sc. Project course that under supervision
//* of Hananel Hazan [hhazan01@CS.haifa.ac.il]
//* All rights reserved.
//*
//* Redistribution and use in source and binary forms, with or without 
//* modification, are permitted provided that the following conditions are met:
//*
//* 1. Redistributions of source code must retain the above copyright notice, this list of conditions 
//*    and the following disclaimer.
//* 2. Redistributions in binary form must reproduce the above copyright notice, this list of 
//*    conditions and the following disclaimer in the documentation and/or other materials provided
//*    with the distribution.
//* 3. Neither the name of the <ORGANIZATION> nor the names of its contributors may be used to endorse
//*    or promote products derived from this software without specific prior written permission.
//*
//* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
//* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
//* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
//* ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
//* ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//* SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
//* CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
//* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
//* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
//* DAMAGE.
//*/

//using System;
//using System.IO;
//using System.Windows.Forms;
//using System.Drawing;


//namespace HOCR
//{
//    static class Program
//    {
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]


//        //static void Main(string[] args)
//        //{
//        //    if (args.Length > 0)
//        //    {
//        //        var fileName = "123.txt";
//        //        const string dir = @"Text";
//        //        Directory.CreateDirectory(dir);
//        //        var filePath = Path.Combine(dir, fileName);
//        //        Application.EnableVisualStyles();
//        //        Application.SetCompatibleTextRenderingDefault(false);
//        //        Class1 c1 = new Class1();
//        //        Bitmap img = new Bitmap($"{args[0]}");
//        //        string text = c1.HOCRClass1(img);
//        //        File.WriteAllText(filePath, text);
//        //        // Application.Run(new MainForm());
//        //    }

//        static void Main(string[] args)
//        {
//            var fileName = "123.txt";
//            const string dir = @"Text";
//            Directory.CreateDirectory(dir);
//            var filePath = Path.Combine(dir, fileName);
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Class1 c1 = new Class1();
//            Bitmap img = new Bitmap(@"C:\Users\This_User\Desktop\פרויקט\תמונות כתב יד\Untitled.png");
//            //  string text = "111";
//            MessageBox.Show("now");
//            string text = c1.HOCRClass1(img);
//            MessageBox.Show("444");
//            File.WriteAllText(filePath, text);
//            MessageBox.Show("555");
//        }
//    }
//}

