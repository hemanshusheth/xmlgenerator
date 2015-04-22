using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxControl.xaml
    /// </summary>
    public partial class TextBoxControl : IControl
    {
        private DecimalUpDown numericUpDown;
        private string title;

        #region Constructors
        public TextBoxControl()
        {
            InitializeComponent();
            numericUpDown = new DecimalUpDown();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="b">A mybutton object</param>
        /// <param name="content">The content of the radio button</param>
        public TextBoxControl(TextBoxControl b, string content)
        {
            InitializeComponent();
            mytextbox.Height = b.mytextbox.Height;
            mytextbox.Width = b.mytextbox.Width;
            //mytextbox.MaxLength= Convert.ToInt32(content);
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        #endregion

        #region MouseDragEvents
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsInMainPanel(e))
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
            TextBoxControl textBoxControl = e.Source as TextBoxControl;
            if (textBoxControl != null)
            {
                Grid grid = textBoxControl.Parent as Grid;
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
            TextBoxControl myTextBox = new TextBoxControl(new TextBoxControl(), labelName);
            return myTextBox;
        }

        public UIElement GetUIElement()
        {
            numericUpDown.Height = 25;
            numericUpDown.Width = 120;
            numericUpDown.HorizontalAlignment = HorizontalAlignment.Left;
            numericUpDown.Increment = 1;
            numericUpDown.Value = 100;
            numericUpDown.Margin = new Thickness(5);
            return numericUpDown;
        }

        public string GetContentLabel()
        {
            return "MaxLength";
        }

        public string GetContents()
        {
            return numericUpDown.Value.ToString();
        }

        public void SetContents(string value)
        {
            numericUpDown.Value = Convert.ToDecimal(value);
        }

        public string GetTitle()
        {
            return "TextBox Control";
        }
    }
}