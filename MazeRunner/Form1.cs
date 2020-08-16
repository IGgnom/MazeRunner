using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace MazeRunner
{
    public partial class Form1 : Form
    {
        public string PngFile = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics Graph = this.CreateGraphics();
            int x1, y1;
            x1 = 10;
            y1 = 10;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Brush PixelBrush = new SolidBrush(this.BackColor);
                    Graph.FillRectangle(PixelBrush, x1, y1, 25, 25);
                    x1 += 25;
                }
                x1 = 10;
                y1 += 25;
            }

            try
            {
                if (button1.Text == "Выбрать")
                {
                    openFileDialog1.FileName = "";
                    openFileDialog1.ShowDialog();
                    textBox1.Text = openFileDialog1.FileName;
                    button1.Text = "Выбрать";
                }
                else
                {
                    button1.Text = "Выбрать";
                }

                PngFile = textBox1.Text.Remove(textBox1.Text.LastIndexOf(@"\"));
                richTextBox1.Text += "Файл загружен \n";

                if (File.Exists(PngFile + @"\maze.txt"))
                    File.Delete(PngFile + @"\maze.txt");
                FileStream CreateTxt = File.Create(PngFile + @"\maze.txt");

                if (File.Exists(PngFile + @"\maze.py"))
                    File.Delete(PngFile + @"\maze.py");
                FileStream CreatePy = File.Create(PngFile + @"\maze.py");

                richTextBox1.Text += "Дополнительные файлы созданы \n";

                string TextPy = "from pybrain.tools.shortcuts import buildNetwork \n" +
                    "from pybrain.datasets import SupervisedDataSet \n" +
                    "from pybrain.supervised.trainers import BackpropTrainer \n" +
                    "from PIL import Image \n" +
                    "im = Image.open(r'" + textBox1.Text + "') \n" +
                    "pix = im.load() \n" +
                    "maze = [] \n" +
                    "for i in range(im.size[0]): \n" +
                    "	maze.append([]) \n" +
                    "	for j in range(im.size[1]): \n" +
                    "		if pix[j,i] == (0,0,0) or pix[j,i] == (1,1,1): \n" +
                    "			maze[i].append(1) \n" +
                    "		elif pix[j,i] == (255, 255, 255) or pix[j,i] == (254, 254, 254): \n" +
                    "			maze[i].append(0) \n" +
                    "inputs = [[1,1,1,0,0,0,0,1],[1,1,0,1,0,0,1,0],[0,1,1,1,1,0,0,0],[1,0,1,1,0,1,0,0],[1,0,0,1,0,1,0,0],[1,0,0,1,0,0,1,0],[0,1,1,0,1,0,0,0],[0,1,1,0,0,0,0,1],[0,0,1,1,0,1,0,0],[0,0,1,1,1,0,0,0],[0,1,0,1,0,0,1,0],[0,1,0,1,1,0,0,0],[1,1,0,0,0,0,0,1],[1,1,0,0,0,0,1,0],[1,0,1,0,0,1,0,0],[1,0,1,0,0,0,0,1],[0,0,0,1,1,0,0,0],[0,0,0,1,0,0,1,0],[0,0,0,1,0,1,0,0],[0,0,1,0,0,1,0,0],[0,0,1,0,1,0,0,0],[0,0,1,0,0,0,0,1],[1,0,0,0,0,1,0,0],[1,0,0,0,0,0,0,1],[1,0,0,0,0,0,1,0],[0,1,0,0,0,0,1,0],[0,1,0,0,1,0,0,0],[0,1,0,0,0,0,0,1]] \n" +
                    "outputs = [[0,0,0,1],[0,0,1,0],[1,0,0,0],[0,1,0,0],[0,0,1,0],[0,1,0,0],[0,0,0,1],[1,0,0,0],[1,0,0,0],[0,1,0,0],[1,0,0,0],[0,0,1,0],[0,0,1,0],[0,0,0,1],[0,0,0,1],[0,1,0,0],[0,1,0,0],[1,0,0,0],[0,0,1,0],[0,0,0,1],[0,1,0,0],[0,1,0,0],[0,0,0,1],[0,0,1,0],[0,0,0,1],[0,0,0,1],[0,0,1,0],[1,0,0,0]] \n" +
                    "net = buildNetwork(8,16,4, bias = True) \n" +
                    "ds = SupervisedDataSet(8,4) \n" +
                    "coords = [] \n" +
                    "for i, j in zip(inputs, outputs): \n" +
                    "	ds.addSample(tuple(i), tuple(j)) \n" +
                    "trainer = BackpropTrainer(net, ds) \n" +
                    "for i in range(500): \n" +
                    "	trainer.train() \n" +
                    "for i in range(im.size[0]): \n" +
                    "	if maze[0][i] == 0: \n" +
                    "		posX = 0 \n" +
                    "		posY = i \n" +
                    "coords.append([posX,posY]) \n" +
                    "maze[posX][posY] = 2 \n" +
                    "posX = posX + 1 \n" +
                    "for i in range(im.size[0]): \n" +
                    "	if maze[im.size[1] - 1][i] == 0: \n" +
                    "		posendX = im.size[1] \n" +
                    "		posendY = i \n" +
                    "coords.append([posX,posY]) \n" +
                    "step = [] \n" +
                    "while posX != posendX - 1: \n" +
                    "	if maze[posX - 1][posY] != 1: \n" +
                    "		step.append(0) \n" +
                    "	else: step.append(1) \n" +
                    "	if maze[posX][posY - 1] != 1: \n" +
                    "		step.append(0) \n" +
                    "	else: step.append(1) \n" +
                    "	if maze[posX][posY + 1] != 1: \n" +
                    "		step.append(0) \n" +
                    "	else: step.append(1) \n" +
                    "	if maze[posX + 1][posY] != 1: \n" +
                    "		step.append(0) \n" +
                    "	else: step.append(1) \n" +
                    "	if maze[posX - 1][posY] == 2: \n" +
                    "		step.append(1) \n" +
                    "	else: step.append(0) \n" +
                    "	if maze[posX][posY - 1] == 2: \n" +
                    "		step.append(1) \n" +
                    "	else: step.append(0) \n" +
                    "	if maze[posX][posY + 1] == 2: \n" +
                    "		step.append(1) \n" +
                    "	else: step.append(0) \n" +
                    "	if maze[posX + 1][posY] == 2: \n" +
                    "		step.append(1) \n" +
                    "	else: step.append(0) \n" +
                    "	compute = net.activate(step) \n" +
                    "	for i in range(im.size[0]): \n" +
                    "		for j in range(im.size[1]): \n" +
                    "			if maze[i][j] == 2: \n" +
                    "				maze[i][j] = 0 \n" +
                    "	maze[posX][posY] = 2 \n" +
                    "	for i in range(len(compute)): \n" +
                    "		if compute[i] == max(compute): \n" +
                    "			if i == 0: \n" +
                    "				posX -= 1 \n" +
                    "			elif  i == 1: \n" +
                    "				posY -= 1 \n" +
                    "			elif  i == 2: \n" +
                    "				posY += 1 \n" +
                    "			elif  i == 3: \n" +
                    "				posX += 1 \n" +
                    "	coords.append([posX, posY]) \n" +
                    "	step.clear() \n" +
                    "for i in range(im.size[0]): \n" +
                    "		for j in range(im.size[1]): \n" +
                    "			if maze[i][j] == 2: \n" +
                    "				maze[i][j] = 0 \n" +
                    "maze[posX][posY] = 2 \n" +
                    "coords.append([posX, posY]) \n" +
                    "file = open(r'" + PngFile + @"\maze.txt" + "', 'w') \n" +
                    "for i in coords: \n" +
                    "    print(i, file= file) \n";

                byte[] TextBytes = Encoding.Default.GetBytes(TextPy);
                CreatePy.Write(TextBytes, 0, TextBytes.Length);
                CreatePy.Close();
                CreateTxt.Close();

                richTextBox1.Text += "Нейросеть загружена \n";
                DrawMaze(textBox1.Text);
            }
            catch { }
        }

        public void DrawMaze(string ImagePath)
        {
            Bitmap Img = new Bitmap(ImagePath);
            Graphics Graph = this.CreateGraphics();
            int x1, y1;
            x1 = 10;
            y1 = 10;
            for (int i = 0; i < Img.Width; i++)
            {
                for (int j = 0; j < Img.Height; j++)
                {
                    Color Pixel = Img.GetPixel(j, i);
                    Brush PixelBrush = new SolidBrush(Pixel);
                    Graph.FillRectangle(PixelBrush, x1, y1, 25, 25);
                    x1 += 25;
                }
                x1 = 10;
                y1 += 25;
            }
            richTextBox1.Text += "Отображение лабиринта \n";
        }

        public void DrawStep()
        {
            for (int i = 0; i < 100; i++)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    richTextBox1.Text = "Процесс обучения:" + Convert.ToString(i % 100 + 1) + "% \n";
                });
                Thread.Sleep(200);
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                richTextBox1.Text += "Обучение завершено \n";
                richTextBox1.Text += "Прохождение лабиринта \n";
            });

            int Step = 0;
            string Coords;
            string FileTxt = textBox1.Text.Remove(textBox1.Text.LastIndexOf(@"\"));
            Graphics Graph = this.CreateGraphics();
            StreamReader Reader = new StreamReader(FileTxt + @"\maze.txt");

            while ((Coords = Reader.ReadLine()) != null)
            {
                Coords = Coords.Remove(Coords.IndexOf('['), 1);
                Coords = Coords.Remove(Coords.IndexOf(']'), 1);
                Coords = Coords.Remove(Coords.IndexOf(','), 1);
                string[] CoordsXY = Coords.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Graph.FillRectangle(new SolidBrush(Color.FromArgb(102, 255, 0)), Convert.ToInt32(CoordsXY[1]) * 25 + 10, Convert.ToInt32(CoordsXY[0]) * 25 + 10, 25, 25);
                Thread.Sleep(100);
                this.Invoke((MethodInvoker)delegate ()
                {
                    richTextBox1.Text = "Шаг №" + Convert.ToString(Step++) + ": X:" + CoordsXY[1] + ", Y:" + CoordsXY[0] + "\n";
                });
                Graph.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), Convert.ToInt32(CoordsXY[1]) * 25 + 10, Convert.ToInt32(CoordsXY[0]) * 25 + 10, 25, 25);
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                richTextBox1.Text += "Лабиринт пройден! \n";
            });
            Reader.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == null)
                button1.Text = "Выбрать";
            else
                button1.Text = "Установить";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "Запуск обучения \n";
            string FilePy = textBox1.Text.Remove(textBox1.Text.LastIndexOf(@"\"));
            var ProcessPy = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = FilePy,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                }
            };
            ProcessPy.Start();

            using (StreamWriter ProcessWriter = ProcessPy.StandardInput)
            {
                if (ProcessWriter.BaseStream.CanWrite)
                {
                    ProcessWriter.WriteLine("python maze.py");
                }
            }

            Thread DrawThread = new Thread(new ThreadStart(DrawStep));
            DrawThread.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(PngFile + @"\maze.txt"))
                File.Delete(PngFile + @"\maze.txt");

            if (File.Exists(PngFile + @"\maze.py"))
                File.Delete(PngFile + @"\maze.py");
        }
    }
}
