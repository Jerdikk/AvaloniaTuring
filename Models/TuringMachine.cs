using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace AvaloniaTuring.Models
{
    public class TuringMachine : TuringObject
    {
        int currentTuringMachineState;
        public List<char> Alphabet = new List<char>();
        public int maxStates;
        public List<Rule> Rules = new List<Rule>();
        public TuringMachine()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
            currentTuringMachineState = 0;
            maxStates = 0;
        }

        public override int GetID()
        {
            return ID;
        }
        public bool ReadXML(string file)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(file);
                // получим корневой элемент
                XmlElement? xRoot = xDoc.DocumentElement;

                if (xRoot != null)
                {
                    // обход всех узлов в корневом элементе
                    foreach (XmlElement xnode in xRoot)
                    {
                        // получаем атрибут name
                        // XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                        //  Console.WriteLine(attr?.Value);

                        if (xnode.Name == "Alphabet")
                        {
                            foreach (XmlNode childnode in xnode.ChildNodes)
                            {
                                // если узел - company
                                if (childnode.Name == "Symbol")
                                {
                                    Debug.WriteLine($"symbol: {childnode.InnerText}");
                                    try
                                    {
                                        if (childnode != null)
                                            Alphabet.Add(char.Parse(childnode.InnerText));
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.ToString());
                                    }
                                }
                            }
                        }
                        // если узел age
                        if (xnode.Name == "Rules")
                        {
                            XmlNode? mStates = xnode.Attributes.GetNamedItem("MaxStates");
                            if (mStates != null)
                            {
                                maxStates = int.Parse(mStates.Value);
                            }

                            foreach (XmlNode childnode in xnode.ChildNodes)
                            {
                                // если узел - company
                                if (childnode.Name == "Rule")
                                {
                                    Rule rule = new Rule();
                                    XmlNode? cSym = childnode.Attributes.GetNamedItem("CurentSymbol");
                                    if (cSym != null)
                                    {
                                        rule.currentSymbol = char.Parse(cSym.Value);
                                    }
                                    XmlNode? cState = childnode.Attributes.GetNamedItem("CurrentState");
                                    if (cState != null)
                                    {
                                        rule.currentState = int.Parse(cState.Value);
                                    }
                                    XmlNode? sToWrite = childnode.Attributes.GetNamedItem("SymbolToWrite");
                                    if (sToWrite != null)
                                    {
                                        rule.symbolToWrite = char.Parse(sToWrite.Value);
                                    }
                                    XmlNode? nState = childnode.Attributes.GetNamedItem("NextState");
                                    if (nState != null)
                                    {
                                        rule.nextState = int.Parse(nState.Value);
                                    }
                                    XmlNode? bRul = childnode.Attributes.GetNamedItem("IsRule");
                                    if (bRul != null)
                                    {
                                        if (int.Parse(bRul.Value) == 1)
                                            rule.isRule = true;
                                        else
                                            rule.isRule = false;
                                    }
                                    XmlNode? dir = childnode.Attributes.GetNamedItem("Direction");
                                    if (dir != null)
                                    {
                                        char t1 = char.Parse(dir.Value);
                                        switch (t1)
                                        {
                                            case 'R':
                                                rule.currDirection = TuringDirection.Right;
                                                break;
                                            case 'L':
                                                rule.currDirection = TuringDirection.Left;
                                                break;
                                            case 'N':
                                                rule.currDirection = TuringDirection.Stay;
                                                break;

                                        }

                                        // Debug.WriteLine($"rule: {attr?.Value}");
                                    }
                                    Rules.Add(rule);
                                }
                            }

                        }


                        Debug.WriteLine(" --- - ");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        Rule GetRules(int aplhaNum, int currState)
        {
            try
            {
              //  Rule rule = new Rule();
                char currSymbol = Alphabet[aplhaNum];
                foreach (Rule rule1 in Rules)
                {
                    if (rule1.isRule)
                    {
                        if (rule1.currentState == currState)
                        {
                            if (rule1.currentSymbol==currSymbol)
                            {
                                return rule1;
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;

            }
        }


        public bool Solve(Ribbon ribbon)
        {
            if (ribbon == null) return false;
            try
            {
                ribbon.Top();

                bool done = false;
                char tempChar;
                int currAlphabetNum;
                int maxAlphabet = Alphabet.Count;
                currentTuringMachineState = 0;
                int i = 0;
                do
                {
                    tempChar = ribbon.ReadCell();
                    currAlphabetNum = -1;
                    for (i = 0; i < maxAlphabet; i++)
                        if (tempChar == Alphabet[i])
                        {
                            currAlphabetNum = i;
                            i = maxAlphabet;
                        }

                    if (currAlphabetNum >= 0 && currAlphabetNum < maxAlphabet)
                    {
                        Rule rule1 = GetRules(currAlphabetNum, currentTuringMachineState);

                        if (rule1!=null&&rule1.isRule)
                        {                            
                            currentTuringMachineState = DoStep(ribbon, rule1);
                            if (currentTuringMachineState == maxStates)
                            {
                                List<char> t11 = new List<char>();
                                foreach (RibbonCell cell in ribbon.ribbonCells)
                                {
                                    t11.Add(cell.RibbonSymbol);
                                }
                                ribbon.Out = new string(t11.ToArray());
                                done = true;
                            }
                        }
                    }
                    else
                    {
                        /// символ не найден в алфавите
                        if (currAlphabetNum == -1 && i == maxAlphabet)
                        {
                            Rule rule2 = GetRules(i, currentTuringMachineState);
                            if (rule2!=null&&rule2.isRule)
                            {
                                /*ribbon.SetCellValue(rule2.symbolToWrite);
                                ribbon.Move(rule2.currDirection);
                                currentTuringMachineState = rule2.nextState;*/
                                currentTuringMachineState = DoStep(ribbon, rule2);
                                if (currentTuringMachineState == maxStates)
                                    done = true;
                            }
                        }
                    }

                    

                } while (!done);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        private static int DoStep(Ribbon ribbon, Rule rule)
        {
            ribbon.SetCellValue(rule.symbolToWrite);
            if (!ribbon.Move(rule.currDirection))
            {
                if (rule.currDirection == TuringDirection.Right)
                {
                    ribbon.Add('*');
                    if (!ribbon.Move(rule.currDirection))
                    {
                        //std::cout << " Error !!!!";
                        Debug.WriteLine(" Error !!!!");
                        //system("PAUSE");
                    }
                }
            }
            return rule.nextState;
        }
    }
}
