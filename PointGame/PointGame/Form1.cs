using System.Net.Sockets;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PointGame
{
    public partial class Form1 : Form
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;

        public Form1()
        { 
            InitializeComponent();
            listOfUsers.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string host = "127.0.0.1";
            int port = 8888;
            string userName = enterName.Text;

            try
            {
                _client = new TcpClient();
                _client.Connect(host, port);

                _reader = new StreamReader(_client.GetStream());
                _writer = new StreamWriter(_client.GetStream()) { AutoFlush = true };

                // ��������� ����� ����� ��� ��������� ������
                Task.Run(() => ReceiveMessageAsync(_reader));

                // ���������� ��� ������������
                await EnterUserAsync(_writer, userName);

                // ��������� ���������
                label1.Text = userName;
                label1.Visible = false;
                enterName.Visible = false;
                btn_signIn.Visible = false;
                listOfUsers.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ �����������: {ex.Message}");
            }
        }

        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // ��������� ����� � ���� ������
                    string message = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(message)) continue;

                    // ��������� ��������� � �������������� Invoke, ��� ��� ��� ���������� � ��������� ������
                    Invoke((MethodInvoker)delegate
                    {
                        Print(message);
                    });
                }
                catch (IOException)
                {
                    // ���������� ���������, ���� ���������� �� ��������� ������
                    // ����� ���������, ���� ������ �������� �������
                    MessageBox.Show("������ �������� �������.");
                    break;
                }
                catch (Exception)
                {
                    // ����� ������ ����������, ������� ����� ���������� ��� ����������
                }
            }
        }

        // ����� ���������� ��������� �� ������������� �� ���� ������ ���������
        private void Print(string message)
        {
            var users = JsonSerializer.Deserialize<List<string>>(message) 
            ?? throw new ArgumentNullException(nameof(message));

            listOfUsers.Items.Clear();
            foreach (var user in users)
                listOfUsers.Items.Add(user);
            
        }

        async Task EnterUserAsync(StreamWriter writer, string userName)
        {
            // ������� ���������� ���
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            //Console.WriteLine("��� �������� ��������� ������� ��������� � ������� Enter");

            //while (true)
            //{
            //    string? message = Console.ReadLine();
            //    await writer.WriteLineAsync(message);
            //    await writer.FlushAsync();
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _reader?.Close();
            _writer?.Close();
            _client?.Close();
        }
    }
}