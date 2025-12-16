using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AvaloniaTuring.Models
{
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
                            In = xnode.InnerText;
                        }
                        // если узел age
                        if (xnode.Name == "Out")
                        {
                            Debug.WriteLine($"out: {xnode.InnerText}");
                            Out = xnode.InnerText;
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

                    int len1 = In.Length;

                    for (int i = 0; i < len1; i++)
                    {
                        char t1 = In[i];

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
                XmlText inText = xDoc.CreateTextNode(In);
                XmlText outText = xDoc.CreateTextNode(Out);
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
            if (currentPosition < ribbonCells.Count - 1)
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
}
