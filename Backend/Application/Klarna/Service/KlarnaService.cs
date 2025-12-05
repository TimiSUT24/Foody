using Application.Klarna.Dto.Request;
using Application.Klarna.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Text;
using System.Text.Json;

public class KlarnaService : IKlarnaService
{
    private readonly IConfiguration _config;

    public KlarnaService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<KlarnaSessionResponse> CreatePaymentSession(Order order)
    {
        var client = new RestClient($"{_config["Klarna:ApiUrl"]}/payments/v1/sessions");
        var request = new RestRequest();
        request.AddHeader("Content-Type", "application/json");

        var auth = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{_config["Klarna:Username"]}:{_config["Klarna:Password"]}")
        );
        request.AddHeader("Authorization", $"Basic {auth}");

        // Map order lines to Klarna format
        const int taxRate = 2500; // 25% VAT
        var orderLines = order.OrderItems.Select(i =>
        {
            int unitPrice = (int)(i.UnitPrice * 100);           // price per item in öre
            int totalAmount = unitPrice * i.Quantity;          // quantity * price
            int totalTax = (int)Math.Floor((double)(totalAmount * taxRate) / (10000 + taxRate));

            return new
            {
                type = "physical",
                name = i.Food.Name,
                quantity = i.Quantity,
                unit_price = unitPrice,
                total_amount = totalAmount,
                tax_rate = taxRate,
                total_tax_amount = totalTax
            };
        }).ToList();

        int totalOrderTax = orderLines.Sum(x => x.total_tax_amount);

        // Build Klarna session request body
        var klarnaRequest = new
        {
            purchase_country = "SE",
            purchase_currency = "SEK",
            locale = "sv-SE",
            order_amount = (int)(order.TotalPrice * 100),
            order_tax_amount = totalOrderTax,
            order_lines = orderLines,
            merchant_urls = new
            {
                confirmation = $"{_config["Klarna:Merchant:ConfirmationUrl"]}/{order.Id}",
                push = $"{_config["Klarna:Merchant:PushUrl"]}/{order.Id}",
                terms = _config["Klarna:Merchant:TermsUrl"]
            }
        };

        request.AddJsonBody(klarnaRequest);

        var response = await client.ExecutePostAsync(request);
        if (!response.IsSuccessful)
            throw new Exception($"Klarna error: {response.Content}");

        var json = JsonDocument.Parse(response.Content);

        var dto = new KlarnaSessionResponse
        {
            ClientToken = json.RootElement.GetProperty("client_token").GetString()!,
            SessionId = json.RootElement.GetProperty("session_id").GetString()!,
            PaymentMethodCategories = json.RootElement.GetProperty("payment_method_categories"),
            Payload = klarnaRequest
        };
        return dto;

    }
}
