using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegramBot1
{
    internal class Program
    {
        static TelegramBotClient bot = new("6208863954:AAEfO1MazHbA6aNXKo8Lh9vW66Vz-eX4ReQ");
        static void Main(string[] args)
        {
            bot.StartReceiving<UpdateHandler>();
            Console.WriteLine("start");
            Console.ReadKey();
        }
    }

    class UpdateHandler : IUpdateHandler
    {

        List<Users> clients = new();
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Parallel.Invoke(() =>
            {
                Task.Run(() => AddUser(botClient, update));
            });
        }
        public async Task AddUser(ITelegramBotClient botClient, Update update)
        {
            try
            {
                if (clients.Exists(x => x.ChatId == update.Message.Chat.Id))
                {
                    await Handle(botClient, update);
                }
                else if (update.Message.Text.Equals("/AddUser", StringComparison.OrdinalIgnoreCase))
                {
                    clients.Add(new Users() { ChatId = update.Message.Chat.Id });
                    await Handle(botClient, update);
                }
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Hi ");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            async Task Handle(ITelegramBotClient botClient, Update update)
            {
                Users client = clients.First(x => x.ChatId == update.Message.Chat.Id);

                long _chatId = update.Message.Chat.Id;

                if (client.a == 0)
                {
                    client.a = 3;
                    await botClient.SendTextMessageAsync(_chatId, "Ismni kiriting: ");
                }
                else if (client.a == 3)
                {
                    client.a--;
                    client.user.Name = update.Message.Text;
                    await botClient.SendTextMessageAsync(_chatId, "Emailni kiriting:");
                }
                else if (client.a == 2)
                {
                    client.a = 1;
                    client.user.Email = update.Message.Text;

                    await botClient.SendTextMessageAsync(_chatId, "Yoshni kiriting:");

                }
                else if (client.a == 1)
                {
                    client.user.Age = update.Message.Text;
                    clients.Remove(client);
                    await botClient.SendTextMessageAsync(_chatId, "Barcha ma`lumotlar kiritildi!\n" + client.user);

                }
            }
        }


    }
    record User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
    }
    class Users
    {
        public long ChatId { get; set; } = 0;
        public byte a { get; set; } = 0;
        public User user { get; set; } = new User();
    }
}