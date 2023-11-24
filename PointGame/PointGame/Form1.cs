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

                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(_reader));

                // отправляем имя пользователя
                await EnterUserAsync(_writer, userName);

                // обновляем интерфейс
                label1.Text = userName;
                label1.Visible = false;
                enterName.Visible = false;
                btn_signIn.Visible = false;
                listOfUsers.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }

        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string message = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(message)) continue;

                    // обновляем интерфейс с использованием Invoke, так как это происходит в отдельном потоке
                    Invoke((MethodInvoker)delegate
                    {
                        Print(message);
                    });
                }
                catch (IOException)
                {
                    // Исключение возникает, если считывание из закрытого потока
                    // может произойти, если сервер отключил клиента
                    MessageBox.Show("Сервер отключил клиента.");
                    break;
                }
                catch (Exception)
                {
                    // Любые другие исключения, которые могут возникнуть при считывании
                }
            }
        }

        // чтобы полученное сообщение не накладывалось на ввод нового сообщения
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
            // сначала отправляем имя
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            //Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

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