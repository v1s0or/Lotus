using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    public class Bot
    {
        public readonly DiscordSocketClient _client;
        private string _botToken;

        // Constructor for Bot, which asks for the token if not provided
        public Bot(string botToken = "")
        {
            _botToken = botToken;
            _client = new DiscordSocketClient();
            _client.Ready += OnReady;
        }

        // Event that triggers when the bot is ready
        public async Task OnReady()
        {
            Console.Title = "Lotus - Bot (Online)";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[+] Bot is online");
            Console.ResetColor();

            // List the guilds (servers) the bot is in
            Console.WriteLine("\nThe bot is connected to the following servers:");
            var guilds = _client.Guilds.ToList();
            for (int i = 0; i < guilds.Count; i++)
            {
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n{i + 1}. {guilds[i].Name} (ID: {guilds[i].Id})");
                Console.ResetColor();
            }

            // Ask the user to choose a server
            Console.Write("\nSelect a server by number: ");
            int serverChoice;
            if (!int.TryParse(Console.ReadLine(), out serverChoice) || serverChoice < 1 || serverChoice > guilds.Count)
            {
                Console.WriteLine("Invalid choice!");
                return;
            }

            var selectedGuild = guilds[serverChoice - 1];

            // List the channels in the selected guild
            Console.WriteLine($"\nChannels in {selectedGuild.Name}:");
            var channels = selectedGuild.Channels.OfType<SocketTextChannel>().ToList();
            for (int i = 0; i < channels.Count; i++)
            {
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n{i + 1}. {channels[i].Name} (ID: {channels[i].Id})");
                Console.ResetColor();
            }

            // Ask the user to choose a channel
            Console.Write("\nSelect a channel by number: \n");
            int channelChoice;
            if (!int.TryParse(Console.ReadLine(), out channelChoice) || channelChoice < 1 || channelChoice > channels.Count)
            {
                Console.WriteLine("\nInvalid choice!");
                return;
            }

            var selectedChannel = channels[channelChoice - 1];

            // Ask the user if they want to delete all other channels and leave only the selected channel open
            Console.WriteLine("\nDo you want to delete all other channels and leave only the selected channel open? (y/n)");
            string deleteChannelsConfirmation = Console.ReadLine().ToLower();

            if (deleteChannelsConfirmation == "y")
            {
                // Delete all other channels except the selected one
                foreach (var channel in channels)
                {
                    if (channel.Id != selectedChannel.Id)
                    {
                        try
                        {
                            await channel.DeleteAsync();
                            Console.WriteLine($"Channel {channel.Name} deleted.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to delete channel {channel.Name}: {ex.Message}");
                        }
                    }
                }
            }

            // Ask the user for the message to repeat
            Console.WriteLine("\nEnter the message you want to repeat in the selected channel: ");
            string repeatMessage = Console.ReadLine();

            // Ask the user how many times they want to send the message
            Console.WriteLine("\nHow many times do you want to send the message?");
            int repeatCount;
            while (!int.TryParse(Console.ReadLine(), out repeatCount) || repeatCount <= 0)
            {
                Console.WriteLine("Please enter a valid positive number for repetitions.");
            }

            // Start sending the message to the selected channel the specified number of times
            await StartMessageLoop(selectedChannel, repeatMessage, repeatCount);
        }

        // Method to repeatedly send messages to the selected channel a specified number of times
        private async Task StartMessageLoop(ITextChannel channel, string repeatMessage, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                await channel.SendMessageAsync(repeatMessage);
                Console.WriteLine($"Message {i + 1}/{repeatCount} sent.");
                await Task.Delay(500);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nMessage sending complete.");
            Console.ResetColor();
        }

        // Start the bot
        public async Task RunBotAsync()
        {
            Console.Title = "Lotus - Bot";

            try
            {
                // If the bot token is empty, ask the user to input it
                if (string.IsNullOrEmpty(_botToken))
                {
                    Console.Clear();
                    Console.WriteLine("Enter your Bot Token: ");
                    _botToken = Console.ReadLine();
                }

                if (string.IsNullOrEmpty(_botToken))
                {
                    Console.WriteLine("Bot token cannot be empty. Please try again.");
                    return;
                }

                Console.Clear();
                Console.WriteLine("Attempting to log in...");

                // Attempt to log in using the provided bot token
                await _client.LoginAsync(TokenType.Bot, _botToken);
                await _client.StartAsync();

                // Block the program until it is closed
                await Task.Delay(-1);
            }
            catch (UnauthorizedAccessException)
            {
                // Handle the 401 Unauthorized error
                Console.WriteLine("Error: Unauthorized. Please check your bot token.");
            }
            catch (Discord.Net.HttpException ex)
            {
                // Handle login issues
                Console.WriteLine("Error: Login failed.");
                Console.WriteLine($"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
