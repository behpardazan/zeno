using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Enumarables;
using Persia;
using RestSharp.Extensions;

namespace DataLayer.Base
{
    public static class BasePersian
    {
        public static bool IsPersian(this DateTime datetime)
        {
            if (datetime.Year < 1500)
                return true;
            return false;
        }

        public static string ToPersian(this string str)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return (string.IsNullOrEmpty(str) == false) ? Number.ConvertToPersian(str) : "";
            }
            return str;
        }

        public static string ToPersian(this int input)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return Number.ConvertToPersian(input);
            }
            return input.ToString();
        }

        public static string ToPersian(this int? input)
        {
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return input == null ? 0.ToPersian() : input.Value.ToPersian();
            }
            return input == null ? 0.ToPersian() : input.Value.ToString();
        }

        public static string ToPersianComplete(this DateTime? datetime)
        {
            if (datetime == null)
                return "";
            return datetime.Value.ToPersianComplete();
        }

        public static string ToPersianComplete(this DateTime datetime)
        {
            //میلادی به شمسی 
            var currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                return Calendar.ConvertToPersian(datetime).Weekday;
            }
            return datetime.ToString();
        }
     
        public static string ToPersianWeakDayComplete(this DateTime datetime)
        {
            var temp = datetime.DayOfWeek;
            string day = "";
            switch (temp)
            {
                case DayOfWeek.Sunday:
                    day = "یک شنبه";
                    break;
                case DayOfWeek.Monday:
                    day = "دو شنبه";
                    break;
                case DayOfWeek.Tuesday:
                    day = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    day = "چهار شنبه";
                    break;
                case DayOfWeek.Thursday:
                    day = "پنج شنبه";
                    break;
                case DayOfWeek.Friday:
                    day = "جمعه";
                    break;
                case DayOfWeek.Saturday:
                    day = " شنبه";
                    break;
                default:
                    day = "شنبه";
                    break;
            }
            return day;
        }

        public static string ToPersianWithTime(this DateTime? datetime)
        {
            if (datetime == null)
                return "";
            return datetime.Value.ToPersianWithTime();
        }

        public static string ToPersianWithTime(this DateTime datetime)
        {
            return Calendar.ConvertToPersian(datetime).Simple + " " + datetime.Hour.ToPersian() + ":" + datetime.Minute.ToPersian();
        }
        public static string ToPersianWithjustHourMinets(this DateTime datetime)
        {
            return datetime.Hour.ToPersian() + ":" + datetime.Minute.ToPersian();
        }
        public static string ToPersianWithjustDay(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            return pc.GetDayOfMonth(datetime).ToString();
        }
        public static string ToPersianWithjustYear(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            return pc.GetYear(datetime).ToString();
        }
        public static string ToPersianWithjustMonth(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            return pc.GetMonth(datetime).ToString();
        }
        public static string ToPersianWithjustMonthComplete(this DateTime datetime)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            var temp = pc.GetMonth(datetime);
            string month = "";
            switch (temp)
            {
                case 1:
                    month = "فروردین";
                    break;
                case 2:
                    month = "اردیبهشت";
                    break;
                case 3:
                    month = "خرداد";
                    break;
                case 4:
                    month = "تیر";
                    break;
                case 5:
                    month = "مرداد";
                    break;
                case 6:
                    month = "شهریور";
                    break;
                case 7:
                    month = " مهر";
                    break;
                case 8:
                    month = "آبان";
                    break;
                case 9:
                    month = "آذر";
                    break;
                case 10:
                    month = "دی";
                    break;
                case 11:
                    month = "بهمن";
                    break;
                case 12:
                    month = "اسفند";
                    break;
                default:
                    month = "";
                    break;
            }
            return month;
        }

        public static string ToPersian(this DateTime? datetime)
        {
            if (datetime == null)
                return "";
            return datetime.Value.ToPersian();
        }

        public static string ToPersian(this DateTime datetime)
        {
            if (datetime != default(DateTime))
                return Number.ConvertToLatin(Calendar.ConvertToPersian(datetime).Simple.ToString());
            return "";
        }
        public static string ToDayName(this DateTime datetime)
        {
            double day = (DateTime.Now - datetime).TotalDays;
            int dayint = (int)day;
            if (dayint == 0)
            {
                return " امروز";
            }
            else if (dayint == 1)
            {
                return " دیروز" ;
            }
            
            else if (dayint == 7)
            {
                return  "هفته پیش" ;
            }
            else if (dayint == 30)
            {
                return " ماه پیش" ;
            }
            else if (dayint != 7 && dayint % 7==0)
            {
                return dayint / 7 + " هفته پیش " ;
            }
            else if (dayint != 30 && dayint % 30 == 0)
            {
                return dayint / 30 + " ماه پیش ";
            }
            else
            {
                return dayint + " روز پیش ";
            }
        }
        public static string ToPersianGiv(this DateTime datetime)
        {
            string temp = "";
            if (datetime != default(DateTime))
            {
                temp += Calendar.ConvertToPersian(datetime).ArrayType[0];
                temp += Calendar.ConvertToPersian(datetime).ArrayType[1].ToString("00");
                temp += Calendar.ConvertToPersian(datetime).ArrayType[2].ToString("00");
            }
            return Number.ConvertToLatin(temp);
        }
        public static string ToPersianChar(this string chare)
        {
            if (chare.HasValue())
            {
                string temp = chare.Replace("ي", "ی").Replace("ك", "ک");
                return temp;
            }
            else
            {
                return "";
            }
        }
        public static string GetEnglish(this string number)
        {
            if (number == null)
                return "";
            return Number.ConvertToLatin(number);
        }
        public static string ToJustTime(this DateTime datetime)
        {
            return datetime.Hour.ToPersian() + ":" + datetime.Minute.ToPersian();
        }
    }
}
