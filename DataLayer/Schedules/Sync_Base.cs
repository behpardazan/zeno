using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public static class Sync_Base
    {
        public static bool Is_Product_Syncing = false;
        public static bool Is_Ladder_Syncing = false;

        public static bool Is_ProductType_Syncing = false;
        public static bool Is_ProductBrand_Syncing = false;
        public static bool Is_ProductCategory_Syncing = false;
        public static bool Is_ProductSubCategory_Syncing = false;
        public static bool Is_ProductCustomField_Syncing = false;
        public static bool Is_Sms_Syncing = false;
        public static bool Is_AccountBasket_Syncing = false;
        public static bool Is_Telegram_Syncing = false;
        public static bool Is_ShrinkDatabase_Syncing = false;
        public static bool Is_Alexa_Syncing = false;
        public static bool Is_Email_Syncing = false;
        public static bool Is_GoldPrice_Syncing = false;
        public static bool Is_PaymentSubscription = false;

        public static bool Is_Discount_Syncing = false;

    }
}
