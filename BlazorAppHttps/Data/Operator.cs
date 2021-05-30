using System;
// using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BlazorAppHttps.Data
{
    public class Operator
    {
        // private int _nTraySamples = 0;

        // [Required]
        // [StringLength(10, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        // [Required]

        public DateTime ProtocolStart { get; init; }

        // [Required]

        public DateTime ProtocolEnd { get; set; }

        public List<DNATrayInput> DNATrayInputs { get; set; } = new();

        // [Required]

        public int NumberOfSamples { get; set; }

        public string NumberOfNormalSamples { get; set; }
        public string NumberOfRemainSamples { get; set; }
        public string NumberOfRedoSamples { get; set; }
        public string NumberOfRedoRemainSamples { get; set; }
        public int NumberOfNCSamples { get; } = 1;
        public int NumberOfDummySamples { get; set; } = 0;

        // private string TrayLocationTemplate(string name)
        // {
        //     return $"((?<{name}Row>[a-hA-H]{1})(?<{name}Col>[0-9]{1,2})|(?<{name}Col2>[0-9]{1,2})(?<{name}Row2>[a-hA-H]{1}))";
        // }

        public bool InputIsValid { get; set; } = false;

        public List<string> ErrorMessages { get; set; } = new List<string>();

        private void InvalidMessage(bool condition, string message) {
            if (condition) {
                InputIsValid = false;
                ErrorMessages.Add(message);
            }
        }

        private void ValidateInput()
        {

            ErrorMessages.Clear();

            InvalidMessage(String.IsNullOrEmpty(Name), "name is required");
            InvalidMessage(DNATrayInputs.Count == 0, "DNATrays are required");
            InvalidMessage(NumberOfSamples == 0, "number of samples should be greater than 0");
            // InvalidMessage(String.IsNullOrEmpty(Name), "name is required");

            // if(String.IsNullOrEmpty(Name)) {
            //     InputIsValid = false;
            //     ErrorMessages.Add("name is required");
            // }

            // if(DNATrayInputs.Count == 0) {
            //     InputIsValid = false;
            //     ErrorMessages.Add("DNATrays are required");
            // }

            // if(NumberOfSamples == 0) {
            //     InputIsValid = false;
            //     ErrorMessages.Add("DNATrays are required");
            // }

        }

        public void CalculateNumberOfSamples()
        {

            Regex regexCheck = new Regex(@"^\s*[a-hA-H0-9-~, ]+\s*$", RegexOptions.Compiled);

            // Regex regexSingle = new Regex(@"^\s*[a-hA-H]{1}[0-9]{1,2}\s*$", RegexOptions.Compiled);
            Regex regexSingle = new Regex(@"^\s*((?<row>[a-hA-H]{1})(?<col>[0-9]{1,2})|(?<col2>[0-9]{1,2})(?<row2>[a-hA-H]{1}))\s*$", RegexOptions.Compiled);

            // Regex regexRange = new Regex(@"^\s*([a-hA-H]{1}[0-9]{1,2})\s*[-]\s*([a-hA-H]{1}[0-9]{1,2})\s*$", RegexOptions.Compiled);
            Regex regexRange = new Regex(
                @"^\s*((?<StartRow>[a-hA-H]{1})(?<StartCol>[0-9]{1,2})|(?<StartCol2>[0-9]{1,2})(?<StartRow2>[a-hA-H]{1}))\s*[-]\s*((?<EndRow>[a-hA-H]{1})(?<EndCol>[0-9]{1,2})|(?<EndCol2>[0-9]{1,2})(?<EndRow2>[a-hA-H]{1}))\s*$",
                RegexOptions.Compiled);

            float ntraySamples = 0;

            DNATrayInputs.ForEach((input) =>
            {
                if (String.IsNullOrEmpty(input.Location) || !regexCheck.IsMatch(input.Location))
                {
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
                                return;
                            }

                            // if (!match.Groups[1].Success || !match.Groups[2].Success)
                            // {
                            //     return;
                            // }

                            // Console.WriteLine(match.Groups["StartRow"].Value);
                            // Console.WriteLine(match.Groups["StartCol"].Value);

                            // string start = match.Groups[1].Value;
                            // string end = match.Groups[2].Value;

                            // int startRow = (int)start.ToCharArray()[0];
                            // int endRow = (int)end.ToCharArray()[0];


                            int s = ((startRow - 97) * 12) + startCol;
                            int e = ((endRow - 97) * 12) + endCol;

                            if (e < s)
                            {
                                return;
                            }

                            ntraySamples += (e - s + 1);

                        }
                    }
                    else if (regexSingle.IsMatch(loct))
                    {
                        ntraySamples++;
                    }
                    else
                    {

                    }

                }

            });

            // NumberOfSamples = num;

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

            // failing:
            //     NumberOfSamples = 0;
            //     NumberOfNCSamples = 0;
            //     NumberOfDummySamples = 0;
            //     return;
        }

    }
}
