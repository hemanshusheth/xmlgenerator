using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using FormProperty;
using MyUserControl.Controls;
using XmlGenerator.FormView;

namespace XmlGenerator.PopUp
{
    /// <summary>
    /// Interaction logic for FormPopUp.xaml
    /// </summary>
    public partial class FormPopUp
    {
        public static readonly DependencyProperty FormTitleProperty =DependencyProperty.Register("FormTitle", typeof(string),
                                                                        typeof(FormPopUp), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty FormLabelProperty = DependencyProperty.Register("FormLabel", typeof(string),
                                                                        typeof(FormPopUp), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty FormTitleUniqueProperty = DependencyProperty.Register("FormTitleUnique", typeof(string),
                                                                                typeof(FormPopUp), new UIPropertyMetadata(string.Empty));
      
        private FieldType _fieldType;
        private FieldType _selectorType;
        private IControl _element;

        public FormPopUp(IControl element)
        {
            InitializeComponent();
            Element = element;
            AddIControlToGrid();
            labelContent.Content = Element.GetContentLabel();
            This.Title = Element.GetTitle();
        }

        private void AddIControlToGrid()
        {
            UIElement uiElement= Element.GetUIElement();
            Grid.SetColumn(uiElement, 1);
            Grid.SetRow(uiElement, 3);
            detailsGrid.Children.Add(uiElement);
        }

        public string FormTitle
        {
            get
            {
                return (string) GetValue(FormTitleProperty);
            }
            set
            {
                SetValue(FormTitleProperty, value);
            }
        }

        public string FormLabel
        {
            get
            {
                return (string)GetValue(FormLabelProperty);
            }
            set
            {
                SetValue(FormLabelProperty, value);
            }
        }

        public string FormTitleUnique
        {
            get
            {
                return (string)GetValue(FormTitleUniqueProperty);
            }
            set
            {
                SetValue(FormTitleUniqueProperty, value);
            }
        }

        public FieldType SelectorType
        {
            get { return _selectorType; }
            set { _selectorType = value; }
        }

        public FieldType FieldType
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }

        public IControl Element
        {
            get { return _element; }
            set { _element = value; }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxUnique.Text))
            {
                ErrorPop errorPop = new ErrorPop("Please enter a Unique Name");
                errorPop.ShowDialog();
            }
            else
            {
                Property property = GetProperty();
                Group group = new Group(property);
                group.UniqueName = textBoxUnique.Text;

                MyGrid myGrid = new MyGrid();
                myGrid.CreateGrid(group);

                MainWindow.Mainpanel.Children.Add(myGrid.Grid);
                MainWindow.CurrentStrategy.CreateEntry(group);

                DialogResult = true;
            }
        }

        private Property GetProperty()
        {
            Property p = new Property();
            p.Title = textBoxTitle.Text;
            p.Count = comboBox1.SelectedIndex + 1;
            p.LabelNames = textBoxNames.Text.Split(',');
            p.Fieldtype = (FieldType) Enum.Parse(typeof(FieldType), Element.GetType().Name, true);
            p.SelectorType =SelectorType;
            p.Content = Element.GetContents();
            return p;
        }

        private void radioHasRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectorType=FieldType.RadioControl;
        }

        private void radioHasCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SelectorType = FieldType.CheckControl;
        }

        private void radioHasNoType_Checked(object sender, RoutedEventArgs e)
        {
            SelectorType = FieldType.NoType;
        }

        private void comboBox1_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox1.ItemsSource = new ArrayList{1,2,3,4,5,6,7,8,9,10};
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}