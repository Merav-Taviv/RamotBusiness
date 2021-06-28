using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HOCR
{
   public class Class1
    {
        //Image
        private Bitmap _bitmap; //bitmap of current image
        private Bitmap[][] _letters; //letters of current image
        private int[][][] _letterBounds; //letters bounds of current image


        //Neural Network
        private NeuralNetwork _fontNetwork; //pixels neural network of current font
        private string _currentFontPath; //path of current font pixels
        private readonly int[] _middleLayers = new[] { 50, 50 }; //size of middle layer
        private readonly int _outputLayer = FontActions.LettersInNetwork.Length; //size of output layer


        private string resultLetterText;
        private string resultText;

        public string ResultLetterText
        {
            get { return resultLetterText; }
            set { resultLetterText = value; }
        }

        public string ResultText
        {
            get { return resultText; }
            set { resultText = value; }
        }
 
        public Class1(Bitmap _bitmap)
        {                     //choose font file
            //_currentFontPath = @"Fonts\\Manuscript.net";
            _fontNetwork = FileActions.LoadNetwork(@"F:\Project\EasyForm1\hocr\HOCR\bin\Debug\Fonts\Manuscript.net");
            if (_fontNetwork == null)
                Console.WriteLine(@"Error reading font file");
            this._bitmap = _bitmap;

            //AutoDetectFontTask();
            GetTextTask();
            Console.WriteLine(ResultText);
        }
        public Class1()
        { }
        public string HOCRClass1(Bitmap _bitmap)
        {                     //choose font file
            //_currentFontPath = @"Fonts\\Manuscript.net";
            _fontNetwork = FileActions.LoadNetwork(@"Fonts\Manuscript.net");
            if (_fontNetwork == null)
                Console.WriteLine(@"Error reading font file");
            this._bitmap = _bitmap;

            //AutoDetectFontTask();
            GetTextTask();
            return ResultText;
        }
        public string HOCR(Bitmap _bitmap)
        {
            //Load network
            _fontNetwork = FileActions.LoadNetwork(@"Fonts\\Manuscript.net");
            this._bitmap = _bitmap;
            // Get the result text
            GetTextTask();
            return resultText;
        }


        /// <summary>
        /// Get letter bitmap, calculate which letter is it,
        /// and return it's index.
        /// </summary>
        /// <param name="letter">letter bitmap</param>
        /// <returns>index of letter</returns>
        private int CalculateLetter(Bitmap letter)
        {
            var sample = FontActions.LetterParameters(letter, FontActions.Pixels);
            var output = _fontNetwork.Compute(sample[0]);
            var max = 0.0;
            var letterId = 0;
            for (var i = 0; i < FontActions.LettersInNetwork.Length; i++)
            {
                var temp = output[i];
                if (temp <= max) continue;
                max = temp;
                letterId = i;
            }
            return letterId;
        }


        /// <summary>
        /// Get line and letter numbers and show that letter on screen
        /// at the repair section
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="letterNumber"></param>
        private void ShowResultLetterText(int letterNumber)
        {
            //show choosen letter on repai section
            var letter = _letters[0][letterNumber - 1];
            int letterId;
            try
            {
                if (FontActions.IsSpace(letter))
                {
                    ResultLetterText = @" ";
                    return;
                }
                letterId = CalculateLetter(letter);
            }
            catch (Exception)
            {
                return;
            }
            ResultLetterText = FontActions.LettersInNetwork[letterId].ToString();
        }


        /// <summary>
        /// Task that been called when click on Get Text button
        /// </summary>
        public void GetTextTask()
        {
            //calculate and separate for letters
            if (_letters == null)
            {
                //try
                //{
                    _letterBounds = TextImageActions.GetAllLettersBounds(_bitmap);
                    _letters = TextImageActions.RetrieveText(_bitmap, _letterBounds, FontActions.LetterSize);
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception("Retrieving text failed" + Environment.NewLine + Environment.NewLine +
                //                      "Seems like the text you have just tried to recognized is too small" + ex.Message);

                //}

                //if (_letters == null)
                //{
                //    throw new Exception("Retrieving text failed");

                //}
            }

            //calculate text
            var text = "";
            var size = FontActions.NumberOfElements(_letters);
            var letters = FontActions.DeployArray(_letters, size);
            foreach (var letter in letters)
            {
                //check case of space
                if (FontActions.IsSpace(letter))
                {
                    text += @" ";
                    continue;
                }
                var result = CalculateLetter(letter);
                text += FontActions.LettersInNetwork[result];
            }

            //add spaces between lines
            var currentLetter = 0;
            foreach (var line in _letters)
            {
                currentLetter += line.Length;
                text = text.Insert(currentLetter, "\n");
                currentLetter++;
            }
            text = text.Remove(text.Length - 1);
            ResultText = text;
        }


        /// <summary>
        /// read image, recognize which font using in it and load that font.
        /// (it reads only the first line to determine)
        /// </summary>
        public void AutoDetectFontTask()
        {

            //calculate and separate for letters
            try
            {
                _letterBounds = TextImageActions.GetAllLettersBounds(_bitmap);
                _letters = TextImageActions.RetrieveText(_bitmap, _letterBounds, FontActions.LetterSize);
            }
            catch (Exception ex)
            {
                throw new Exception("");

            }
            if (_letters == null)
            {
                throw new Exception("No ");

            }

            //init variables
            var maxFontRank = 0.0;
            var Font = "";

            //load font of current item

            var currentFontPath = FontActions.FontsFolderPath + @"\" + "david.net";
            //var currentFontPath = @"D:\Users\User\Desktop\newProject\newProject\bin\Debug\Fonts\kredit.net";
            var fontNetwork = FileActions.LoadNetwork(currentFontPath);
            //check font and rank it
            var result = _letters.SelectMany(letter => letter).Sum(letter =>
                fontNetwork.Compute(FontActions.LetterParameters(letter, FontActions.Pixels)[0]).Max());
            // if (result < maxFontRank) continue;
            maxFontRank = result;
            Font = "kredit";


            //choose font with best rank
            _currentFontPath = FontActions.FontsFolderPath + @"\" + Font + ".net";
            _fontNetwork = FileActions.LoadNetwork(_currentFontPath);

        }
    }
}
