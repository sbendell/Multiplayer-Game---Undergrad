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
    public partial class ChatBox : Form
    {

        public string text;

        public ChatBox()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            text = textBox1.Text;
            Hide();
            Network.outmsg = Network.Client.CreateMessage();
            Console.WriteLine(textBox1.Text);
            Network.outmsg.Write("chat");
            Network.outmsg.Write(Login.userName + ": " + textBox1.Text);
            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);
            textBox1.Clear();
        }

        private void ChatBox_Load(object sender, EventArgs e)
        {

        }
    }
}
