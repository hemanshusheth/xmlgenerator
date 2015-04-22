
using System.Windows;
using FormProperty;

namespace MyUserControl.Controls
{
    public interface IControl
    {
        IControl GenerateControl(string labelName);
        UIElement GetUIElement();

        string GetContentLabel();

        string GetContents();

        void SetContents(string value);

        string GetTitle();
    }
}
