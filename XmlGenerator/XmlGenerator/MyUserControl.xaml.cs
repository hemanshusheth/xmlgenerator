using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MyUserControl.Controls;
using XmlGenerator.PopUp;

namespace XmlGenerator
{
    /// <summary>
    /// Interaction logic for MyUserControl.xaml
    /// </summary>
    public partial class MyUserControl
    {
        public MyUserControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Adds the corresponding control to the Mainpanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = VisualTreeHelper.GetParent((UIElement) e.Source) as Grid;
            if (grid == null) return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grid); i++)
            {
                Visual childVisual = (Visual) VisualTreeHelper.GetChild(grid, i);
                if (childVisual != null)
                {
                    if ((childVisual.GetType() != sender.GetType()) 
                        && (Grid.GetRow((UIElement) childVisual).Equals(Grid.GetRow((UIElement) sender)))
                        && (childVisual is IControl))
                    {
                        IControl r = (IControl) Activator.CreateInstance(childVisual.GetType());
                        if (MainWindow.CurrentStrategy != null)
                        {
                            if (MainWindow.StrategyCombobox.SelectedIndex == -1)
                            {
                                MainWindow.StrategyCombobox.SelectedItem = MainWindow.CurrentStrategy;
                            }
                            //TextBoxPopUp pop = new TextBoxPopUp(r);
                            //pop.ShowDialog();
                            //FormPopUp popup = new FormPopUp(r);
                            //popup.ShowDialog();
                            //TimeControlPopUp p = new TimeControlPopUp(r);
                            //p.ShowDialog();
                            DropListControlPopUp p = new DropListControlPopUp(r);
                            p.ShowDialog();
                        }
                        else
                        {
                            ErrorPop errorPop = new ErrorPop("Create a Strategy First");
                            errorPop.ShowDialog();
                        }
                        break;
                    }
                }
            }
        }
    }
}