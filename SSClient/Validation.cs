using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ScanningServicesAdmin
{
    class Validation
    {
        /// <summary>
        /// Determine if Date String is an actual date
        /// Date format = MM/DD/YYYY
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        static public bool ValidateDate(string date)
        {
            try
            {
                // for US, alter to suit if splitting on hyphen, comma, etc.
                string[] dateParts = date.Split('/');

                // create new date from the parts; if this does not fail
                // the method will return true and the date is valid
                DateTime testDate = new
                    DateTime(Convert.ToInt32(dateParts[2]),
                    Convert.ToInt32(dateParts[0]),
                    Convert.ToInt32(dateParts[1]));

                return true;
            }
            catch
            {
                // if a test date cannot be created, the
                // method will return false
                return false;
            }
        }

        static public void validateTextIntegerInTextBox(object sender, EventArgs e)
        {
            Exception X = new Exception();
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
            try
            {
                if (T.Text != "-")
                {
                    int x = int.Parse(T.Text);
                }
            }
            catch (Exception)
            {
                try
                {
                    Console.Beep();
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception) { }
            }
        }


        static public void validateTextDoubleInTextBox(object sender, EventArgs e)
        {
            Exception X = new Exception();
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
            try
            {
                if (T.Text != "-")
                {
                    double x = double.Parse(T.Text);

                    if (T.Text.Contains(','))
                        throw X;
                }
            }
            catch (Exception)
            {
                try
                {
                    Console.Beep();
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception) { }
            }
        }

        static public void validateTextCharacterInTextBox(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
            try
            {
                //Not Allowing Numbers
                char[] UnallowedCharacters = { '0', '1',
                                           '2', '3',
                                           '4', '5',
                                           '6', '7',
                                           '8', '9'};

                if (textContainsUnallowedCharacter(T.Text, UnallowedCharacters))
                {
                    Console.Beep();
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
            }
            catch (Exception) { }
        }

        static private bool textContainsUnallowedCharacter(string T, char[] UnallowedCharacters)
        {
            for (int i = 0; i < UnallowedCharacters.Length; i++)
                if (T.Contains(UnallowedCharacters[i]))
                    return true;

            return false;
        }

        public static bool validateTextAlphaNum(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
            if (string.IsNullOrEmpty(T.Text))
                return false;

            for (int i = 0; i < T.Text.Length; i++)
            {
                if (!(char.IsLetter(T.Text[i])) && (!(char.IsNumber(T.Text[i]))))
                {
                    Console.Beep();
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                    return false;
                }
            }
            return true;
        }

        public static bool validateTextAlphaNumButUnderscore(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
            if (string.IsNullOrEmpty(T.Text))
                return false;

            for (int i = 0; i < T.Text.Length; i++)
            {
                if (T.Text[i] != System.Convert.ToChar("_"))
                {
                    if (!(char.IsLetter(T.Text[i])) && (!(char.IsNumber(T.Text[i]))))
                    {
                        Console.Beep();
                        int CursorIndex = T.SelectionStart - 1;
                        T.Text = T.Text.Remove(CursorIndex, 1);

                        //Align Cursor to same index
                        T.SelectionStart = CursorIndex;
                        T.SelectionLength = 0;
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool validateTextAlphaNumButUnderscoreAndhyphens(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
            if (string.IsNullOrEmpty(T.Text))
                return false;

            for (int i = 0; i < T.Text.Length; i++)
            {
                if (T.Text[i] != System.Convert.ToChar("_"))
                {
                    if (T.Text[i] != System.Convert.ToChar("-"))
                    {
                        if (!(char.IsLetter(T.Text[i])) && (!(char.IsNumber(T.Text[i]))))
                        {
                            Console.Beep();
                            int CursorIndex = T.SelectionStart - 1;
                            T.Text = T.Text.Remove(CursorIndex, 1);

                            //Align Cursor to same index
                            T.SelectionStart = CursorIndex;
                            T.SelectionLength = 0;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        static public void ErrorMessage(Exception e)
        {
            string msg1 = "";
            string msg2 = "";
            if (e.Message != null)
                msg1 = e.Message;
            if (e.InnerException != null)
                msg2 = "StackTrace: " + e.InnerException.Message;

            //MessageBox.Show("Exception caught:  " + e.Message + "\r\n " + e.InnerException.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            MessageBox.Show("Exception caught:  " + msg1 + "\r\n " + msg2 + "\r\r" + e.StackTrace, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }

        static public void ErrorMessage(string message)
        {
            MessageBox.Show("Error:  " + message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }

        /// <summary>/// Gets whether the specified path is a valid absolute file path.
        /// </summary>
        /// <param name="path">Any path. OK if null or empty.</param>
        static public bool IsValidPath(string path)
        {
            Regex r = new Regex(@"^(?:[\w]\:|\\)(\\[a-zA-Z_\-\ –\s0-9\.\\]+)$");
            //Regex r = new Regex(@"^[a - zA - Z]:\\(((? ![<>:"/\\|?*]).)+((?<![ .])\\)?)*$");
            //Regex r = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.(?i)(txt|gif|pdf|doc|docx|xls|xlsx)$");
            return r.IsMatch(path.ToLower());
        }

        static public bool IsValidURL(string path)
        {
            Regex r = new Regex(@"^(http|ftp|https|www)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$", RegexOptions.IgnoreCase);
            //return Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute);
            //Regex r = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.\\]+)$");
            return r.IsMatch(path.ToLower());
        }

        static public bool IsValidPathExtended(string path)
        {
            Regex r = new Regex(@"^((([a-zA-Z]\:)|(\\))(((\\{1})[^\\\ ]([^/:*?<>""|\\]*))+)[\\])$");
            //Regex r = new Regex(@"^((([a-zA-Z]\:)|(\\))((\\{1})[^\\\ ]([^/:*?<>""|\\]*)))$");

            return r.IsMatch(path.ToLower());
        }

        static public bool IsValidInteger(string value)
        {
            Regex r = new Regex("^[0-9]+$");
            return r.IsMatch(value.ToLower());
        }

        static public bool IsValidBoolean(string value)
        {
            value = value.ToLower();
            Regex r = new Regex("true|false");
            return r.IsMatch(value.ToLower());
        }

        static public bool IsValidTime(string value)
        {
            value = value.ToLower();
            Regex r = new Regex("^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            return r.IsMatch(value.ToLower());
        }

        
        public static bool validHostName(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
            if (string.IsNullOrEmpty(T.Text))
                return false;

            for (int i = 0; i < T.Text.Length; i++)
            {
                if (!(char.IsLetter(T.Text[i]) || char.IsNumber(T.Text[i]) || T.Text[i] == System.Convert.ToChar("-")))
                {
                    Console.Beep();
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                    return false;
                }
            }
            return true;
        }

        static public void validateValidCharInFileDirNames(object sender, EventArgs e)
        {
            Exception X = new Exception();
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;

            var values2 = new[] { "*", "|", "\\", ":", "\"", "/", "'", "<", ">", "?", "%", "[","]","{","}" };
            var values = new[] { "*", "|", "\\", ":", "\"", "/", "'", "<", ">", "?"};

            try
            {
                // "\\", "\"" };
                if (values.Any(T.Text.Contains))
                {
                    Console.Beep();
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }               
            }
            catch (Exception)
            {

            }
        }

        static public void validateValidCharInFileDirNamesExtended(object sender, EventArgs e)
        {
            Exception X = new Exception();
            System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;

            var values = new[] { "*", "|", "\\", ":", "\"", "/", "'", "<", ">", "?", "%", "[", "]", "{", "}" };
           
            try
            {
                // "\\", "\"" };
                if (values.Any(T.Text.Contains))
                {
                    Console.Beep();
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
