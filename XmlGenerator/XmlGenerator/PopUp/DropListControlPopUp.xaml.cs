using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using FormProperty;
using MyUserControl.Controls;
using Xceed.Wpf.Toolkit;
using XmlGenerator.FormView;

namespace XmlGenerator.PopUp
{
    /// <summary>
    /// Interaction logic for DropListControlPopUp.xaml
    /// </summary>
    public partial class DropListControlPopUp
    {
        public DropListControlPopUp(IControl element)
        {
           InitializeComponent();
            Element = element;
        }

        public static readonly DependencyProperty FormTitleProperty =DependencyProperty.Register("FormTitle", typeof(string),
                                                                        typeof(FormPopUp), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty FormLabelProperty = DependencyProperty.Register("FormLabel", typeof(string),
                                                                        typeof(FormPopUp), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty FormTitleUniqueProperty = DependencyProperty.Register("FormTitleUnique", typeof(string),
                                                                                typeof(FormPopUp), new UIPropertyMetadata(string.Empty));
      
        private FieldType _fieldType;
        private FieldType _selectorType;
        private IControl _element;

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

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = e.Source as ComboBox;
            int count = 0;
            if (comboBox != null)
            {
                 count = (int) comboBox.SelectedValue;
            }
            MyGrid myGrid = new MyGrid();
            labelGrid= myGrid.CreateDropListGrid(count,labelGrid);
        }

        private void comboBoxItemCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = e.Source as ComboBox;
            int count = 0;
            if (comboBox != null)
            {
                count = (int)comboBox.SelectedValue;
            }
            MyGrid myGrid = new MyGrid();
            itemsGrid = myGrid.CreateDropListItemsGrid(count, itemsGrid);
        }

        private void radioHasRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectorType = FieldType.RadioControl;
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
            comboBox1.ItemsSource = new ArrayList {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        }

        private void comboBoxItemCount_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxItemCount.ItemsSource = new ArrayList { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
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

        private Property GetProperty()
        {
            Property p = new Property();
            p.Title = textBoxTitle.Text;
            p.Count = comboBox1.SelectedIndex + 1;
            p.Fieldtype = (FieldType) Enum.Parse(typeof(FieldType), Element.GetType().Name, true);
            p.SelectorType = SelectorType;

            p.LabelNames = new string[p.Count];
            p.UniqueName = new string[p.Count];
            
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;

            foreach (var uielement in labelGrid.Children)
            {
                if (uielement.GetType() == typeof(TextBox))
                {
                    TextBox textBox = uielement as TextBox;
                    if (textBox != null)
                    {
                        switch (Grid.GetColumn(textBox))
                        {
                            case 0:
                                p.LabelNames[i++] = string.IsNullOrEmpty(textBox.Text) 
                                                             ? string.Empty:textBox.Text;
                                break;
                            case 1:
                                p.UniqueName[j++] = string.IsNullOrEmpty(textBox.Text) 
                                                             ? string.Empty:textBox.Text;
                                break;
                        }
                    }
                }
                else if(uielement.GetType()==typeof(DecimalUpDown))
                {
                    DecimalUpDown decimalUpDown = uielement as DecimalUpDown;
                    if (decimalUpDown != null)
                    {
                        switch (Grid.GetColumn(decimalUpDown))
                        {
                            case 2:
                                p.PrefferedWidth[k++] = decimalUpDown.Value != null
                                                            ? (decimal) decimalUpDown.Value
                                                            : 0;
                                break;
                            case 3:
                                p.MaxLen[l++] = decimalUpDown.Value != null 
                                                            ? (decimal) decimalUpDown.Value 
                                                            : 0;
                                break;
                        }
                    }
                }
            }
            return p;
        }

        
    }
}
