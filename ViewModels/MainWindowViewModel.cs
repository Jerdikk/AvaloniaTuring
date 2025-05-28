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
            Ribbon ribbon = new Ribbon();
            //ribbon.ReadXML("ribbon.xml");
            ribbon.ReadXML("ribbon1.xml");

            TuringMachine turingMachine = new TuringMachine();
            //turingMachine.ReadXML("turing1.xml");
            turingMachine.ReadXML("turing2.xml");

            turingMachine.Solve(ribbon);

            ribbon.SaveXML("outribbon1.xml");

            Debug.WriteLine("test");
            
            //throw new NotImplementedException();
        }

     
    }
}
