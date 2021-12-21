using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Files
{
    public partial class Form1 : Form
    {
        int foldersCreated;
        string DeskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public Form1()
        {
            InitializeComponent();
            fetchFoldersCreated();
            this.Text = "Making Some Files";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            foldersCreated += 1;
            makeSomeFiles(foldersCreated);
            updateNumberFile(foldersCreated);
            stopWatch.Stop();
            textBox1.Text = $"Elapsed time: {stopWatch.ElapsedMilliseconds.ToString()}ms. Now check your desktop.";
        }

        void fetchFoldersCreated()
        {
            string folder = "ManyManyFiles";
            var file = Path.Combine(DeskPath, folder);
            try
            {
                if (Directory.Exists(file))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(file);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(file));

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally {

                string FileName = "DO NOT OPEN THIS FILE.txt";
                if (File.Exists(Path.Combine(DeskPath, folder, FileName)))
                {
                    StreamReader sr = new StreamReader(Path.Combine(DeskPath, folder, FileName));

                    string str = sr.ReadLine();

                    sr.Close();
                    if (Int32.TryParse(str, out int j))
                    {
                        Console.WriteLine(j);
                        foldersCreated = j;
                    }
                    else
                    {
                        Console.WriteLine("String could not be parsed.");
                        foldersCreated = -1;
                    }
                }
                else
                {
                    File.Create(Path.Combine(DeskPath, folder, FileName)).Close();
                    StreamWriter sw = new StreamWriter(Path.Combine(DeskPath, folder, FileName));
                    sw.Write("0");
                    foldersCreated = 0;
                    sw.Close();
                }
            }
        }

        void updateNumberFile(int num)
        {
            StreamWriter sw = new StreamWriter(Path.Combine(DeskPath, "ManyManyFiles", "DO NOT OPEN THIS FILE.txt"));
            sw.Write(num);
            foldersCreated = num;
            sw.Close();
        }

        void makeSomeFiles(int folderNum)
        {
            string folder = Path.Combine(DeskPath, "ManyManyFiles", $"File Batch {folderNum}");
            Directory.CreateDirectory(folder);
            Random rnd = new Random();

            void makeFile()
            {
                string randLetters = "";
                for (int i = 0; i < 8; i++)
                {
                    randLetters += Convert.ToChar(rnd.Next(65, 91));
                }

                string filePath = Path.Combine(folder, $"{randLetters}.json");
                File.Create(filePath).Close();
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine( "{\n"+ $"\t\"randomCharacters\" : \"{randLetters}\"" + "\n}");                
                sw.Close();

            }

            // ASYNC

            for (int i = 1; i <= 5000; i++)
            { 
                Task.Run(makeFile);     // ASYNCHRONOUS
                //makeFile();             // SYNCHRONOUS
            }
        }
    }
}
