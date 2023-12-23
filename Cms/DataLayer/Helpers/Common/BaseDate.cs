using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DataLayer.Base
{
    public static class BaseDate
    {
        //میلادی به شمسی 
        public static DateTime? GetGregorian(this string date)
        {
            if (string.IsNullOrEmpty(date) == false)
            {
                CustomDate temp = new CustomDate(date);
                return GetGregorian(temp);
            }
            else
                return null;
        }
        private static Method GetMethod(Enum_RequestMethod method)
        {
            Method sharpMethod = Method.GET;
            switch (method)
            {
                case Enum_RequestMethod.GET:
                    sharpMethod = Method.GET;
                    break;
                case Enum_RequestMethod.POST:
                    sharpMethod = Method.POST;
                    break;
                case Enum_RequestMethod.PUT:
                    sharpMethod = Method.PUT;
                    break;
                case Enum_RequestMethod.DELETE:
                    sharpMethod = Method.DELETE;
                    break;
                default:
                    break;
            }
            return sharpMethod;
        }
        public class DataObject
        {
            public string Name { get; set; }
        }
        public static bool IsHoliday(this string date)
        {
           
            string webAddr = "https://pholiday.herokuapp.com/date/" + date;
            var result = "-1";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
         
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                var model = JsonConvert.DeserializeObject<ViewHoliday>(result);
                if (model.isHoliday == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //Method sharpMethod = GetMethod(Enum_RequestMethod.GET);
            //var client = new RestClient("https://pholiday.herokuapp.com/date/" + date);
            //var request = new RestRequest("NON", sharpMethod);

            //IRestResponse response = client.Execute(request);
            //var test = response.Content;
            //ViewHoliday result = JsonConvert.DeserializeObject<ViewHoliday>(response.Content);

        }

        public static DateTime? GetGregorian(this DateTime? date)
        {
            if (date != null)
            {
                CustomDate temp = new CustomDate(date.Value);
                return BaseDate.GetGregorian(temp);
            }
            else
                return null;
        }

        public static DateTime GetGregorian(this DateTime date)
        {
            CustomDate temp = new CustomDate(date);
            return BaseDate.GetGregorian(temp).GetValueOrDefault();
        }

        public static DateTime? GetGregorian(CustomDate date)
        {


            if (date.Year == 0)
                return null;
            if (date.Year < 1500)
                return Persia.Calendar.ConvertToGregorian(date.Year, date.Month, date.Day, Persia.DateType.Persian);
            else
                return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static string GetTimeString(this TimeSpan time)
        {
            string output = time.Hours.ToPersian();
            if (time.Minutes != 0)
                output += ":" + time.Minutes.ToPersian();
            return output;
        }


        public static string GetDateString(this DateTime datetime)
        {
            return datetime.Year + "-" + datetime.Month + "-" + datetime.Day + " " + datetime.Hour + ":" + datetime.Minute + ":" + datetime.Second;
        }
        public static string GetDateStringOnly(this DateTime datetime)
        {
            return datetime.Year + "-" + datetime.Month + "-" + datetime.Day;
        }
        public static string GetDateStringOnly(this DateTime ? datetime)
        {
            if (datetime == null)
                return "1880-01-01";
            else
            {
                DateTime dt = datetime.Value;
                return dt.Year + "-" + dt.Month + "-" + dt.Day;
            }
        }
        public static string GetDateStringByTime(this DateTime datetime)
        {
            string month = datetime.Month < 10 ? "0" + datetime.Month.ToString() : datetime.Month.ToString();
            string day = datetime.Day < 10 ? "0" + datetime.Day.ToString() : datetime.Day.ToString();

            return datetime.Year + "-" + month + "-" + day+"T" + +datetime.Hour + ":" + datetime.Minute + ":" + datetime.Second+"+01:00";
        }
        public static string GetDateStringByTime(this DateTime? datetime)
        {
            if (datetime == null)
                return "1880-01-01";
            else
            {
                DateTime dt = datetime.Value;
                string month = dt.Month < 10 ? "0" + dt.Month.ToString() : dt.Month.ToString();
                string day = dt.Day < 10 ? "0" + dt.Day.ToString() : dt.Day.ToString();
                return dt.Year + "-" + month + "-" + day + "T" + +dt.Hour + ":" + dt.Minute + ":" + dt.Second + "+01:00";
            }
        }
        public static string GetDateString(this DateTime? datetime)
        {
            if (datetime == null)
                return "1880-01-01";
            else
            {
                DateTime dt = datetime.Value;
                return dt.GetDateString();
            }
        }

        public static string GetPersianMonth(int MonthNumber)
        {
            switch (MonthNumber)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }
    }

    public class CustomDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public CustomDate() { }
        public CustomDate(DateTime date)
        {
            if (date.IsPersian() == false)
            {
                string temp = date.ToPersian();
                temp = Persia.Number.ConvertToLatin(temp);
                string[] split = temp.Split('/');
                this.Year = split[0].GetInteger();
                this.Month = split[1].GetInteger();
                this.Day = split[2].GetInteger();
            }
            else
            {
                this.Year = date.Year;
                this.Month = date.Month;
                this.Day = date.Day;
            }
        }

        public CustomDate(string date)
        {
            date = Persia.Number.ConvertToLatin(date);
            string[] dateSplit = date.Split('/');
            if (dateSplit.Count() > 2)
            {
                int year = dateSplit[0].GetInteger();
                if (year < 1500)
                {
                    string temp = Persia.Number.ConvertToLatin(date);
                    string[] split = temp.Split('/');
                    this.Year = split[0].GetInteger();
                    this.Month = split[1].GetInteger();
                    this.Day = split[2].GetInteger();
                }
                else
                {
                    this.Year = date.Split('/')[0].GetInteger();
                    this.Month = date.Split('/')[1].GetInteger();
                    this.Day = date.Split('/')[2].GetInteger();
                }
            }
        }
    }
}
