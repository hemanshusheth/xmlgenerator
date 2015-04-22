using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace XmlGenerator.PopUp
{
    public class ErrorsToMessageConverter : IValueConverter
    {
        /// <summary>
        /// single Value Converter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter,CultureInfo culture)
        {
            var sb = new StringBuilder();
            var errors = value as ReadOnlyCollection<ValidationError>;
            if (errors != null)
            {
                foreach (var e in errors.Where(e => e.ErrorContent != null))
                {
                    sb.AppendLine(e.ErrorContent.ToString());
                }
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter,CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(ValidationError), typeof(bool))]
    public class TextBoxHasErrorToButtonIsEnabledConverter : IMultiValueConverter
    {        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var isValid in values)
            {
                if (isValid as bool? == true)
                    return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}