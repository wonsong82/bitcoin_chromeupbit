using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Utils
{
    class Log
    {
        public string name;
        public string fileName;

        public Log(string name)
        {
            this.name = name;
            this.fileName = name + ".log";

            if (!File.Exists(fileName))
            {
                FileStream stream = File.Create(fileName);
                stream.Close();
            }
        }


        public List<string> read(bool async = false)
        {
            if (async)
            {
                Thread thread = new Thread(() => readFile(true));
                thread.Start();
                return null; // return by event
            }
            else
                return readFile();
        }


        public void write(string data, bool async = false)
        {
            if (async)
            {
                Thread thread = new Thread(() => writeFile(data, true));
                thread.Start();
            }
            else
                writeFile(data);
        }

        public void write(List<string> data, bool async = false)
        {
            if (async)
            {
                Thread thread = new Thread(() => writeFile(data, true));
                thread.Start();
            }
            else
                writeFile(data);
        }

        public void clear(bool async = false)
        {
            if (async)
            {
                Thread thread = new Thread(() => clearFile(true));
                thread.Start();
            }
            else
                clearFile();
        }



        






        protected List<string> readFile(bool triggerEvent = false)
        {
            FileInfo file = new FileInfo(this.fileName);
            bool isReadable = false;
            FileStream stream = null;
            List<string> data = new List<string>();

            while(!isReadable)
            {
                try
                {
                    stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (stream != null)
                        stream.Close();
                    TextReader reader = new StreamReader(this.fileName);
                    while (reader.Peek() >= 0)
                    {
                        string d = reader.ReadLine();
                        data.Add(d);
                    }
                    reader.Close();
                    isReadable = true;

                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
                catch (IOException)
                {
                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
            }

            return data;
        }

        protected void writeFile(string data, bool triggerEvent = false)
        {
            FileInfo file = new FileInfo(this.fileName);
            bool isReadable = false;
            FileStream stream = null;

            while (!isReadable)
            {
                try
                {
                    stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
                    if (stream != null)
                        stream.Close();
                    TextWriter writer = new StreamWriter(this.fileName, true);
                    writer.WriteLine(data);
                    writer.Close();
                    isReadable = true;

                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
                catch (IOException)
                {
                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
            }
        }


        protected void writeFile(List<string> data, bool triggerEvent = false)
        {
            FileInfo file = new FileInfo(this.fileName);
            bool isReadable = false;
            FileStream stream = null;

            while (!isReadable)
            {
                try
                {
                    stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
                    if (stream != null)
                        stream.Close();
                    TextWriter writer = new StreamWriter(this.fileName, true);
                    foreach (string d in data)
                    {
                        writer.WriteLine(d);
                    }
                    writer.Close();
                    isReadable = true;

                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
                catch (IOException)
                {
                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
            }
        }


        protected void clearFile(bool triggerEvent = false)
        {
            FileInfo file = new FileInfo(this.fileName);
            bool isReadable = false;
            FileStream stream = null;

            while (!isReadable)
            {
                try
                {
                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    if (stream != null)
                        stream.Close();
                    TextWriter writer = new StreamWriter(this.fileName);
                    writer.Write("");
                    writer.Close();
                    isReadable = true;

                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
                catch (IOException)
                {
                    if (triggerEvent)
                    {
                        // Trigger event
                    }
                }
            }
        }


    }
}
