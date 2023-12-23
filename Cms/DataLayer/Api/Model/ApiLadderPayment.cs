using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiLadderPayment : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, int? accountId, int? siteuserId, ViewLadderPayment payment,bool ?isMobile=false)
        {
            
            payment.Status = payment.Status == null ?
            payment.Status = new ViewCode()
            {
                Label = Enum_Code.PAYMENT_STATUS_INSERTED.ToString()
            } : payment.Status;

            LadderPayment paymentObj = _context.LadderPayment.CreatePayment(accountId,  payment.Price, payment.Merchant.Id,payment.LadderSettingId, payment.Description, payment.Status.Label, productId: payment.ProductId,isMobile:isMobile);
           
            ViewLadderPayment output = paymentObj.ToApi();
            return output != null ? CreateSuccessResult(output) : CreateErrorResult(Enum_Message.INVALID_PAYMENT_OBJECT);
        }


    }
}
