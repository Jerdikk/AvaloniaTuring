using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace AvaloniaTuring.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand BuyMusicCommand { get; }
        public string Greeting { get; } = "Welcome to Avalonia!";

        public MainWindowViewModel()
        {
            BuyMusicCommand = ReactiveCommand.Create(OpenThePodBayDoors);



        }

        private void OpenThePodBayDoors()
        {
            Debug.WriteLine("test");
            //throw new NotImplementedException();
        }

     
    }
}
