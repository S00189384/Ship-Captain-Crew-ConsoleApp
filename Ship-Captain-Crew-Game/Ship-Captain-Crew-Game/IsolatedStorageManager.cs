using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ship_Captain_Crew_Game
{
    public class IsolatedStorageManager
    {
        public static Object synObj = new Object();
        private IsolatedStorageFile store;

        public string folderName;
        public string textFileName;
        public string pathToTextFile;

        public string json;
        public bool jsonFileExists;

        public IsolatedStorageManager(string folderName,string textFileName)
        {
            this.folderName = folderName;
            this.textFileName = textFileName;

            pathToTextFile = $"{folderName}\\{textFileName}";

            store = IsolatedStorageFile.GetUserStoreForDomain();
        }

        public void CheckDirectoryExists()
        {
            if (store != null)
            {
                Monitor.Enter(synObj);
                try
                {
                    if (!store.FileExists(pathToTextFile))
                    {
                        jsonFileExists = false;
                    }
                    else
                    {
                        jsonFileExists = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Monitor.Pulse(synObj);
                    Monitor.Exit(synObj);
                }
            }
        }

        public void writeToStorage(object objectToWrite)
        {
            if (store != null)
            {
                Monitor.Enter(synObj);
                try
                {
                    if (!store.DirectoryExists(folderName))
                        store.CreateDirectory(folderName);

                    using (IsolatedStorageFileStream isoStorageTxtFile =
                        store.OpenFile(pathToTextFile, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(isoStorageTxtFile))
                        {
                            writer.Write(objectToWrite);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Monitor.Pulse(synObj);
                    Monitor.Exit(synObj);
                }
            }

        }

        public void readFromStorage()
        {
            if (store != null)
            {
                Monitor.Enter(synObj);
                try
                {
                    using (IsolatedStorageFileStream isoStorageTxtFile = store.OpenFile(pathToTextFile, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader reader = new StreamReader(isoStorageTxtFile))
                        {
                            json = reader.ReadToEnd();
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Monitor.Pulse(synObj);
                    Monitor.Exit(synObj);
                }
            }
        }
    }
}
