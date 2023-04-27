using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegramBot1
{
    internal class Program
    {
        static TelegramBotClient bot = new("6268728289:AAGRIW0vItlqE15orLtlo2wKsnqBM20aBeI");
        static void Main(string[] args)
        {
            bot.StartReceiving<UpdateHandler>();
            Console.WriteLine("start");
            Console.ReadKey();
        }
    }

    class UpdateHandler : IUpdateHandler
    {
        int a = 0;
        long chatId = 0;
        readonly User user = new();
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            //  await botClient.SendTextMessageAsync(botClient.BotId, "Bu error:" + exception.Message);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            long _chatId = update.Message.Chat.Id;
            await Console.Out.WriteLineAsync("start1");
            Console.Out.WriteLineAsync(update.Message.Text+"ChatId:"+_chatId+"Name:"+update.Message.Chat.Username+ "Bio:" + update.Message.Chat.Bio);

            if (chatId == 0 && a == 0 & update.Message.Text == "/AddUser")
            {
                a = 3;
                chatId = _chatId;
                await botClient.SendTextMessageAsync(_chatId, "Ismni kiriting: "+ _chatId);
            }
            else if (chatId == _chatId && a == 3)
            {
                a--;
                user.Name = update.Message.Text;
                await botClient.SendTextMessageAsync(_chatId, "Emailni kiriting:");
            }
            else if (chatId == _chatId && a == 2)
            {
                a = 1;
                user.Email = update.Message.Text;

                await botClient.SendTextMessageAsync(_chatId, "Yoshni kiriting:");

            }
            else if (chatId == _chatId && a == 1)
            {
                user.Age = update.Message.Text;
                a = 0;
                chatId = 0;
                await botClient.SendTextMessageAsync(_chatId, "Barcha ma`lumotlar kiritildi!\n"+user);

            }
           else await botClient.SendTextMessageAsync(_chatId, "Hi "+_chatId);

            //await botClient.SendTextMessageAsync(update.Message.Chat.Username, "salom");

        }
        record User
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Age { get; set; }
        }


    }
}