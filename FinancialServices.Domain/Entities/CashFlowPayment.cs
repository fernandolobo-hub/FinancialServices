using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Entities
{
    public class CashFlowPayment : Entity
    {
        public DateTime Date { get; set; }

        public int BusinessDays { get; set; }

        /// <summary>
        /// Present value. Indicates the amount that the payment is currently worth.
        /// </summary>
        public double PresentValue { get; set; }

        /// <summary>
        /// Future Value. Indicates the amount to be paid when the payment is due.
        /// </summary>
        public double FutureValue { get; set; }

        /// <summary>
        /// Indicates if is a coupon payment or if the payment is the principal when the bond matures.
        /// </summary>
        public bool Coupon { get; set; }
    }

}
