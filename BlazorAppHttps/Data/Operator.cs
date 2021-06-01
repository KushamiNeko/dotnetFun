using System;
// using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BlazorAppHttps.Data
{
    public class Operator
    {
        public string TimeFormat
        {
            get
            {
                return "yyyy/MM/dd HH:mm:ss";
            }
        }

        public string Name { get; set; } = "";

        public DateTime ProtocolStart { get; init; }

        public DateTime ProtocolEnd { get; set; }

        public List<DNATrayInput> DNATrayInputs { get; set; } = new();

        public int NumberOfSamples { get; set; } = 0;

        public string NumberOfNormalSamples { get; set; } = "";
        public string NumberOfRemainSamples { get; set; } = "";
        public string NumberOfRedoSamples { get; set; } = "";
        public string NumberOfRedoRemainSamples { get; set; } = "";
        public int NumberOfNCSamples { get; } = 1;
        public int NumberOfDummySamples { get; set; } = 0;

        public bool InputIsValid { get; set; } = false;

        public List<string> ErrorMessages { get; set; } = new List<string>();

        private void IfInvalidMessage(bool condition, string message)
        {
            if (condition)
            {
                InvalidMessage(message);
            }
        }

        private void InvalidMessage(string message)
        {
            InputIsValid = false;
            ErrorMessages.Add(message);
        }

        public void ValidateInputs()
        {

            InputIsValid = true;

            ErrorMessages.Clear();

            IfInvalidMessage(String.IsNullOrEmpty(Name?.Trim()), "担当者を入力してください");

            if (DNATrayInputs.Count == 0)
            {
                InvalidMessage("DNAトレイを入力してください");
            }
            else
            {
                CalculateNumberOfSamples();
            }

            uint n = 0;

            uint samples = 0;
            if (!String.IsNullOrEmpty(NumberOfNormalSamples))
            {
                if (UInt32.TryParse(NumberOfNormalSamples, out samples))
                {
                    n += samples;
                }
                else
                {
                    InvalidMessage("無効なサンプル数です");
                }
            }

            if (!String.IsNullOrEmpty(NumberOfRemainSamples))
            {
                if (UInt32.TryParse(NumberOfRemainSamples, out samples))
                {
                    n += samples;
                }
                else
                {
                    InvalidMessage("無効なサンプル数です");
                }
            }

            if (!String.IsNullOrEmpty(NumberOfRedoSamples))
            {
                if (UInt32.TryParse(NumberOfRedoSamples, out samples))
                {
                    n += samples;
                }
                else
                {
                    InvalidMessage("無効なサンプル数です");
                }
            }

            if (!String.IsNullOrEmpty(NumberOfRedoRemainSamples))
            {
                if (UInt32.TryParse(NumberOfRedoRemainSamples, out samples))
                {
                    n += samples;
                }
                else
                {
                    InvalidMessage("無効なサンプル数です");
                }
            }

            IfInvalidMessage(n != NumberOfSamples - NumberOfNCSamples - NumberOfDummySamples, "サンプル数が一致しません");

        }

        private void CalculateNumberOfSamples()
        {

            Regex regexTrayID = new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled);

            Regex regexCheck = new Regex(@"^\s*[a-hA-H0-9-~, ]+\s*$", RegexOptions.Compiled);

            // Regex regexSingle = new Regex(@"^\s*[a-hA-H]{1}[0-9]{1,2}\s*$", RegexOptions.Compiled);
            Regex regexSingle = new Regex(@"^\s*((?<row>[a-hA-H]{1})(?<col>[0-9]{1,2})|(?<col2>[0-9]{1,2})(?<row2>[a-hA-H]{1}))\s*$", RegexOptions.Compiled);

            // Regex regexRange = new Regex(@"^\s*([a-hA-H]{1}[0-9]{1,2})\s*[-]\s*([a-hA-H]{1}[0-9]{1,2})\s*$", RegexOptions.Compiled);
            Regex regexRange = new Regex(
                @"^\s*((?<StartRow>[a-hA-H]{1})(?<StartCol>[0-9]{1,2})|(?<StartCol2>[0-9]{1,2})(?<StartRow2>[a-hA-H]{1}))\s*[-]\s*((?<EndRow>[a-hA-H]{1})(?<EndCol>[0-9]{1,2})|(?<EndCol2>[0-9]{1,2})(?<EndRow2>[a-hA-H]{1}))\s*$",
                RegexOptions.Compiled);

            float ntraySamples = 0;

            foreach (DNATrayInput input in DNATrayInputs)
            {
                if (String.IsNullOrEmpty(input.TrayID?.Trim()))
                {
                    InvalidMessage("DNAトレイIDを入力してください");
                }
                else
                {

                    IfInvalidMessage(!regexTrayID.IsMatch(input.TrayID), "無効なDNAトレイIDです");
                }

                if (String.IsNullOrEmpty(input.Location?.Trim()) || !regexCheck.IsMatch(input.Location))
                {
                    InvalidMessage("位置情報を入力してください");
                    return;
                }

                string[] locs = input.Location.Split(",");

                foreach (string loc in locs)
                {
                    string loct = loc.Trim().ToLower();

                    if (regexRange.IsMatch(loct))
                    {

                        MatchCollection matches = regexRange.Matches(loct);

                        foreach (Match match in matches)
                        {

                            int startRow, endRow;
                            int startCol, endCol;

                            if (match.Groups["StartRow"].Success && match.Groups["StartCol"].Success)
                            {
                                startRow = (int)match.Groups["StartRow"].Value.ToCharArray()[0];
                                startCol = Int32.Parse(match.Groups["StartCol"].Value);
                            }
                            else if (match.Groups["StartRow2"].Success && match.Groups["StartCol2"].Success)
                            {
                                startRow = (int)match.Groups["StartRow2"].Value.ToCharArray()[0];
                                startCol = Int32.Parse(match.Groups["StartCol2"].Value);
                            }
                            else
                            {
                                InvalidMessage("無効な位置情報です");
                                return;
                            }

                            if (match.Groups["EndRow"].Success && match.Groups["EndCol"].Success)
                            {
                                endRow = (int)match.Groups["EndRow"].Value.ToCharArray()[0];
                                endCol = Int32.Parse(match.Groups["EndCol"].Value);
                            }
                            else if (match.Groups["EndRow2"].Success && match.Groups["EndCol2"].Success)
                            {
                                endRow = (int)match.Groups["EndRow2"].Value.ToCharArray()[0];
                                endCol = Int32.Parse(match.Groups["EndCol2"].Value);
                            }
                            else
                            {
                                InvalidMessage("無効な位置情報です");
                                return;
                            }

                            if (startCol <= 0 || endCol <= 0 || startCol > 12 || endCol > 12)
                            {
                                InvalidMessage("無効な位置情報です");
                                return;
                            }

                            int s = ((startRow - 97) * 12) + startCol;
                            int e = ((endRow - 97) * 12) + endCol;

                            if (e < s)
                            {
                                InvalidMessage("無効な位置情報です");
                                return;
                            }

                            ntraySamples += (e - s + 1);

                        }
                    }
                    else if (regexSingle.IsMatch(loct))
                    {
                        MatchCollection matches = regexSingle.Matches(loct);

                        foreach (Match match in matches)
                        {
                            int col;

                            if (match.Groups["row"].Success && match.Groups["col"].Success)
                            {
                                col = Int32.Parse(match.Groups["col"].Value);
                            }
                            else if (match.Groups["row2"].Success && match.Groups["col2"].Success)
                            {
                                col = Int32.Parse(match.Groups["col2"].Value);
                            }
                            else
                            {
                                InvalidMessage("無効な位置情報です");
                                return;
                            }

                            if (col <= 0 || col > 12)
                            {
                                InvalidMessage("無効な位置情報です");
                                return;
                            }
                        }

                        ntraySamples++;
                    }
                    else
                    {
                        InvalidMessage("無効な位置情報です");
                    }

                }
            }

            if (ntraySamples <= 0)
            {
                NumberOfSamples = 0;
                // NumberOfNCSamples = 0;
                NumberOfDummySamples = 0;
                return;
            }
            else
            {
                if ((ntraySamples + 1) % 8 == 0)
                {
                    NumberOfSamples = (int)(ntraySamples + 1);
                    NumberOfDummySamples = 0;
                }
                else
                {
                    NumberOfSamples = (int)((ntraySamples + 1) / 8) * 8 + 8;
                    NumberOfDummySamples = NumberOfSamples - ((int)ntraySamples + 1);
                }

                return;
            }

        }

    }
}
