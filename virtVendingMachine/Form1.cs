using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace virtVendingMachine
{
    public partial class Form1 : Form
    {
        private string AlphaSelector = String.Empty;
        private int NumericSelector = -1;
        private double Balance = 0;
        protected List<List<string>> labelnames = new List<List<string>>();
        Inventory stock = null;
        static TcpListener server;
        static readonly object o = new object();
        bool exit = false;

        public Form1(Inventory inventory)
        {
            InitializeComponent();
            inventory.FillInventory();
            UpdateLabels(inventory);
            stock = inventory;
            Console.WriteLine(inventory.GetAvailableItems());
            Console.WriteLine(inventory.GetPriceList());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlphaSelector = "A";
            if (lblSelection.Text.Length < 12)
                lblSelection.Text = lblSelection.Text + "A";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AlphaSelector = "B";
            if (lblSelection.Text.Length < 12)
                lblSelection.Text = lblSelection.Text + "B";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AlphaSelector = "C";
            if (lblSelection.Text.Length < 12)
                lblSelection.Text = lblSelection.Text + "C";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AlphaSelector = "D";
            if (lblSelection.Text.Length < 12)
                lblSelection.Text = lblSelection.Text + "D";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AlphaSelector = "E";
            if (lblSelection.Text.Length < 12)
                lblSelection.Text = lblSelection.Text + "E";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AlphaSelector = "F";
            if (lblSelection.Text.Length < 12)
                lblSelection.Text = lblSelection.Text + "F";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NumericSelector = 1;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "1";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NumericSelector = 2;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "2";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            NumericSelector = 3;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "3";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            NumericSelector = 4;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "4";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            NumericSelector = 5;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "5";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            NumericSelector = 6;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "6";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            NumericSelector = 7;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "7";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            NumericSelector = 8;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "8";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            NumericSelector = 9;
            if (lblSelection.Text.Length == 12 && Char.IsLetter(lblSelection.Text, 11))
                lblSelection.Text = lblSelection.Text + "9";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //Submit
            if (txtMoney.Text == String.Empty)
            {
                if (AlphaSelector != String.Empty && NumericSelector > 0)
                {
                    //get price and display price
                    Item item = stock.items[string.Format("{0}{1}", AlphaSelector, NumericSelector)];
                    if (item.Count < 1)
                    {
                        MessageBox.Show(string.Format("Your selection {0} {1} is out of stock. Please select another item", item.Selector, item.Name));
                        ResetSelection();
                    }
                    else if (item.Count > 0)
                    {
                        if (Balance > item.Price)
                        {
                            item.Count -= 1;
                            lblCoinReturn.Text = string.Format("Coin Return: ${0}", (Balance - item.Price).ToString());
                            MessageBox.Show(string.Format("Your selection, {0} is ready below. Please retrieve your change", item.Name));
                            CompleteTransaction();
                        }
                        else if (Balance == item.Price)
                        {
                            item.Count -= 1;
                            MessageBox.Show(string.Format("Your selection, {0} is ready below", item.Name));
                            CompleteTransaction();

                        }
                        else
                        {
                            MessageBox.Show(string.Format("You need ${0} more to make this selection", (item.Price - Balance).ToString()));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please make a selection");
                }
            }
            else
            {
                //parse txtMoney.Text
                ParseMoney();
            }
            lblBalance.Text = string.Format("Balance: ${0}", Balance);
            txtMoney.Text = String.Empty;
        }

        private void ParseMoney()
        {
            if (isDouble(txtMoney.Text))
                Balance += double.Parse(txtMoney.Text);
            else
            {
                if (txtMoney.Text.Contains('$'))
                {
                    if (isDouble(txtMoney.Text.Replace('$', '0')))
                    {
                        Balance += double.Parse(txtMoney.Text.Replace('$', '0'));
                    }
                }
                else if (txtMoney.Text.Contains("nickel"))
                {
                    Balance += 0.05;
                }
                else if (txtMoney.Text.Contains("dime"))
                {
                    Balance += 0.10;
                }
                else if (txtMoney.Text.Contains("quarter"))
                {
                    Balance += 0.25;
                }
                else if (txtMoney.Text.Contains("five dollar"))
                {
                    Balance += 5;
                }
                else if (txtMoney.Text.Contains("dollar"))
                {
                    Balance += 1;
                }
                else
                    MessageBox.Show("Invalid money");
            }
        }

        private void CompleteTransaction()
        {
            lblBalance.Text = "Balance: ";
            Balance = 0;
            ResetSelection();
            CoinReturn();
        }

        private void CoinReturn()
        {
            if (Balance > 0)
                lblCoinReturn.Text = string.Format("Coin Return: ${0}", Balance.ToString());
            else
                lblCoinReturn.Text = "Coin Return: ";
        }

        private void ResetSelection()
        {
            AlphaSelector = String.Empty;
            NumericSelector = -1;
            lblSelection.Text = "Selection: ";
        }

        private void Cancel()
        {
            CoinReturn();
            lblBalance.Text = "Balance: ";
            Balance = 0;
            ResetSelection();
        }

        private bool isDouble(string data)
        {
            try
            {
                double d = double.Parse(data);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Cancel
            if (Balance > 0)
                lblCoinReturn.Text = string.Format("Coin Return: ${0}", Balance.ToString());
            else
                lblCoinReturn.Text = "Coin Return: ";
            lblBalance.Text = "Balance: ";
            Balance = 0;
            AlphaSelector = String.Empty;
            NumericSelector = -1;
            lblSelection.Text = "Selection: ";
            
        }

        private void UpdateLabels(Inventory stock)
        {
            foreach (Item item in stock.items.Values)
            {
                switch(item.Selector)
                {
                    case "A1":
                        label1.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "A2":
                        label2.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "A3":
                        label3.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "A4":
                        label4.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "A5":
                        label5.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "A6":
                        label6.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "B1":
                        label7.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "B2":
                        label8.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "B3":
                        label9.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "B4":
                        label10.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "B5":
                        label11.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "B6":
                        label12.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "C1":
                        label13.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "C2":
                        label14.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "C3":
                        label15.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "C4":
                        label16.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "C5":
                        label17.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "C6":
                        label18.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "D1":
                        label19.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "D2":
                        label20.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "D3":
                        label21.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "D4":
                        label22.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "D5":
                        label23.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "D6":
                        label24.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "E1":
                        label25.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "E2":
                        label26.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "E3":
                        label27.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "E4":
                        label28.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "E5":
                        label29.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "E6":
                        label30.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "F1":
                        label31.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "F2":
                        label32.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "F3":
                        label33.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "F4":
                        label34.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "F5":
                        label35.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                    case "F6":
                        label36.Text = string.Format("{0} {1} ${2}", item.Selector, item.Name, item.Price);
                        break;
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip = new IPAddress(host.AddressList.First().GetAddressBytes());

            server = new TcpListener(ip, 3000);
            server.Start();
            Console.WriteLine("Server started on port 3000");

            while (!exit)
            {
                if (server.Pending())
                {
                    new Thread(ServerService).Start();
                }
            }

            server.Stop();
        }

        private void ServerService()
        {
            Socket socket = server.AcceptSocket();

            Console.WriteLine("Connected " + socket.RemoteEndPoint);

            Stream stream = new NetworkStream(socket);
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);

            writer.AutoFlush = true;

            bool client = false;

            new Thread(() =>
            {
                while (!client)
                {
                    string s = "";
                    s = reader.ReadLine();

                    if (s == "req")
                    {
                        writer.Write("Test request successful!");
                        writer.Flush();
                    }

                    if (s == "exit")
                    {
                        writer.WriteLine("Exiting...");
                        exit = true;
                    }
                }
            }).Start();
        }

    }
}
