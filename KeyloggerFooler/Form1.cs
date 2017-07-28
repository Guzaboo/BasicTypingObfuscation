using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeyloggerFooler
{
    public partial class Form1 : Form
    {
        string last = "";
        string lastlast = "";
        string randomChars = "";
        System.Timers.Timer charTimer = null;
        static Form1 form;

        public Form1()
        {
            form = this;
            InitializeComponent();
            textBox2.Select();
            textBox2.Focus();
            this.Show();
            SetVisibleCore(true);
            typeRandChars();
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
                value = false;
            }
            base.SetVisibleCore(value);
        }

        private void typeRandChars()
        {
            charTimer = new System.Timers.Timer(random.Next(100) + 150);
            charTimer.Enabled = true;
            charTimer.Elapsed += typeRandChar;
            charTimer.Start();
        }

        private void typeRandChar(Object source, ElapsedEventArgs e)
        {
            charTimer.Enabled = false;
            string s = RandomString(1);
            form.type(s);
            typeRandChars();
        }

        public void type(string s)
        {
            lastlast = last;
            last = s;
            form.Invoke((MethodInvoker)delegate {
                form.textBox2.Select();
                form.textBox2.Focus();
                if (GFW.IsActive(form.Handle))
                    SendKeys.SendWait(s);
            });
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnmABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (randomChars.Length > textBox2.Text.Length)
            {
                textBox1.Text = backspace();
                randomChars = textBox2.Text;
                lastlast = getActualLastLast();
                last = getActualLast();
                return;
            }
            if (!(last.Equals(getActualLast()) && lastlast.Equals(getActualLastLast()))){
                
                textBox1.Text += textBox2.Text.Substring(textBox2.TextLength - 1);
                lastlast = last;
                last = textBox2.Text.Substring(textBox2.TextLength - 1);
            }
            randomChars = textBox2.Text;
        }

        private string getActualLast()
        {
            try
            {
                return textBox2.Text.Substring(textBox2.TextLength - 1);
            }
            catch {
                return "";
            }
        }

        private string backspace()
        {
            if (textBox1.Text.Length > 0)
            {
                return textBox1.Text.Substring(0, textBox1.TextLength - 1);
            }
            else
            {
                return textBox1.Text;
            }
        }

        private string getActualLastLast()
        {
            try
            {
                return textBox2.Text.Substring(textBox2.TextLength - 2,1);
            }
            catch
            {
                return "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBox1.Text);
        }
    }

    internal class GFW
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static bool IsActive(IntPtr handle)
        {
            IntPtr activeHandle = GetForegroundWindow();
            return (activeHandle == handle);
        }
    }
}
