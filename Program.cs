using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DiscordBot;
using Discord;
using Discord.WebSocket;

namespace Lotus
{
    internal class Program
    {
        // Method to display the banner from the text file
        public static void Banner()
        {
            string filePath = "banner.txt";  // Specify the path to your banner file

            try
            {
                // Try to read the file content
                string content = File.ReadAllText(filePath);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(content);  // Output the banner content
                Console.ResetColor();
            }
            catch (Exception)
            {
            Console.Clear();
            }
        }

        public static async Task Main()
        {
            Console.Title = "Lotus";
            Console.Clear();

            // Call the Banner method to print the text file content or fallback banner
            Banner();

            // Main menu options
            Console.WriteLine($"1. Bot");
            Console.WriteLine($"2. Webhook");
            Console.WriteLine($"3. Nebula");

            // Handle user input for menu selection
            var choice = Console.ReadKey().KeyChar;
            Console.WriteLine();  // Move to the next line after the user's choice

            // Handle the user's choice
            if (choice == '1')
            {
                // Bot section
                Console.Clear();
                Console.WriteLine("Hit enter to enter into the bot section.");
                string botToke = Console.ReadLine();  

                Bot bot = new Bot(botToke);  
                await bot.RunBotAsync();  // Call the Main method in Bot.cs
            }
            else if (choice == '2')  // Webhook section
            {
                // Webhook section
                Console.Clear();
                Console.WriteLine("Hit enter to enter webhook.");
                string webhookUrl = Console.ReadLine();  // Get webhook URL from the user

                // Create an instance of the Webhook class
                Webhook webhook = new Webhook(webhookUrl);
                await webhook.RunWebhook();  // Assuming RunWebhook() sends messages to the webhook
            }
            else if (choice == '3')  // Add more options as needed
            {
                Console.Clear();
                Console.WriteLine("Hit enter to enter Nebula");
                Process.Start("Nebula.exe");
            }

            else
            {
                // Invalid choice
                Console.Clear();
                Console.WriteLine("Invalid choice.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                await Main();  // Start again
            }
        }
    }
}
