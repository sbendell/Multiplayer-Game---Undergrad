using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lidgren.Network;

namespace Client
{
    public partial class Login : Form
    {
        public static string userName;
        public static bool IsConnected = false;

        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            userName = textBox1.Text;
            Network.Client.Start();
            Network.Client.Connect("127.0.0.1", 12345);
            System.Threading.Thread.Sleep(300);
            Network.outmsg = Network.Client.CreateMessage();
            Network.outmsg.Write("connect");
            Network.outmsg.Write(userName);
            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);
            IsConnected = true;
            Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
