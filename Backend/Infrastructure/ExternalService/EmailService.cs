using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalService
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"] ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        }

        public async Task SendOrderConfirmationEmail(string toEmail, Order order)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("xmotherx500@gmail.com", "Foody");
            var subject = $"Order Confirmation #{order.Id}";
            var to = new EmailAddress(toEmail);
            var htmlContent = $@"
            <h2>Thank you for your order!</h2>
            <p>Order ID: {order.Id}</p>
            <p>Total: {order.TotalPrice:C}</p>
            <h3>Items:</h3>
            <ul>
                {string.Join("", order.OrderItems.Select(i => $"<li>{i.Food.Name} x{i.Quantity} - {i.UnitPrice:C}</li>"))}
            </ul>
        ";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                // Optional: log errors
                throw new Exception($"SendGrid failed: {response.StatusCode}");
            }
        }
    }
}
