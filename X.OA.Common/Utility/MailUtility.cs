using System;
using System.Collections.Generic;
using System.Configuration;
using static System.Configuration.ConfigurationManager;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mail;

namespace X.OA.Common.Utility
{
    public static class MailUtility
    {
        /// <summary>
        /// Send Mail
        /// Example Config:
        /// add key = "mailFrom" value="X@X.com"/>
        /// add key = "mailPassword" value="sdhufuenx"/>
        /// add key = "mailSmtpServer" value="smtp.qq.com"/>
        /// </summary>
        /// <param name="messageData">A MailMessage instance that contains To,Subject,Body</param>
        public static void SendMail(MailMessage messageData)
        {
            MailMessage message = new MailMessage();
            message.To = messageData.To;
            message.From = AppSettings["mailFrom"];
            message.Subject = messageData.Subject;
            message.Body = messageData.Body;
            message.BodyFormat = messageData.BodyFormat;
            message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", message.From);
            message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", AppSettings["mailPassword"]);
            message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//SMTP 服务器要求安全连接需要设置此属性

            SmtpMail.SmtpServer = AppSettings["mailSmtpServer"];
            SmtpMail.Send(message);
        }
    }
}
