using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaTuring.Models;
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

        private Avalonia.Media.IBrush? _brush1;
        public Avalonia.Media.IBrush? Brush1
        {
            get => _brush1;
            set=> this.RaiseAndSetIfChanged(ref _brush1, value);
        }
        private Avalonia.Media.IBrush? _brush2;
        public Avalonia.Media.IBrush? Brush2
        {
            get => _brush2;
            set => this.RaiseAndSetIfChanged(ref _brush2, value);
        }
        private Avalonia.Media.IBrush? _brush3;
        public Avalonia.Media.IBrush? Brush3
        {
            get => _brush3;
            set => this.RaiseAndSetIfChanged(ref _brush3, value);
        }
        private Avalonia.Media.IBrush? _brush4;
        public Avalonia.Media.IBrush? Brush4
        {
            get => _brush4;
            set => this.RaiseAndSetIfChanged(ref _brush4, value);
        }
        private Avalonia.Media.IBrush? _brush5;
        public Avalonia.Media.IBrush? Brush5
        {
            get => _brush5;
            set => this.RaiseAndSetIfChanged(ref _brush5, value);
        }
        private Avalonia.Media.IBrush? _brush6;
        public Avalonia.Media.IBrush? Brush6
        {
            get => _brush6;
            set => this.RaiseAndSetIfChanged(ref _brush6, value);
        }
        private Avalonia.Media.IBrush? _brush7;
        public Avalonia.Media.IBrush? Brush7
        {
            get => _brush7;
            set => this.RaiseAndSetIfChanged(ref _brush7, value);
        }
        private Avalonia.Media.IBrush? _brush8;
        public Avalonia.Media.IBrush? Brush8
        {
            get => _brush8;
            set => this.RaiseAndSetIfChanged(ref _brush8, value);
        }

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

        bool isNeedRepaint;

        Ribbon ribbon = new Ribbon();

        private DispatcherTimer? _timer;

        public MainWindowViewModel()
        {
            IndexInRibbon = 0;
            ribbon.ReadXML("ribbon1.xml");
            isNeedRepaint = true;
            
            SolveCommand = ReactiveCommand.Create(SolveQ);
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.1); 
            _timer.Tick += Timer_Tick; 
            _timer.Start(); 
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (isNeedRepaint)
            {
                ShowRibbon(ribbon, IndexInRibbon);
                isNeedRepaint=false;
            }
        }

        public void StopTimer()
        {
            _timer.Stop();
        }

        TuringMachine? turingMachine;
        private void SolveQ()
        {
            try
            {
                Brush1 = Brushes.Beige;
                ribbon.ReadXML("ribbon1.xml");
                turingMachine = new TuringMachine();
                //turingMachine.ReadXML("turing1.xml");
                turingMachine.ReadXML("turing2.xml");
                turingMachine.Solve(ribbon);
                
                IndexInRibbon = 0;
                isNeedRepaint=true;
                
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
            try
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

                        int ribPos = ribbon.currentPosition - startPos;
                        
                        if ((ribPos >= 0)&&(ribPos<8))
                        {
                            if (ribPos==0)
                                Brush1 = Brushes.Beige;
                            else
                                Brush1 = Brushes.Black;
                            if (ribPos == 1)
                                Brush2 = Brushes.Beige;
                            else
                                Brush2 = Brushes.Black;
                            if (ribPos == 2)
                                Brush3 = Brushes.Beige;
                            else
                                Brush3 = Brushes.Black;
                            if (ribPos == 3)
                                Brush4 = Brushes.Beige;
                            else
                                Brush4 = Brushes.Black;
                            if (ribPos == 4)
                                Brush5 = Brushes.Beige;
                            else
                                Brush5 = Brushes.Black;
                            if (ribPos == 5)
                                Brush6 = Brushes.Beige;
                            else
                                Brush6 = Brushes.Black;
                            if (ribPos == 6)
                                Brush7 = Brushes.Beige;
                            else
                                Brush7 = Brushes.Black;
                            if (ribPos == 7)
                                Brush8 = Brushes.Beige;
                            else
                                Brush8 = Brushes.Black;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        internal void OnKey(Key key)
        {
            try
            {
                switch (key)
                {
                    case Avalonia.Input.Key.Left:
                        IndexInRibbon--;
                        if (IndexInRibbon < 0) IndexInRibbon = 0;
                        break;
                    case Avalonia.Input.Key.Right:
                        IndexInRibbon++;
                        if (IndexInRibbon > (ribbon.ribbonCells.Count - 1)) IndexInRibbon = ribbon.ribbonCells.Count - 1;
                        break;
                    default:
                        int hh = 1;
                        break;
                }
                //IndexInRibbon = 0;
                isNeedRepaint = true;
                //ShowRibbon(ribbon, IndexInRibbon);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        internal void OnClose(WindowClosingEventArgs e)
        {
         _timer?.Stop();
        }
    }
}
