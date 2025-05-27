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
                XmlText inText = xDoc.CreateTextNode("123445");
                XmlText outText = xDoc.CreateTextNode("53421");
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

        public int currentPosition;
        public List<RibbonCell> ribbonCells;

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

    }

    public class Rules : TuringObject
    {

        public int nextstate;
        public char currSymbol;
        public TuringDirection currDirection;
        public bool isRule;

        public Rules()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
        }

        public override int GetID()
        {
            return ID;
        }

    }

    public class TuringMachine : TuringObject
    {

        public TuringMachine()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
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
                                }
                            }
                        }
                        // если узел age
                        if (xnode.Name == "Rules")
                        {
                            foreach (XmlNode childnode in xnode.ChildNodes)
                            {
                                // если узел - company
                                if (childnode.Name == "Rule")
                                {

                                    XmlNode? attr = childnode.Attributes.GetNamedItem("CurentSymbol");

                                    Debug.WriteLine($"rule: {attr?.Value}");
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
    }
}
