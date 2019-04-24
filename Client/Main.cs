using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using ChatApp;
using System.Collections;
namespace ChatApplication
{
    public partial class Main : Form
    {

        private TcpChannel tcpClient;
        private IChat chat;
        private ClientProxy clientProxy;
        private Thread connectionThread;
        /// <summary>
        /// Initialize clients & create thread to listen from server
        /// </summary>
        public Main()
        {

            InitializeComponent();
            BinaryClientFormatterSinkProvider binaryClient = new BinaryClientFormatterSinkProvider();//convert messages in form of binary rather than xml
            BinaryServerFormatterSinkProvider binaryServer = new BinaryServerFormatterSinkProvider();//convert messages in form of binary rather than xml
            binaryServer.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;//cover all events 
            Hashtable properties = new Hashtable();
            properties["Name"] = "ClientChannel";
            properties["port"] = 0;//means any available port in client.
            tcpClient = new TcpChannel(properties, binaryClient, binaryServer);
            clientProxy = new ClientProxy();
            clientProxy.MessageArrived += ClientProxy_MessageArrived;
            connectionThread = new Thread(ConnectToServer);
            connectionThread.IsBackground = true;
        }

        private void ClientProxy_MessageArrived(string Msg)
        {
            SetText(Msg);
        }

        private delegate void SetTextCallBack(string msg);//handle SetText method to call it in the  UI thread

        /// <summary>
        /// Write string to conversationBox textbox, this method can be used by different threads to write on conversationBox textbox
        /// </summary>
        /// <param name="Msg">
        /// string to write in conversationBox textbox
        /// </param>
        private void SetText(string Msg)
        {
            try
            {
                if (this.ConversationBox.InvokeRequired)//check if caller Id is different from creator id 
                {
                    SetTextCallBack setText = new SetTextCallBack(SetText);
                    BeginInvoke(setText, Msg);//call delegate from creator thread
                }
                else//if method called from creator thread
                {
                    ConversationBox.AppendText(Msg + "\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Check if tcp channel is registered before
        /// </summary>
        /// <returns></returns>
        private bool IsRegistered()
        {
            foreach (var Channel in ChannelServices.RegisteredChannels)
            {
                if (Channel.ChannelName == tcpClient.ChannelName)
                {
                    return true;
                }
            }
            return false;
        }
        private void ConnectToServer()
        {
            try
            {
                if (string.IsNullOrEmpty(NameBox.Text))//check if name is inserted or not
                {
                    throw new NullReferenceException();//throw exception
                }
                if (!IsRegistered())
                {
                    ChannelServices.RegisterChannel(tcpClient, false);
                }
                chat = (IChat)Activator.GetObject(typeof(IChat), "tcp://" + AddressBox.Text + ":9988/Chat");
                if (chat.GetNumOfClients() == 3)
                {
                    throw new FullException();

                }
                chat.MessageArrived += new MessageArrivedEvent(clientProxy.ProxyBroadCastMessage);
                chat.BroadCastMessage(NameBox.Text + " Connected");
                InformationPanel.Enabled = false;
            }
            catch (FullException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintErrors(ex.Message, ex);
                chat = null;
            }
            catch (NullReferenceException ex)//catch exception that fired when Namebox is empty
            {
                MessageBox.Show("Please write user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintErrors("Please write user name", ex);
                chat = null;
            }
            catch (FormatException ex)//catch exception that fired when AddressBox is either empty or contain invalid text
            {

                if (AddressBox.Text == "")//check if AddressBox is empty 
                {
                    MessageBox.Show("Please write IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PrintErrors("Please write IP Address", ex);
                }
                else//check if AddressBox  contain invalid text than can't be converted into IP
                {
                    MessageBox.Show("Incorrect IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PrintErrors("Incorrect IP Address", ex);
                }
                ClearBox(AddressBox);
                chat = null;
            }
            catch (Exception ex)//handle any exception than may occur
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintErrors(ex);
                ClearBox(AddressBox);
                chat = null;
            }
        }

        public delegate void ClearTextBoxes(TextBox box);

        private void ClearBox(TextBox box)//this method will be used by thread 
        {
            if (box.InvokeRequired)
            {
                ClearTextBoxes clearTextBoxes = new ClearTextBoxes(ClearBox);
                BeginInvoke(clearTextBoxes, box);
            }
            else
            {
                box.Text = "";
            }
        }
        /// <summary>
        /// create connection between client and server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)//start thread for connection.
        {
            if (connectionThread.ThreadState == (System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Background))
            {
                connectionThread.Start();
            }
            else if (connectionThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                connectionThread = new Thread(ConnectToServer);
                connectionThread.Start();
            }
        }
        /// <summary>
        /// send text from MsgBox into server using the established connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MsgBox.Text != "")//if no message written then no data to send
                {
                    chat.BroadCastMessage(string.Concat(NameBox.Text + " : ", MsgBox.Text));
                    MsgBox.Text = "";//clear message box    
                    MsgBox.Focus();//give message box foucs to write next message
                }
            }
            catch (DisconnectedException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintErrors(ex);
                chat = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintErrors(ex);
                chat = null;
            }
        }

        private void MsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                SendButton_Click(null, null);
            }
        }

        private void AddressBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConnectButton_Click(null, null);
            }
        }
        /// <summary>
        /// Notify server that client will close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (chat != null)
                {
                    chat.DecrementNumOfClients();//notify server that you are exiting the room.
                }
            }
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
            }
        }
        /// <summary>
        /// Print exception message in Error.txt file in the application folder.
        /// </summary>
        /// <param name="ex">
        /// exception that occured
        /// </param>
        private void PrintErrors(Exception ex)
        {
            string ErrorPath = System.IO.Directory.GetParent(@"..\..\..\").FullName;
            using (StreamWriter stream = new StreamWriter(ErrorPath + @"\Error.txt", true))
            {
                stream.WriteLine("Date : " + DateTime.Now.ToLocalTime());
                stream.WriteLine("Stack trace :");
                stream.WriteLine(ex.StackTrace);
                stream.WriteLine("Message :");
                stream.WriteLine(ex.Message);
                stream.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
        }
        /// <summary>
        /// Print exception message in Error.txt file in the application folder.
        /// </summary>
        /// <param name="Message">
        /// error message
        /// </param>
        /// <param name="ex">
        /// exception to write it's stack trace
        /// </param>
        private void PrintErrors(string Message, Exception ex)
        {
            string ErrorPath = System.IO.Directory.GetParent(@"..\..\..\").FullName;
            using (StreamWriter stream = new StreamWriter(ErrorPath + @"\Error.txt", true))
            {
                stream.WriteLine("Date : " + DateTime.Now.ToLocalTime());
                stream.WriteLine("Stack trace :");
                stream.WriteLine(ex.StackTrace);
                stream.WriteLine("Message :");
                stream.WriteLine(Message);
                stream.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
        }
    }
}
