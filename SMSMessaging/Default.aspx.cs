using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.UI;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SMSMessaging
{
    public partial class _Default : Page
    {
        public static string senderphNo = ConfigurationManager.AppSettings["SenderPhoneNo"];
        public static string ErrLogfile = ConfigurationManager.AppSettings["ErrLogFilePath"];

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_SendClick(object sender, EventArgs e)
        {
            try
            {
                string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

                string countryCode=Request.Form["comboCountryCode"].ToString();
                string recipientphNo = countryCode + Request.Form["txtRecipientPhNo"].ToString();
                string message = Request.Form["txtMessage"].ToString();

                TwilioClient.Init(accountSid, authToken);

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2

                var sendmessage = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(senderphNo),
                    to: new Twilio.Types.PhoneNumber(recipientphNo)
                );

                Response.Write("<script>alert('Sent Message Successfully');window.location = 'Default.aspx';</script>");
            }
            catch (Exception ex)
            {
                ErrorLogString(ex);
            }
        }

        private static void ErrorLogString(Exception str)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += str;
            message += Environment.NewLine;

            CreateErrorLog(message);
        }

        private static void CreateErrorLog(string message)
        {
            FileInfo fileInfo = new FileInfo(ErrLogfile);
            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            using (StreamWriter strmwriter = new StreamWriter(ErrLogfile, true))
            {
                strmwriter.WriteLine(message);
                strmwriter.Close();
            }
        }
    }
}