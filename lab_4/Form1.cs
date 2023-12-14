using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // (a^b) mod c
        static int findAPowBModC(int a, int b, int c)
        {
            if (b == 0)
                return 1;
            long temp = findAPowBModC(a, b / 2, c);
            long result = (temp * temp) % c;
            // Если степень нечетная, умножаем на a
            if (b % 2 == 1)
                result = (result * a) % c;
            return (int)result;
        }
        // Шифрование
        private void button1_Click(object sender, EventArgs e)
        {
            int p = (int) numericUpDown1.Value;
            int q = (int) numericUpDown2.Value;
            int n = p * q;
            int eNum = (int) numericUpDown3.Value;
            int m = (int)numericUpDown5.Value;
            label10.Text = "Зашифрованное число: " + findAPowBModC(m, eNum, n); 
        }

        // Расшифрование
        private void button2_Click(object sender, EventArgs e)
        {
            int d = (int)numericUpDown7.Value;
            int n = (int)numericUpDown6.Value;
            int s = (int)numericUpDown8.Value;
            label14.Text = "Расшифрованное число: " + findAPowBModC(s, d, n);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int p = (int)numericUpDown1.Value;
            int q = (int)numericUpDown2.Value;
            label5.Text = "n: " + p * q;
            label6.Text = "φ(n):" + (p - 1) * (q - 1);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int p = (int)numericUpDown1.Value;
            int q = (int)numericUpDown2.Value;
            label5.Text = "n: " + p * q;
            label6.Text = "φ(n):" + (p - 1) * (q - 1);
        }
    }
}
