namespace dailyTyping
{
    public partial class Form1 : Form
    {
        Boolean state = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!state)
            {
                Hook.SetHook(IntPtr.Zero, dailyTypingCount);                
            }
        }
    }
}