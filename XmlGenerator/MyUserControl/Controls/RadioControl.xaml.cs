using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FormProperty;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for RadioControl.xaml
    /// </summary>
    public partial class RadioControl : IControl
    {
        private TextBox textBox;
        #region Constructors
        public RadioControl()
        {
            InitializeComponent();
            textBox = new TextBox();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="b">A mybutton object</param>
        /// <param name="content">The content of the radio button</param>
        public RadioControl(RadioControl b,string content)
        {
            InitializeComponent();
            myradiobutton.Height = b.myradiobutton.Height;
            myradiobutton.Width = b.myradiobutton.Width;
            myradiobutton.Content = content;
        }
        #endregion

        #region MouseDragEvents
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(IsInMainPanel(e))
            {
                return;
            }

            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Package the data.
                DataObject data = new DataObject();
                data.SetData("Object", this);

                // Inititate the drag-and-drop operation.
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }

        private bool IsInMainPanel(MouseEventArgs e)
        {
            RadioControl radioControl = e.Source as RadioControl;
            if (radioControl != null)
            {
                Grid grid = radioControl.Parent as Grid;
                if (grid != null)
                {
                    StackPanel stackPanel = grid.Parent as StackPanel;
                    if (stackPanel != null && (stackPanel.Name.Equals("panel_main")))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
        #endregion

        public IControl GenerateControl(string labelName)
        {
            RadioControl myRadioButton = new RadioControl(new RadioControl(), labelName);
            return myRadioButton;
        }

        public UIElement GetUIElement()
        {
            textBox.Height = 25;
            textBox.Width = 120;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.IsEnabled = false;
            textBox.Margin = new Thickness(5);
            return textBox;
        }

        public string GetContentLabel()
        {
            return "Contents";
        }

        public string GetContents()
        {
            return textBox.Text;
        }

        public void SetContents(string value)
        {
            textBox.Text = value;
        }

        public string GetTitle()
        {
            return "Radio Control";
        }
    }
}