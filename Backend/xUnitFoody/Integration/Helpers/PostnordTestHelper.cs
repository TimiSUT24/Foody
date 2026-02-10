using Application.Order.Dto.Request;
using Application.Postnord.Dto;
using Application.Stripe.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitFoody.Integration.Helpers
{
    public static class PostnordTestHelper
    {
        public static ShippingInformation ValidFalkenberg => new ShippingInformation
        {
            Adress = "TestGatan 2",
            State = "Halland",
            City = "Falkenberg",
            Email = "timpan200@gmail.com",
            FirstName = "Tim",
            LastName = "Petersen",
            PhoneNumber = "121343324423",
            PostalCode = "31173",
        };

        public static StripeShippingDto2 ValidStripeFalkenberg => new StripeShippingDto2
        {
            Adress = "TestGatan 2",
            ServiceCode = "17",
            State = "Halland",
            City = "Falkenberg",
            Email = "timpan200@gmail.com",
            FirstName = "Tim",
            LastName = "Petersen",
            PhoneNumber = "121343324423",
            PostalCode = "31173",
            DeliveryOptionId = ""
        };

    }
}
