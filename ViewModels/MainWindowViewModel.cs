using Avalonia.Input;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace AvaloniaTuring.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public int IndexInRibbon;
        public ICommand SolveCommand { get; }

        private string? _t01;
        public string? t01
        {
            get => _t01;
            set => this.RaiseAndSetIfChanged(ref _t01, value);

        }
        private string? _t02;
        public string? t02
        {
            get => _t02;
            set => this.RaiseAndSetIfChanged(ref _t02, value);

        }
        private string? _t03;
        public string? t03
        {
            get => _t03;
            set => this.RaiseAndSetIfChanged(ref _t03, value);

        }
        private string? _t04;
        public string? t04
        {
            get => _t04;
            set => this.RaiseAndSetIfChanged(ref _t04, value);

        }
        private string? _t05;
        public string? t05
        {
            get => _t05;
            set => this.RaiseAndSetIfChanged(ref _t05, value);

        }
        private string? _t06;
        public string? t06
        {
            get => _t06;
            set => this.RaiseAndSetIfChanged(ref _t06, value);

        }
        private string? _t07;
        public string? t07
        {
            get => _t07;
            set => this.RaiseAndSetIfChanged(ref _t07, value);

        }


        // public string Greeting { get; } = "Welcome to Avalonia!";
        private string? _t11;
        public string? t11
        {
            get => _t11;
            set => this.RaiseAndSetIfChanged(ref _t11, value);

        }
        private string? _t12;
        public string? t12
        {
            get => _t12;
            set => this.RaiseAndSetIfChanged(ref _t12, value);

        }
        private string? _t13;
        public string? t13
        {
            get => _t13;
            set => this.RaiseAndSetIfChanged(ref _t13, value);

        }
        private string? _t14;
        public string? t14
        {
            get => _t14;
            set => this.RaiseAndSetIfChanged(ref _t14, value);

        }
        private string? _t15;
        public string? t15
        {
            get => _t15;
            set => this.RaiseAndSetIfChanged(ref _t15, value);

        }
        private string? _t16;
        public string? t16
        {
            get => _t16;
            set => this.RaiseAndSetIfChanged(ref _t16, value);

        }
        private string? _t17;
        public string? t17
        {
            get => _t17;
            set => this.RaiseAndSetIfChanged(ref _t17, value);

        }

        Ribbon ribbon = new Ribbon();

        public MainWindowViewModel()
        {
            IndexInRibbon = 0;
            ribbon.ReadXML("ribbon1.xml");
            ShowRibbon(ribbon, IndexInRibbon);
            SolveCommand = ReactiveCommand.Create(SolveQ);
        }

        private void SolveQ()
        {
            try            
            {
                ribbon.ReadXML("ribbon1.xml");
                TuringMachine turingMachine = new TuringMachine();
                //turingMachine.ReadXML("turing1.xml");
                turingMachine.ReadXML("turing2.xml");

                turingMachine.Solve(ribbon);
                ShowRibbon(ribbon, 0);
                ribbon.SaveXML("outribbon1.xml");

                Debug.WriteLine("test");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            //throw new NotImplementedException();
        }

        private void ShowRibbon(Ribbon ribbon, int startPos)
        {
            if (ribbon != null)
            {
                if (ribbon.ribbonCells != null)
                {
                    t01 = startPos.ToString();
                    t02 = (startPos + 1).ToString();
                    t03 = (startPos + 2).ToString();
                    t04 = (startPos + 3).ToString();
                    t05 = (startPos + 4).ToString();
                    t06 = (startPos + 5).ToString();
                    t07 = (startPos + 6).ToString();

                    if (ribbon.ribbonCells.Count > startPos)
                        t11 = ribbon.ribbonCells[0 + startPos].RibbonSymbol.ToString();
                    else
                        t11 = "";
                    if (ribbon.ribbonCells.Count > (startPos + 1))
                        t12 = ribbon.ribbonCells[1 + startPos].RibbonSymbol.ToString();
                    else
                        t12 = "";
                    if (ribbon.ribbonCells.Count > (startPos + 2))
                        t13 = ribbon.ribbonCells[2 + startPos].RibbonSymbol.ToString();
                    else
                        t13 = "";
                    if (ribbon.ribbonCells.Count > (startPos + 3))
                        t14 = ribbon.ribbonCells[3 + startPos].RibbonSymbol.ToString();
                    else
                        t14 = "";
                    if (ribbon.ribbonCells.Count > (startPos + 4))
                        t15 = ribbon.ribbonCells[4 + startPos].RibbonSymbol.ToString();
                    else
                        t15 = "";
                    if (ribbon.ribbonCells.Count > (startPos + 5))
                        t16 = ribbon.ribbonCells[5 + startPos].RibbonSymbol.ToString();
                    else
                        t16 = "";
                    if (ribbon.ribbonCells.Count > (startPos + 6))
                        t17 = ribbon.ribbonCells[6 + startPos].RibbonSymbol.ToString();
                    else
                        t17 = "";
                }
            }
        }

        internal void OnKey(Key key)
        {
            switch (key)
            {
                case Avalonia.Input.Key.Left:
                    IndexInRibbon--;
                    if (IndexInRibbon < 0) IndexInRibbon = 0;
                    break;
                case Avalonia.Input.Key.Right:
                    IndexInRibbon++;
                    if (IndexInRibbon >(ribbon.ribbonCells.Count-1) ) IndexInRibbon = ribbon.ribbonCells.Count - 1;
                    break;
                default:
                    int hh = 1;
                    break;
            }
            ShowRibbon(ribbon, IndexInRibbon);
        }
    }
}
