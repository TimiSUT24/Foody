using Application.Klarna.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klarna.Interfaces
{
    public interface IKlarnaService
    {
        Task<KlarnaSessionResponse> CreatePaymentSession(Domain.Models.Order order);
    }
}
