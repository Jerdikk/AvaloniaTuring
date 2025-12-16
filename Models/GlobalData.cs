using System;
using System.Threading;

namespace AvaloniaTuring.Models
{
    public sealed class GlobalData
    {
        #region Singleton
        private static object syncRoot = new object();
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

                        if (m != null && !doesNotExist && !otherException)
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
}
