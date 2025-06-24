using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Enum
{
    public enum OrderStatus
    {
        
        Pending = 1,
        AcceptedByDeliveryCompany,
        RejectedByDeliveryCompany,
        Delivered,
        DeliveredToDeliveryMan,
        CanNotBeReached,
        Postponed,
        PartiallyDelivered,
        CanceledByCustomer,
        RejectWithPayment,
        RejectWithoutPayment,
        RejectWithPartiallyPaid
    }
}
