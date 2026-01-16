using Application.Postnord.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Postnord.Interfaces
{
    public interface IPostnordService
    {
        Task<object> BookShipmentAsync(PostNordBookingRequestDto dto, CancellationToken ct);
        Task<object> GetDeliveryOptionsAsync(string postCode);
        Task<ValidationPostalCode> ValidatePostalCode(PostalCodeRequest dtorequest);
    }
}
