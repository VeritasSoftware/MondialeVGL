using CsvHelper;
using MondialeVGL.OrderProcessor.Repository.Entities;
using System;
using System.Text.RegularExpressions;

namespace MondialeVGL.OrderProcessor.Repository
{
    public static class Extensions
    {
        public static RecordType GetRecordType(this CsvReader csvReader)
        {
            var text =  csvReader.GetField<string>(0);

            if (string.IsNullOrEmpty(text) ||
                            !Regex.Match(text, @"^\s*(H|D)\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled).Success)
            {
                throw new ApplicationException($"{text} is an invalid record type. Valid values [H|D].");
            }

            return Enum.Parse<RecordType>(text.Trim().ToUpper());
        }
    }
}
