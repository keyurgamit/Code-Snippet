using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public class CommonController : Controller
    {

        //private ICommonRepository _commonRepository;

        //public CommonController(ICommonRepository CommonRepository)
        //{
        //    this._commonRepository = CommonRepository;
        //}

        public string EncryptionKey = "E5AFB6E2924A4709A52C";
        public string EncryptString(string clearText)
        {
            //string EncryptionKey = RandomStringGenerator();

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string DecryptString(string cipherText)
        {
            //string EncryptionKey = RandomStringGenerator();

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string RandomStringGenerator(int size = 20)
        {
            return Guid.NewGuid().ToString("n").Substring(0, size).ToUpper();
        }

        public bool SendMail(string emailId, string emailSubject, string emailBody)
        {
            SmtpSection MailConfig = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                using (MailMessage mm = new MailMessage(MailConfig.From, emailId))
                {
                    mm.Subject = emailSubject;
                    mm.Body = emailBody;
                    mm.IsBodyHtml = true;

                    using (SmtpClient objSmtpClient = new SmtpClient())
                    {
                        objSmtpClient.Host = MailConfig.Network.Host;
                        objSmtpClient.Port = MailConfig.Network.Port;
                        objSmtpClient.EnableSsl = MailConfig.Network.EnableSsl;
                        objSmtpClient.DeliveryMethod = MailConfig.DeliveryMethod;
                        objSmtpClient.UseDefaultCredentials = false;
                        objSmtpClient.Credentials = new NetworkCredential(MailConfig.Network.UserName, MailConfig.Network.Password);

                        objSmtpClient.Send(mm);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        public string GetBrowserDetails()
        {
            string browserDetails = string.Empty;

            System.Web.HttpBrowserCapabilities browser = System.Web.HttpContext.Current.Request.Browser;
            browserDetails = "Name = " + browser.Browser + "," + "Type = " + browser.Type + "," + "Version = " + browser.Version;
            //+ ","
            //+ "Major Version = " + browser.MajorVersion + ","
            //+ "Minor Version = " + browser.MinorVersion + ","
            //+ "Platform = " + browser.Platform + ","
            //+ "Is Beta = " + browser.Beta + ","
            //+ "Is Crawler = " + browser.Crawler + ","
            //+ "Is AOL = " + browser.AOL + ","
            //+ "Is Win16 = " + browser.Win16 + ","
            //+ "Is Win32 = " + browser.Win32 + ","
            //+ "Supports Frames = " + browser.Frames + ","
            //+ "Supports Tables = " + browser.Tables + ","
            //+ "Supports Cookies = " + browser.Cookies + ","
            //+ "Supports VBScript = " + browser.VBScript + ","
            //+ "Supports JavaScript = " + "," +
            //browser.EcmaScriptVersion.ToString() + ","
            //+ "Supports Java Applets = " + browser.JavaApplets + ","
            //+ "Supports ActiveX Controls = " + browser.ActiveXControls
            //+ ","
            //+ "Supports JavaScript Version = " +
            //browser["JavaScriptVersion"];

            return browserDetails;
        }
    }
}