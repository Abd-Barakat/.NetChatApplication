using System;
using System.Windows.Forms;
using System.Threading;
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
        /// <summary>
        /// Initialize clients & create thread to listen from server
        /// </summary>
        public Main()
        {
            try
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
            }
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
            }
        }

        private void ClientProxy_MessageArrived(string Msg)
        {
            try
            {
                SetText(Msg);
            }
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
            }
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
            try
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
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
                return false;
            }
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
                chat.BroadCastMessage(NameBox.Text + " is connected");
                DisableInfoPanel();
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
        public delegate void DisableInformationPanel();
        /// <summary>
        /// Disable Information Panel from Connection Thread after connection successed.
        /// </summary>
        private void DisableInfoPanel()
        {
            try
            {
                if (InformationPanel.InvokeRequired)
                {
                    DisableInformationPanel disable = new DisableInformationPanel(DisableInfoPanel);
                    BeginInvoke(disable);
                }
                else
                {
                    InformationPanel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
            }
        }
        /// <summary>
        /// Clear Msg Box after delivering the previous message.
        /// </summary>
        /// <param name="box"></param>
        private void ClearBox(TextBox box)//this method will be used by thread 
        {
            try
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
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
            }
        }
        /// <summary>
        /// create connection between client and server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)//start thread for connection.
        {
            try
            {
                ThreadPool.QueueUserWorkItem((x) => { ConnectToServer(); });
            }
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
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
                InformationPanel.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintErrors(ex);
                InformationPanel.Enabled = true;
                chat = null;
            }
        }

        private void MsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    SendButton_Click(null, null);
                }
            }
            catch(Exception ex)
            {
                PrintErrors(ex.Message, ex);
            }
        }

        private void AddressBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    ConnectButton_Click(null, null);
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
            try
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
            catch (Exception NewEx)
            {
                MessageBox.Show(NewEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
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
            catch (Exception NewEx)
            {
                MessageBox.Show(NewEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        ///  Notify server that client will close.
        /// </summary>
        private void Closing_Client()
        {
            try
            {
                if (chat != null)
                {
                    chat.DecrementNumOfClients();//decrement number of clients 
                    chat.MessageArrived -= clientProxy.ProxyBroadCastMessage;//remove client event handler from server delegate.
                    ChannelServices.UnregisterChannel(tcpClient);//remove current tcp port to make it free for other procces 
                }
            }
            catch (Exception ex)
            {
                //Server is  Shutdown.
                PrintErrors(ex.InnerException.Message, ex);
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem((x) => { Closing_Client(); });
            }
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
            }
        }
    }
}
