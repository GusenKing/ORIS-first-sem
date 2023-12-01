using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using Microsoft.Maui.Platform;

namespace MauiPointGame;

public partial class MainPage : ContentPage
{
    private int count = 0;
    private List<string> listOfUsers = new() {"player1, player2"};
    private TcpClient _client;

    public MainPage()
    {
        InitializeComponent();
        UsersList.ItemsSource = listOfUsers;
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        StartupLayout.IsVisible = false;
        MainLayout.IsVisible = true;
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
                    await Task.Run(() => Print(message));
                }
                catch (IOException)
                {
                    // Исключение возникает, если считывание из закрытого потока
                    // может произойти, если сервер отключил клиента
                    await DisplayAlert("Ошибка", "Сервер отключил клиента.", "Ок");
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

            listOfUsers.Clear();
            foreach (var user in users)
                listOfUsers.Add(user);
            
        }

        async Task EnterUserAsync(StreamWriter writer, string userName)
        {
            // сначала отправляем имя
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
        }
}