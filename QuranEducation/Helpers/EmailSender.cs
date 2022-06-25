using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Mailjet.Client.TransactionalEmails;
using System.Net.Mail;
using System.Net;

namespace QuranEducation.Helpers
{
    public class EmailSender
    {

        public bool SendMail(string receiver, string message)//EmailModel model)

        {

            try

            {
                sendEmailAsync(receiver, message);
                return true;
                //using (MailMessage mm = new MailMessage("itqanplatform2020@gmail.com", receiver))//model.receiver))

                //{

                //    mm.Subject = QuranRes.SiteTitle;
                //    mm.Body = message;

                //    //mm.IsBodyHtml = true;

                //    using (SmtpClient smtp = new SmtpClient())

                //    {
                //        smtp.Host = "smtp.gmail.com";

                //        smtp.EnableSsl = true;

                //        NetworkCredential NetworkCred = new NetworkCredential("itqanplatform2020@gmail.com", "itqan@2020");

                //        smtp.UseDefaultCredentials = false;

                //        smtp.Credentials = NetworkCred;

                //        smtp.Port = 587;

                //        smtp.Send(mm);

                //        return true;

                //    }

                //}

            }

            catch (Exception e)

            {

                return false;

            }

        }


        void sendEmailAsync(string receiver, string message)
        {
            try
            {
                var apiKey = "348865bd2709a451b73ad13a42834f6a";
                var apiSecret = "25cc13fdc7b4153c0dc85c81a3ec18d5";

                MailjetClient client = new MailjetClient(apiKey, apiSecret)
                {

                };
                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                    .Property(Send.FromEmail, "itqanq@hotmail.com")
                    .Property(Send.FromName, "ITQAN")
                    .Property(Send.Recipients, new JArray {
                 new JObject {
                     {"Email", receiver},
                     {"Name", "Dear Subscriber"}
                 }
                    })
                    .Property(Send.Subject, "Auto Generated Message")
                    //.Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")
                    .Property(Send.HtmlPart, "<h3>" + message + "</h3><br />");
                MailjetResponse response = Task.Run(async () => await client.PostAsync(request)).Result;
                // invoke API to send email
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                    Console.WriteLine(response.GetData());
                }
                else
                {
                    Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                    Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                    Console.WriteLine(response.GetData());
                    Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
                }
            }
            catch (Exception e)
            {


            }

        }


    }
}