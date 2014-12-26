using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace arduinoCommunication
{
    public partial class Main : Form
    {

        SerialPort port;
        public void InitializeArduino(String listeningPort, int boundrate) 
        {
            try
            {
                port = new SerialPort(listeningPort, boundrate);
                port.BaudRate = 9600;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                port.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string line = sp.ReadLine();
            this.BeginInvoke(new LineReceivedEvent(LineReceived), line);
        }

        private delegate void LineReceivedEvent(string line);

        private void LineReceived(string line)
        {
            txtLog.Text = txtLog.Text + line + "\n";
        }

        public Main()
        {
            InitializeComponent();
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeArduino("COM3", 9600);
            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                cmbPorts.Items.Add(port);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            InitializeArduino(cmbPorts.Text, Convert.ToInt32(txtBoundRate.Text));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            String s = txtSend.Text;
            port.Write(s);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            port.Close();
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
    
    }
}
