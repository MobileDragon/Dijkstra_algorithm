using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRAF
{
    
    public partial class Form1 : Form
    {
        private Graphics g;
        private Graf graf;
        private int r;
        private List<int> puth = new List<int>();
        public Form1()
        {
            InitializeComponent();
            Pen peak = new Pen(Color.Black, 20);
            g = pictureBox1.CreateGraphics();
            graf = new Graf(50);
            r = pictureBox1.Width/2;
            if (pictureBox1.Height/2 < r)
                r = pictureBox1.Height/2;
            button1.Click += new EventHandler(button2_Click);
            for (int i = 0; i < 50; i++)
            {
                comboBox2.Items.Add(Convert.ToString(i));
                comboBox3.Items.Add(Convert.ToString(i));
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            button2_Click(sender, e);
        }

        private void Clrscr()
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Application.DoEvents();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pen peak = new Pen(Color.Green , 2);
            int R = r - 20;
            for (int i = 0; i < 50; i++)
            {
                int x = point_at_Circle(r,r,R, 50, i)[0]+10;
                int y = point_at_Circle(r,r,R, 50, i)[1]+10;
                g.DrawEllipse(peak, x - 10, y - 10, 20, 20);
                Font f = new Font(this.Font.FontFamily, 8);
                g.DrawString(Convert.ToString(i), f, Brushes.Black, new Point(x-8, y-7));
            }
            drow_Line();
            drow_Way(puth);


        }
        private int[] point_at_Circle(int x, int y, int r,int max,int n)//получение координвт на окружности
        {

            int[] mas_xy = new int[2];
            double tangle = 360.0 / max;
            mas_xy[0] = Convert.ToInt32(Math.Cos((tangle * n) * Math.PI / 180) * r + x);
            mas_xy[1] = Convert.ToInt32(Math.Sin((tangle * n) * Math.PI / 180) * r + y);
            return (mas_xy);

        }

        private void drow_Line()//рисование линий между вершинами
        {
            Pen line = new Pen(Color.Black, 1);
            int R = r-31;
            for (int i = 0; i < 50; i++)
            {
                int x1 = point_at_Circle(r,r,R, 50, i)[0] + 10;
                int y1 = point_at_Circle(r,r,R, 50, i)[1] + 10;
                for (int j = i; j < 50; j++)//поиск ребер
                {
                    if (graf.PriceLine(i, j) > 0)
                    {
                        int x2 = point_at_Circle(r,r,R, 50, j)[0] + 10;
                        int y2 = point_at_Circle(r,r,R, 50, j)[1] + 10;
                        g.DrawLine(line, x1, y1, x2, y2);//рисуем
                    }
                }
            }
        }
        private void drow_Way(List<int> puth)//рисование линий между вершинами
        {
            Pen line = new Pen(Color.Blue, 3);
            int R = r - 31;
            for (int j = 0; j < puth.Count-1;j++)
            {
                int x1 = point_at_Circle(r, r, R, 50, puth[j])[0] + 10;
                int y1 = point_at_Circle(r, r, R, 50, puth[j])[1] + 10;
                int x2 = point_at_Circle(r, r, R, 50, puth[j+1])[0] + 10;
                int y2 = point_at_Circle(r, r, R, 50, puth[j+1])[1] + 10;
                g.DrawLine(line, x1, y1, x2, y2);//рисуем траекторию
                int x = (x1 + x2) / 2;
                int y = (y1 + y2) / 2;
                Font f = new Font(this.Font.FontFamily, 12);
                g.DrawString(Convert.ToString(graf.PriceLine(puth[j], puth[j + 1])),
                            f, Brushes.Red, new Point(x, y));
            }
        }
        private void whotPrice()//отображение цены выбранных вершин
        {
            if (String.IsNullOrEmpty(comboBox2.Text) || String.IsNullOrEmpty(comboBox3.Text))
                return;
            int cena=graf.PriceLine(Convert.ToInt32(comboBox2.Text), Convert.ToInt32(comboBox3.Text));
            if (cena < 0)
            {
                textBox1.ForeColor = Color.Red;
                textBox1.Text = "-1";
                return;
            }
            textBox1.ForeColor = Color.Green;
            textBox1.Text = Convert.ToString(cena);
        }

        private void button1_Click(object sender, EventArgs e)//добавление связи
        {

            if (String.IsNullOrEmpty(comboBox2.Text) || String.IsNullOrEmpty(comboBox3.Text) || String.IsNullOrEmpty(textBox1.Text))
                return;
            if (Convert.ToInt32(comboBox2.Text) > 50 || Convert.ToInt32(comboBox2.Text)<0
                || Convert.ToInt32(comboBox3.Text) > 50 || Convert.ToInt32(comboBox3.Text)<0)
                return;
            if(Convert.ToInt32(comboBox2.Text)== Convert.ToInt32(comboBox3.Text)
                ||Convert.ToInt32(textBox1.Text)<0||Convert.ToInt32(textBox1.Text)>100000)
                return;

            graf.AddLine(Convert.ToInt32(comboBox2.Text), Convert.ToInt32(comboBox3.Text), Convert.ToInt32(textBox1.Text));
            whotPrice();
            Clrscr();
            button2_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)//удаление связи
        {
            if (String.IsNullOrEmpty(comboBox2.Text) || String.IsNullOrEmpty(comboBox3.Text))
                return;
            if (Convert.ToInt32(comboBox2.Text) > 50 || Convert.ToInt32(comboBox2.Text) < 0
                || Convert.ToInt32(comboBox3.Text) > 50 || Convert.ToInt32(comboBox3.Text) < 0)
                return;

            graf.DelLine(Convert.ToInt32(comboBox2.Text), Convert.ToInt32(comboBox3.Text));
            whotPrice();
            Clrscr();
            button2_Click(sender, e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else textBox1.ForeColor = Color.Black;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true; 
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            whotPrice();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            whotPrice();
        }

        private void comboBox2_KeyUp(object sender, KeyEventArgs e)
        {
            whotPrice();
        }

        private void comboBox3_KeyUp(object sender, KeyEventArgs e)
        {
            whotPrice();
        }

        private void button4_Click(object sender, EventArgs e)//вывод минимального маршрута
        {
            puth=graf.MinimumWay(Convert.ToInt32(comboBox2.Text), Convert.ToInt32(comboBox3.Text));
            label2.Text = "";
            //label2.Text = Convert.ToString(puth.Count);
            drow_Way(puth);
            for (int i = 0; i < puth.Count; i++)
            {

                label2.Text += puth[i];
                label2.Text += ", ";
            }
            textBox1.ForeColor = Color.Blue;
            textBox1.Text = Convert.ToString(graf.CenaofPeak(Convert.ToInt32(comboBox3.Text)));
        }

    }
    public struct Line
    {
        public int dot1;
        public int dot2;
        public int Value;
    }
    public struct Dot
    {
        public bool visit;
        public int price;
        public List<int>way;
    }
    public class Graf
    {
        private int kol = 0;
        private Dot[] dot;
        private List<Line> lines;
        public Graf(int size)
        {
            dot = new Dot[size];
            lines = new List<Line>();
            kol = size;
            Dot t = new Dot();
            t.visit = false;
            t.price = 100000;
            t.way = new List<int>();
            for (int i = 0; i < size; i++)
                dot[i]=t;
        }
        public void AddLine(int peac1,int peac2,int cena)
        {
            if (lines == null)
                return;
            Line t=new Line();
            DelLine(peac1, peac2);
            t.dot1=peac1;
            t.dot2 = peac2;
            t.Value = cena;
            lines.Add(t);
        }
        public void DelLine(int peac1, int peac2)
        {
            if (lines==null)
                return;
            Line t = new Line();
            foreach (Line line in lines)
            {
                if (line.dot1 == peac1 && line.dot2 == peac2)
                    t = line;
                if (line.dot1 == peac2 && line.dot2 == peac1)
                    t = line;
            }
            lines.Remove(t);
        }
        public int PriceLine(int peac1, int peac2)
        {
            if (lines == null)
                return(-1);
            Line t = new Line();
            t.Value=-1;
            foreach (Line line in lines)
            {
                if (line.dot1 == peac1 && line.dot2 == peac2)
                    t = line;
                if (line.dot1 == peac2 && line.dot2 == peac1)
                    t = line;
            }
            return(t.Value);
        }
        public List<int> WhoCanSee(int d1)
        {
            List<int> masd2 = new List<int>();
            if (lines == null)
                return masd2;

            foreach (Line line in lines)
            {
                
                if (line.dot1 == d1)
                    masd2.Add(line.dot2);
                else
                    if (line.dot2 == d1)
                        masd2.Add(line.dot1);
            }
            return masd2;
        }
        public List<int> MinimumWay(int peac1, int peak2)
        {
            for (int t = 0; t < kol; t++)
            {
                dot[t].price = 10000;
                dot[t].visit = false;
            }
            dot[peac1].way = new List<int>();
            dot[peac1].way.Add(peac1);
            dot[peac1].price = 0;

            List<int> watch = new List<int>();//исследуемые вершины

            watch.Add(peac1);

            int i=peac1;
            int index = 0;
            while(index<watch.Count)
            {
                i = watch[index++];
                if (i != peak2)
                {
                    dot[i].visit = true;
                    foreach (int j in WhoCanSee(i))
                    {

                        if (!dot[j].visit)//если вершина не есть полностью исследована
                        {
                            
                            if (dot[j].price > dot[i].price + PriceLine(i, j))//был ли найден более дешевый путь к вершине?
                            {
                                dot[j].way =new List<int>(dot[i].way);
                                dot[j].way.Add(j);
                                dot[j].price = dot[i].price + PriceLine(i, j);
                                watch.Add(j);
                            }
                        }
                    }
                    
                }
                //if(i==peak2)
                    
            }
            return (dot[peak2].way);
            //return new List<int>();
        }
        public int CenaofPeak(int peac1)
        {
            return dot[peac1].price;
        }



    }
}
