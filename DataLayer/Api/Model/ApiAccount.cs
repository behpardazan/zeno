using DataLayer.Base;
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
    public class ApiAccount : ApiResponse
    {
        public static ApiResult Post_RegisterVerify(UnitOfWork _context, string mobile, string token, string deviceId = "")
        {
            mobile = mobile.ToEnglishDigit();
            Account account = _context.Account.GetByMobile(mobile);
            if (account == null)
            {
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);
            }
            else
            {
                if (account.VerifyToken == token)
                {
                    account.Verify = true;
                  
                    _context.Account.Update(account);
                    _context.Save();
                    ViewAccount entity = account.ToApi();
                    //Boolean iswebsiteSetting = DataLayer.Base.BaseWebsite.WebsiteSetting.IsLoginWithNationalCode;
                    //if (iswebsiteSetting)
                    //{
                    _context.AccountBasket.MergeAccountBasket(account.ID);
                    _context.AccountBasket.DeleteNotAvailableItems(_context, account.ID);

                    //}
                    AccountLog log = _context.AccountLog.NewLog(account.ID, true);
                    _context.AccountLog.Insert(log);
                    _context.Save();
                    entity.Key = log.AccountKey;
                    return CreateSuccessResult(Enum_Message.SUCCESSFULL_API, entity);
                }
                else
                {
                    return CreateErrorResult(Enum_Message.INVALID_Code_Mobile);
                }
            }

        }
        public static ApiResult Post_RegisterVerify(UnitOfWork _context, string mobile, bool createNew, string password)
        {
            mobile = mobile.ToEnglishDigit();
            Account temp = _context.Account.GetByMobile(mobile);
            Guid uniqueId = temp != null ? temp.UniqueId : new Guid();
            password = password.ToEnglishDigit();
            string first_password = BaseSecurity.HashMd5(password);
            string second_password = BaseSecurity.HashPassword(uniqueId, password);

            Account user = _context.Account.GetFromUsernameAndPassword(temp, first_password, second_password);

            if (user == null)
            {
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);
            }
            else
            {
                Random generator = new Random();
                String verifyToken = generator.Next(0, 1000000000).ToString("D8");
                user.VerifyToken = verifyToken;
                user.Verify = false;
                StringBuilder str = new StringBuilder();
                ViewWebsite website = BaseWebsite.ShopWebsite;
                string token1 = verifyToken;
                string token2 = "";
                string token3 = "";
                str.AppendLine("کد تایید شما در سایت (" + website.Domain + ")");
                str.AppendLine(token1);
                _context.Sms.SaveNewSms(user.Mobile, Enum_SmsType.ACCOUNT_SendCodeVerify, str.ToString(), token1, token2, token3);
                _context.Account.Update(user);
                _context.Save();
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_REGISTER_SendCode);

            }

        }

        public static ApiResult Post_RegisterVerify2(UnitOfWork _context, string mobile, bool createNew, string password, bool isCustom)
        {
            mobile = mobile.ToEnglishDigit();
            Account temp = _context.Account.GetByMobile(mobile);

            if (temp == null)
            {
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);
            }
            else
            {
                Random generator = new Random();
                String verifyToken = generator.Next(0, 1000000000).ToString("D8");
                temp.VerifyToken = verifyToken;
                if (isCustom != true)
                {
                    temp.Verify = false;
                }
                StringBuilder str = new StringBuilder();
                ViewWebsite website = BaseWebsite.ShopWebsite;
                string token1 = verifyToken;
                string token2 = "";
                string token3 = "";
                str.AppendLine("کد تایید شما در سایت (" + website.Domain + ")");
                str.AppendLine(token1);
                _context.Sms.SaveNewSms(temp.Mobile, Enum_SmsType.ACCOUNT_SendCodeVerify, str.ToString(), token1, token2, token3);
                _context.Account.Update(temp);
                _context.Save();
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_REGISTER_SendCode);

            }

        }
        public static ApiResult Post_DoLogin(UnitOfWork _context, Account account, string password, bool isWeb, bool returnLog = true, AccountAddress address = null)
        {
            account.Mobile = account.Mobile != null ? account.Mobile.ToEnglishDigit() : account.Mobile;
            password = password != null ? password.ToEnglishDigit() : password;

            Account temp = _context.Account.GetByEmail(account.Email);
            Guid uniqueId = temp != null ? temp.UniqueId : new Guid();
            password = password.ToEnglishDigit();
            string first_password = BaseSecurity.HashMd5(password);
            string second_password = BaseSecurity.HashPassword(uniqueId, password);

            Account user = _context.Account.GetFromUsernameAndPassword(account, first_password, second_password);
            if (user == null)
            {
                //if (1 == 1)
                //{

                //    string url = "http://lms.psbf.ir/webservice/rest/server.php";
                //    List<ApiHeaderParameter> header = new List<ApiHeaderParameter>() {
                //                    new ApiHeaderParameter() { Name="String-Token",Value=""},
                //                     new ApiHeaderParameter() { Name="Ip-Token",Value=""},
                //    };
                //    ApiResult result = ApiRequest.CreateApiRequest<ViewAccount>(url, Enum_RequestMethod.POST, Enum_Api.PRODUCTCATEGORY, headerList:header);
                //    ViewAccount res = (ViewAccount)result.Value;
                //    return CreateErrorResult(Enum_Message.INVALID_USERNAME_PASSWORD);

                //}
                //else
                //{

                    return CreateErrorResult(Enum_Message.INVALID_USERNAME_PASSWORD);
                //}
               
            }
                
            else if (user.Deleted)
            {
                return CreateErrorResult(Enum_Message.INVALID_USERNAME_PASSWORD);
            }
            else if (user.Verify == false && (user.VerifyToken!="" || user.VerifyToken!=null))
            {
                return CreateErrorResult(Enum_Message.INVALID_Code_Mobile);
            }
            else if (returnLog)
            {
                if (isWeb)
                {
                    _context.AccountBasket.MergeAccountBasket(user.ID);
                    _context.AccountBasket.DeleteNotAvailableItems(_context, user.ID);

                }
                AccountLog log = _context.AccountLog.NewLog(user.ID, isWeb);
                _context.AccountLog.Insert(log);
                _context.Save();

                ViewAccount entity = user.ToApi(address);
                entity.Key = log.AccountKey;
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_LOGIN, entity);
            }
            else
            {
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_LOGIN, user.ID);
            }
        }
        public static ApiResult Post_DoRegister2(UnitOfWork _context, Account account, bool isWeb, string ConfirmPassword, string bothEmailMobile = "false", Boolean iswebsiteSetting = false)
        {
            if (bothEmailMobile == "null")
            {
                bothEmailMobile = "false";
            }
            if (account != null)
            {
                account.Mobile = string.IsNullOrEmpty(account.Mobile) == false ? account.Mobile.Trim().ToEnglishDigit().ToString() : account.Mobile;
                account.Email = string.IsNullOrEmpty(account.Email) == false ? account.Email.Trim().GetEnglish().ToString() : account.Email;
            }

            if (account.Mobile == null && BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE))
            {
                account.Mobile = account.Mobile;
                // account.Email = null;
            }

            if ((account.Email == null || account.Email == "") && BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL))
            {
                account.Email = account.Email;
                // account.Mobile = null;
            }

            Account reagentUser = _context.Account.GetFromReagentCode(account.ReagentCode);
            account.Mobile = account.Mobile.ToEnglishDigit();
            if (account == null)
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT);

            //if (account.Email == null || account.Email == "")
            //    return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            else if (string.IsNullOrEmpty(account.FullName))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);

            else if (!Convert.ToBoolean(bothEmailMobile) && (string.IsNullOrEmpty(account.Email) && string.IsNullOrEmpty(account.Mobile)))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL_OR_MOBILE);

            else if (Convert.ToBoolean(bothEmailMobile) && (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Mobile)))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL_AND_MOBILE);
            if (iswebsiteSetting)
            {
                if (string.IsNullOrEmpty(account.NationalCode))
                {
                    return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_NationalCode);
                }
                else if (BaseSecurity.IsValidInput(account.NationalCode, Enum_Validation.NationalCode) == false)
                    return CreateErrorResult(Enum_Message.INVALID_REAGENT_NationalCode);
            }

            else if (BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);
            else if (string.IsNullOrEmpty(account.Mobile) == false && _context.Account.GetByMobile(account.Mobile) != null)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_MOBILE);
            else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_EMAIL);
            else if (string.IsNullOrEmpty(account.Email) == false && _context.Account.GetByEmail(account.Email) != null)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
            else if (account.ReagentCode != null && string.IsNullOrEmpty(account.ReagentCode.Trim()) == false && reagentUser == null)
                return CreateErrorResult(Enum_Message.INVALID_REAGENT_CODE);
            else if (string.IsNullOrEmpty(account.Password))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_PASSWORD);
            else if (account.Password != ConfirmPassword)
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (BaseSecurity.IsValidInput(account.Password, Enum_Validation.PASSWORD) == false)
                return CreateErrorResult(Enum_Message.Validate_ACCOUNT_PASSWORD);

            account.ReagentId = reagentUser != null ? reagentUser.ID : default(int?);
            account.ReagentCode = _context.Account.GenerateNewReagentCode();
            account.Password = BaseSecurity.HashMd5(account.Password);
            account.UpdateDatetime = DateTime.Now;
            account.CreateDatetime = DateTime.Now;
            account.CountryId = 118;
            account.Verify = false;
            Random generator = new Random();
            String verifyToken = generator.Next(0, 1000000000).ToString("D8");
            account.VerifyToken = verifyToken;
            account.UniqueId = Guid.NewGuid();
            _context.Account.Insert(account);
            _context.Save();

            ViewAccount entity = account.ToApi();
            AccountLog log = _context.AccountLog.NewLog(account.ID, isWeb);
            _context.AccountLog.Insert(log);
            _context.Save();
            entity.Key = log.AccountKey;
            if (isWeb)
            {
                _context.AccountBasket.MergeAccountBasket(account.ID);
                _context.AccountBasket.DeleteNotAvailableItems(_context, account.ID);

            }
            StringBuilder str = new StringBuilder();
            ViewWebsite website = BaseWebsite.ShopWebsite;
            string token1 = account.Mobile;
            string token2 = ConfirmPassword;
            string token3 = verifyToken;
            str.AppendLine("ثبت نام شما در سایت (" + website.Domain + ") با موفقیت انجام شد");
            str.AppendLine("نام کاربری: " + token1);
            str.AppendLine("رمز عبور: " + token2);
            str.AppendLine("کد تایید: " + token3);
            _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.ACCOUNT_REGISTER, str.ToString(), token1, token2, token3);
            //بعد خرید باید کامنت بشه
            //_context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.ACCOUNT_REGISTER4, str.ToString(), token1, token2, token3);
            ////_context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.ACCOUNT_REGISTER, str.ToString(), token1, token2, token3);
            _context.Email.SaveNewEmail(account.Email, Enum_EmailType.ACCOUNT_REGISTER, str.ToString(), "ثبت نام در سایت");
            _context.Save();

            return CreateSuccessResult(Enum_Message.SUCCESSFULL_REGISTER, entity);
        }

        public static ApiResult Post_DoRegister(UnitOfWork _context, Account account, bool isWeb, string ConfirmPassword, string bothEmailMobile = "false", Boolean iswebsiteSetting = false)
        {
            if (bothEmailMobile == "null")
            {
                bothEmailMobile = "false";
            }
           
            if (account != null)
            {
                account.Mobile = string.IsNullOrEmpty(account.Mobile) == false ? account.Mobile.Trim().GetEnglish().ToString() : account.Mobile;
                account.Email = string.IsNullOrEmpty(account.Email) == false ? account.Email.Trim().GetEnglish().ToString() : account.Email;
            }

            if (account.Mobile == null && BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE))
            {
                account.Mobile = account.Mobile;
                // account.Email = null;
            }

            if ((account.Email == null || account.Email == "") && BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL))
            {
                account.Email = account.Email;
                // account.Mobile = null;
            }
            account.Mobile = account.Mobile.ToEnglishDigit();
            Account reagentUser = _context.Account.GetFromReagentCode(account.ReagentCode);
            
            if (account == null)
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT);

            //if (account.Email == null || account.Email == "")
            //    return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            else if (string.IsNullOrEmpty(account.FullName))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);

            else if (!Convert.ToBoolean(bothEmailMobile) && (string.IsNullOrEmpty(account.Email) && string.IsNullOrEmpty(account.Mobile)))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL_OR_MOBILE);

            else if (Convert.ToBoolean(bothEmailMobile) && (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Mobile)))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL_AND_MOBILE);
            if (iswebsiteSetting)
            {
                if (string.IsNullOrEmpty(account.NationalCode))
                {
                    return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_NationalCode);
                }
                else if (BaseSecurity.IsValidInput(account.NationalCode, Enum_Validation.NationalCode) == false)
                    return CreateErrorResult(Enum_Message.INVALID_REAGENT_NationalCode);
            }

            else if (BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);
            else if (string.IsNullOrEmpty(account.Mobile) == false && _context.Account.GetByMobile(account.Mobile) != null)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_MOBILE);
            else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_EMAIL);
            else if (string.IsNullOrEmpty(account.Email) == false && _context.Account.GetByEmail(account.Email) != null)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
            else if (account.ReagentCode != null && string.IsNullOrEmpty(account.ReagentCode.Trim()) == false && reagentUser == null)
                return CreateErrorResult(Enum_Message.INVALID_REAGENT_CODE);
            else if (string.IsNullOrEmpty(account.Password))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_PASSWORD);
            else if (account.Password != ConfirmPassword)
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (BaseSecurity.IsValidInput(account.Password, Enum_Validation.PASSWORD) == false)
                return CreateErrorResult(Enum_Message.Validate_ACCOUNT_PASSWORD);
            account.Password = account.Password.ToEnglishDigit();
            account.Mobile = account.Mobile.ToEnglishDigit();

            account.ReagentId = reagentUser != null ? reagentUser.ID : default(int?);
            account.ReagentCode = _context.Account.GenerateNewReagentCode();
            account.Password = BaseSecurity.HashMd5(account.Password);
            account.UpdateDatetime = DateTime.Now;
            account.CreateDatetime = DateTime.Now;
            account.CountryId = 118;
            account.UniqueId = Guid.NewGuid();
            _context.Account.Insert(account);
            _context.Save();

            ViewAccount entity = account.ToApi();
            AccountLog log = _context.AccountLog.NewLog(account.ID, isWeb);
            _context.AccountLog.Insert(log);
            _context.Save();
            entity.Key = log.AccountKey;
            if (isWeb)
            {
                _context.AccountBasket.MergeAccountBasket(account.ID);
                _context.AccountBasket.DeleteNotAvailableItems(_context, account.ID);

            }
            StringBuilder str = new StringBuilder();
            ViewWebsite website = BaseWebsite.ShopWebsite;
            string token1 = account.Mobile;
            string token2 = ConfirmPassword;
            string token3 = "";
            str.AppendLine("ثبت نام شما در سایت (" + website.Domain + ") با موفقیت انجام شد");
            str.AppendLine("نام کاربری: " + token1);
            str.AppendLine("رمز عبور: " + token2);
            _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.ACCOUNT_REGISTER, str.ToString(), token1, token2, token3);
            _context.Email.SaveNewEmail(account.Email, Enum_EmailType.ACCOUNT_REGISTER, str.ToString(), "ثبت نام در سایت");
            _context.Save();

            return CreateSuccessResult(Enum_Message.SUCCESSFULL_REGISTER, entity);
        }

        public static ApiResult Get_Account(UnitOfWork _context, int accountId)
        {
            Account account = _context.Account.GetById(accountId);
            return CreateSuccessResult(account.ToApi());
        }

        public static ApiResult Put_Account(UnitOfWork _context, ViewAccount account, int accountId)
        {
            if (account.Email != null && BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
            {
                return CreateErrorResult(Enum_Message.INVALID_EMAIL_FORMAT);
            }
            else if (account.CardNumber != null && account.CardNumber.Trim() != string.Empty)
            {
                if (account.CardNumber.Trim().Length != 16)
                    return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_CardNumber);
                if (BaseSecurity.IsValidInput(account.CardNumber, Enum_Validation.TEXT))
                    return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_CardNumber);
                if (BaseSecurity.IsValidInput(account.CardNumber, Enum_Validation.PERSIAN))
                    return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_CardNumber);
            }
            else if (account.NationalCode != null &&  account.NationalCode.Trim() != string.Empty)
            {
                if(BaseSecurity.IsValidInput(account.NationalCode, Enum_Validation.NationalCode) == false)
                    return CreateErrorResult(Enum_Message.INVALID_REAGENT_NationalCode);
            }
             
            Account entity = _context.Account.GetById(accountId);
            entity.PictureId = account.PictureID;
            entity.Address = account.Address;
            entity.IsOffice = account.IsOffice;

            entity.FatherName = account.FatherName;

            entity.Agent = account.Agent;
            entity.AgentPhone = account.AgentPhone;
            entity.Company = account.Company;
            entity.CompanyNo = account.CompanyNo;
            entity.Email = account.Email;
            entity.FullName = account.FullName;
            entity.IsMale = account.IsMale;
            entity.PictureId = account.PictureID;
            //entity.PictureUrl = account.Picture != null ? account.Picture.GetUrl() : "";
            entity.Job = account.Job;
            entity.NationalCode = account.NationalCode;
            entity.Phone = account.Phone;
            entity.ReagentName = account.ReagentName;
            entity.BirthDay = DataLayer.Base.BaseDate.GetGregorian(account.FaBirthDay);
            entity.Description = account.Description;
            entity.PictureId = account.PictureID;
            entity.Sheba = account.Sheba;
            entity.CardNumber = account.CardNumber;
            entity.StateId = account.StateId != null ? account.StateId : 0;
            entity.CityId = account.CityId != null ? account.CityId : 0;
            entity.CountryId = account.CountryId != null ? account.CountryId : 0;

            _context.Account.Update(entity);
            _context.Save();

            Account tempEntity = _context.Account.GetById(accountId);
            return CreateSuccessResult(tempEntity.ToApi());
        }

        public static ApiResult Put_ChangePassword(UnitOfWork _context, int accountId, string oldPassword, string newPassword, string confirmPassword)
        {
            Account entity = _context.Account.GetById(accountId);

            if (string.IsNullOrEmpty(oldPassword))
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD_PREVIOUS);
            else if (string.IsNullOrEmpty(newPassword))
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD);
            else if (string.IsNullOrEmpty(confirmPassword))
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            else if (newPassword != confirmPassword)
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (BaseSecurity.IsValidInput(newPassword, Enum_Validation.PASSWORD) == false)
                return CreateErrorResult(Enum_Message.Validate_ACCOUNT_PASSWORD);
            else if (entity.Password != BaseSecurity.HashMd5(oldPassword))
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_PREVIOUS);

            entity.Password = BaseSecurity.HashMd5(newPassword);
            _context.Save();

            return CreateSuccessResult(entity.ToApi());
        }

        public static ApiResult Put_ChangeUserName(UnitOfWork _context, int accountId, string username)
        {
            //Account entity = _context.Account.GetById(accountId);

            if (string.IsNullOrEmpty(username))
                return CreateErrorResult(Enum_Message.REQUIRED_USERNAME);
            username = username.Trim().Replace(" ", "");
            if (BaseSecurity.IsValidInput(username, Enum_Validation.USERNAME) == false)
                return CreateErrorResult(Enum_Message.INVALID_USERNAME);
            else if (_context.Account.ChangeUserName(accountId, username) == false)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_USERNAME);
            return CreateSuccessResult();
        }
    }
}
