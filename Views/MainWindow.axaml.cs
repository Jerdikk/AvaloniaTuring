using Avalonia.Controls;
using AvaloniaTuring.ViewModels;
using ReactiveUI;
using System.Windows.Input;

namespace AvaloniaTuring.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel? mv;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            mv = this.DataContext as MainWindowViewModel;
            mv.OnKey(e.Key);
         
        }
    }
}