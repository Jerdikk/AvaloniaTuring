using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AvaloniaTuring.ViewModels
{

    public sealed class GlobalData
    {
        #region Singleton
        private static object syncRoot = new Object();
        private static volatile GlobalData instance;
        public static int programNum = 0;
        public static string mutexString;
        public static Mutex m = null;

        public static GlobalData Instance
        {
            get
            {
                if (instance == null)
                {

                    bool doesNotExist = false;
                    bool otherException = false;
                    bool createdMutex = false;

                    while (!createdMutex)
                    {
                        mutexString = "Turing" + programNum.ToString();
                        try
                        {
                            // Open the mutex with (MutexRights.Synchronize |
                            // MutexRights.Modify), to enter and release the
                            // named mutex.
                            //                                                       

                            m = Mutex.OpenExisting(mutexString);


                        }
                        catch (WaitHandleCannotBeOpenedException)
                        {
                            Console.WriteLine("Mutex does not exist.");
                            doesNotExist = true;
                            m = new Mutex(false, mutexString, out createdMutex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception in mutex: {0}", ex.Message);
                            otherException = true;
                        }

                        if ((m != null) && (!doesNotExist) && (!otherException))
                        {
                            // m.ReleaseMutex();
                            m = null;
                            programNum++;
                        }

                    }


                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GlobalData();
                    }
                }

                return instance;
            }
        }
        #endregion

        public int gTuringObjectNum;
        public GlobalData()
        {
            gTuringObjectNum = 0;
        }
    }

    public abstract class TuringObject
    {
        public int ID;
        public abstract int GetID();
    }

    public enum TuringDirection { Left, Right, Stay };

    public class RibbonCell : TuringObject
    {
        private char _RibbonSymbol;
        public char RibbonSymbol
        {
            get { return _RibbonSymbol; }
            set { _RibbonSymbol = value; }
        }

        public RibbonCell(char t1)
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
            Set(t1);
        }

        public RibbonCell()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
        }

        public void Set(char value)
        {
            this.RibbonSymbol = value;
        }

        public char Get()
        {
            return this.RibbonSymbol;
        }

        public override int GetID()
        {
            return ID;
        }
    }

    public class Ribbon : TuringObject
    {
        public string In;
        public string Out;
        public int currentPosition;
        public List<RibbonCell> ribbonCells;



        public bool ReadXML(string file)
        {
            try
            {
                ribbonCells.Clear();
                In = "";
                Out = "";
                currentPosition = 0;

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

                        if (xnode.Name == "In")
                        {
                            Debug.WriteLine($"in: {xnode.InnerText}");
                            this.In = xnode.InnerText;
                        }
                        // если узел age
                        if (xnode.Name == "Out")
                        {
                            Debug.WriteLine($"out: {xnode.InnerText}");
                            this.Out = xnode.InnerText;
                        }

                        // обходим все дочерние узлы элемента user

                        /*  foreach (XmlNode childnode in xnode.ChildNodes)
                          {
                              // если узел - company
                              if (childnode.Name == "In")
                              {
                                  Debug.WriteLine($"Company: {childnode.InnerText}");
                              }
                              // если узел age
                              if (childnode.Name == "Out")
                              {
                                  Debug.WriteLine($"Age: {childnode.InnerText}");
                              }
                          }*/
                        Debug.WriteLine(" --- - ");
                    }

                    int len1 = this.In.Length;

                    for (int i = 0; i < len1; i++)
                    {
                        char t1 = this.In[i];

                        ribbonCells.Add(new RibbonCell(t1));

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

        public bool SaveXML(string file)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                XmlElement? xRoot = xDoc.CreateElement("Ribbon");

                XmlDeclaration xmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);


                XmlElement inElement = xDoc.CreateElement("In");
                XmlText inText = xDoc.CreateTextNode(this.In);
                XmlText outText = xDoc.CreateTextNode(this.Out);
                inElement.AppendChild(inText);
                XmlElement outElement = xDoc.CreateElement("Out");
                outElement.AppendChild(outText);
                xRoot.AppendChild(inElement);
                xRoot.AppendChild(outElement);
                xDoc.AppendChild(xRoot);
                xDoc.InsertBefore(xmlDeclaration, xRoot);
                xDoc.Save(file);
                Debug.WriteLine("saved xml");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        public Ribbon()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
            currentPosition = 0;
            ribbonCells = new List<RibbonCell>();

        }

        public override int GetID()
        {
            return ID;
        }


        public bool Left()
        {
            if (currentPosition > 0)
            {
                currentPosition--;
                return true;
            }
            return false;
        }
        public bool Right()
        {
            if (currentPosition < (ribbonCells.Count - 1))
            {
                currentPosition++;
                return true;
            }
            return false;
        }

        public bool Top()
        {
            currentPosition = 0;
            return true;
        }

        public bool Move(TuringDirection direction)
        {
            if (direction == TuringDirection.Left)
            {
                return Left();
            }
            if (direction == TuringDirection.Right)
            {
                return Right();
            }
            if (direction == TuringDirection.Stay)
            {
                return true;
            }

            return false;
        }

        public static void Serialize(Ribbon ribbon, string fileName)
        {
            try
            {
                // передаем в конструктор тип класса Person
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Ribbon));

                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, ribbon);

                    Debug.WriteLine("Object has been serialized");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public static Ribbon Deserialize(string fileName)
        {
            Ribbon? ribbon = new Ribbon();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Ribbon));

                // десериализуем объект
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    ribbon = xmlSerializer.Deserialize(fs) as Ribbon;
                    Debug.WriteLine($"Object has been deserialized");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return ribbon;
        }

        internal char ReadCell()
        {
            try
            {
                return ribbonCells[currentPosition].RibbonSymbol;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return '\0';
            }
        }

        public void SetCellValue(char currentSymbol)
        {
            try
            {
                ribbonCells[currentPosition].RibbonSymbol = currentSymbol;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        internal void Add(char v)
        {
            ribbonCells.Add(new RibbonCell(v));
        }
    }

    public class Rule : TuringObject
    {

        public int nextState;
        public int currentState;
        public char currentSymbol;
        public char symbolToWrite;
        public TuringDirection currDirection;
        public bool isRule;

        public Rule()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
            isRule = false;
        }

        public override int GetID()
        {
            return ID;
        }

    }

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

                    if ((currAlphabetNum >= 0) && (currAlphabetNum < maxAlphabet))
                    {
                        Rule rule1 = GetRules(currAlphabetNum, currentTuringMachineState);

                        if ((rule1!=null)&&(rule1.isRule))
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
                        if ((currAlphabetNum == -1) && (i == maxAlphabet))
                        {
                            Rule rule2 = GetRules(i, currentTuringMachineState);
                            if ((rule2!=null)&&(rule2.isRule))
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
