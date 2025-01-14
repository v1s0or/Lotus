using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lotus
{
    public class Webhook
    {
        private string _webhookUrl;

        public Webhook(string webhookUrl)
        {
            _webhookUrl = webhookUrl;
        }

        // Non-static method
        public async Task Send(string message)
        {
            using (HttpClient client = new HttpClient())
            {
                // Create the JSON payload with the message
                var payload = new StringContent(
                    "{\"content\":\"" + message + "\"}",  // Discord expects the message in the "content" field
                    Encoding.UTF8,
                    "application/json"
                );

                // Sending the POST request to Discord Webhook URL
                HttpResponseMessage response = await client.PostAsync(_webhookUrl, payload);

                // Handling 
                if (response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[+] Sent!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[-] Failed to send. (if its 429 then you might want to chill with the amount of messages pls) Error: " + response.StatusCode);
                }
            }
        }
        public async Task RunWebhook()
        {
            Console.Title = "Lotus - Webhook";
            Console.Clear();
            Console.WriteLine("Enter the Discord webhook URL:");
            string webhookUrl = Console.ReadLine();

            // Create an instance of Webhook with the provided URL
            Webhook webhook = new Webhook(webhookUrl);

            Console.WriteLine("How many times will this message be sent?");
            int times = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the message to send:");
            string message = Console.ReadLine();

            Console.WriteLine("[+] Sending message...");

            // Send the message the specified number of times
            for (int i = 0; i < times; i++)
            {
                await webhook.Send(message);
            }

            Console.WriteLine("[+] Sent!");
        }
    }
}