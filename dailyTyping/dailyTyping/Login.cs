using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace dailyTyping
{
    public partial class Login : Form
    {
        TextBox[] txtList;
        const string IdPlaceholder = "Id (email)";
        const string PwPlaceholder = "Password";

        public Login()
        {
            InitializeComponent();

            txtList = new TextBox[] { textBox1, textBox2 };

            foreach (var txt in txtList)
            {
                txt.ForeColor = Color.DarkGray;
                if (txt == textBox1)
                {
                    txt.Text = IdPlaceholder;
                }

                if (txt == textBox2)
                {
                    txt.Text = PwPlaceholder;
                }

                txt.GotFocus += RemovePlaceholder;
                txt.LostFocus += SetPlaceholder;

            }

        }

        private void SetPlaceholder(object? sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                //사용자 입력값이 하나도 없는 경우에 포커스 잃으면 Placeholder 적용해주기
                txt.ForeColor = Color.DarkGray; //Placeholder 흐린 글씨
                if (txt == textBox1) { 
                    txt.Text = IdPlaceholder; 
                }
                
                if (txt == textBox2) { 
                    txt.Text = PwPlaceholder; 
                    textBox2.PasswordChar = default; 
                }
            }
        }

        private void RemovePlaceholder(object? sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text == IdPlaceholder | txt.Text == PwPlaceholder)
            { //텍스트박스 내용이 사용자가 입력한 값이 아닌 Placeholder일 경우에만, 커서 포커스일때 빈칸으로 만들기
                txt.ForeColor = Color.Black; //사용자 입력 진한 글씨
                txt.Text = string.Empty;
                if (txt == textBox2) textBox2.PasswordChar = '●';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
            this.Close();
        }
    }
}
