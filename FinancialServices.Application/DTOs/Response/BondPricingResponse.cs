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

        public decimal Duration { get; set; }   

        public decimal PresentValue { get; set; }

        public void TruncatePrices()
        {
           foreach(var cashFlow in CashFlowPayments)
            {
                cashFlow.PresentValue = Math.Truncate(cashFlow.PresentValue * 100) / 100;
                cashFlow.FutureValue = Math.Truncate(cashFlow.FutureValue * 100) / 100;
            }

            PresentValue = Math.Truncate(PresentValue * 100) / 100;
        }

        public void UpdatePricesByQuantityFactor(decimal quantity)
        {
            foreach(var cashflow in CashFlowPayments)
            {
                cashflow.PresentValue *= quantity;
                cashflow.FutureValue *= quantity;
            }

            PresentValue *= quantity;
        }

    }
}
