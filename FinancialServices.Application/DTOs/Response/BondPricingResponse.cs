using PublicBonds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.DTOs.Response
{
    public class BondPricingResponse
    {
        public List<CashFlowPayment> CashFlowPayments { get; set; }

        public double Duration { get; set; }   

        public double PresentValue { get; set; }
    }
}
