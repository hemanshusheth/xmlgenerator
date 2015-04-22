using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for ButtonControl.xaml
    /// </summary>
    public partial class ButtonControl : IControl
    {
        #region Constructor
        public ButtonControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="b">A mybutton object</param>
        /// <param name="content"> </param>
        public ButtonControl(ButtonControl b,string content)
        {
            InitializeComponent();
            mybutton.Height = b.mybutton.Height;
            mybutton.Width = b.mybutton.Width;
            mybutton.Content = content;
        }
        #endregion

        #region MouseDragEvents
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((((e.Source as ButtonControl).Parent as Grid).Parent as StackPanel).Name.Equals("panel_main"))
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

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
        #endregion

        public IControl GenerateControl(string labelName)
        {
            ButtonControl myButton = new ButtonControl(new ButtonControl(), labelName);
            return myButton;
        }

        public UIElement GetUIElement()
        {
            throw new System.NotImplementedException();
        }

        public string GetContentLabel()
        {
            return "";
        }

        public string GetContents()
        {
            return null;
        }

        public void SetContents(string value)
        {
            
        }

        public string GetTitle()
        {
            return "Button Control";
        }
    }
}
