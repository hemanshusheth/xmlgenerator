using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace XmlGenerator.PopUp
{
    public class TitleValidationRules :ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            if (String.IsNullOrEmpty(str))
            {
                return new ValidationResult(false, "Unique Name Cannot be blank");
            }

            if (MainWindow.CurrentStrategy.HasEntry(str))
            {
                return new ValidationResult(false, "This Unique Name already exist");
            }

            if (!Regex.IsMatch(str,"^[a-zA-Z0-9_]+$") && !str.StartsWith("_"))
            {
                return new ValidationResult(false, "Unique Name cannot contains Special Chars");
            }

            return new ValidationResult(true, null);
        }
    }

    public class LabelValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // var str = value as string;
            //if (String.IsNullOrEmpty(str))
            //{
            //    return new ValidationResult(false, "Please enter a Label Name");
            //}
            return new ValidationResult(true, null);
        }
    }
}