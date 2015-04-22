using System.Windows;

namespace XmlGenerator.PopUp
{
    /// <summary>
    /// Interaction logic for ErrorPop.xaml
    /// </summary>
    public partial class ErrorPop : Window
    {
        private string _errorMessage;
        public ErrorPop(string message)
        {
            InitializeComponent();
            ErrorMessage = message;
            errorTextBlock.Text = ErrorMessage;
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}
