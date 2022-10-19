using Xunit.Abstractions;
using Xunit;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Codesanook.Examples.DotNet.Datetime
{
    public class DiscordTest
    {
        private readonly ITestOutputHelper output;
        public DiscordTest(ITestOutputHelper output) => this.output = output;

        [Fact]
        public async Task Test()
        {
            const string token = @"";
            var client = new DiscordSocketClient();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            output.WriteLine($"login status {client.LoginState}");
            await client.SetStatusAsync(UserStatus.Online);

            ulong id = 0;
            var channel = await client.GetChannelAsync(id) as IMessageChannel;
            await channel.SendFileAsync(
                @"https://static.mercdn.net/item/detail/orig/photos/m27349562532_1.jpg?1665316641",
                 $" SONY NT1 (3,000JPY) https://jp.mercari.com/item/m27349562532"); // 5

        }
    }
}
