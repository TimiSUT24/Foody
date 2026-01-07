using Application.Postnord.Dto;
using Application.Postnord.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Postnord.Service
{
    public class PostnordService : IPostnordService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public PostnordService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Postnord:ApiKey"]!;
        }

        public async Task<object> GetDeliveryOptionsAsync(string postCode)
        {
            var payload = new
            {
                warehouses = new[]
                {
                new
                {
                    id = "Falkenberg",
                    address = new
                    {
                        postCode = "31175",
                        street = "Sandgatan 34",
                        city = "Falkenberg",
                        countryCode = "SE"
                    },
                    orderHandling = new
                    {
                        daysUntilOrderIsReady = "0-2"
                    }
                }
            },
                customer = new { customerKey = "example_request_customer_key" },
                recipient = new
                {
                    address = new
                    {
                        postCode,
                        countryCode = "SE"
                    }
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://atapi2.postnord.com/rest/shipment/v1/deliveryoptions/bywarehouse?apikey={_apiKey}"
            );

            request.Headers.Add("Accept-Language", "sv");
            request.Content = JsonContent.Create(payload);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            var filtered = json
                .GetProperty("warehouseToDeliveryOptions")
                .EnumerateArray()
                .Select(wh => new
                {
                    warehouse = wh.GetProperty("warehouse"),
                    deliveryOptions = wh.GetProperty("deliveryOptions")
                        .EnumerateArray()
                        .Where(o => o.GetProperty("type").GetString() == "home")
                })
                .Where(x => x.deliveryOptions.Any());

            return filtered;
        }

        public async Task<object> BookShipmentAsync(PostNordBookingRequestDto dto,CancellationToken ct)
        {
            var address = dto.Shipping.Shipping.Address;
            var payload = new
            {
                messageDate = DateTime.UtcNow,
                messageFunction = "Instruction",
                messageId = $"order-{dto.OrderId}".Substring(0, 30),
                application = new
                {
                    applicationId = 9999,
                    name = "Foody",
                    version = "1.0"
                },
                updateIndicator = "Original",
                shipment = new[]
                {
                    new
                    {
                        shipmentIdentification = new
                        {
                            shipmentId = dto.OrderId.Substring(0, 30)
                        },
                        dateAndTimes = new
                        {
                            loadingDate = DateTime.UtcNow
                        },
                        service = new
                        {
                            basicServiceCode = dto.Shipping.ServiceCode,
                            additionalServiceCode = Array.Empty<string>()
                        },
                        numberOfPackages = new { value = 1 },
                        totalGrossWeight = new
                        {
                            value = dto.TotalWeight,
                            unit = "KGM"
                        },
                        parties = new
                        {
                            consignor = new
                            {
                                issuerCode = "Z12",
                                partyIdentification = new
                                {
                                    partyId = "1111111111",
                                    partyIdType = "160"
                                },
                                party = new
                                {
                                    nameIdentification = new { name = "Foody" },
                                    address = new
                                    {
                                        streets = new[] { "Sandgatan 34" },
                                        postalCode = "31175",
                                        city = "Falkenberg",
                                        countryCode = "SE"
                                    }
                                }
                            },
                            consignee = new
                            {
                                party = new
                                {
                                    nameIdentification = new
                                    {
                                        name = $"{dto.Shipping.Shipping.Name} {dto.Shipping.Lastname}"
                                    },
                                    address = new
                                    {
                                        streets = new[] { address.Line1 },
                                        postalCode = address.Postal_Code,
                                        city = address.City,
                                        countryCode = "SE"
                                    },
                                    contact = new
                                    {
                                        contactName = $"{dto.Shipping.Shipping.Name} {dto.Shipping.Lastname}",
                                        emailAddress = dto.Shipping.Email,
                                        smsNo = dto.Shipping.Shipping.Phone
                                    }
                                }
                            }
                        },
                        goodsItem = new[]
                        {
                            new
                            {
                                packageTypeCode = "PC",
                                items = new[]
                                {
                                    new
                                    {
                                        itemIdentification = new
                                        {
                                            itemId = "0",
                                            itemIdType = "SSCC"
                                        },
                                        grossWeight = new
                                        {
                                            value = dto.TotalWeight,
                                            unit = "KGM"
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            };

            var response = await _http.PostAsJsonAsync(
                $"https://atapi2.postnord.com/rest/shipment/v3/edi?apikey={_apiKey}",
                payload
            );

            var body = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"PostNord {response.StatusCode}: {body}"
                );
            }

            return JsonSerializer.Deserialize<object>(body)!;
        }

    }
}
