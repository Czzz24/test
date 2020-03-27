using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace EF.Component.Tools
{
    public class EmailHelper
    {
        private String _SmtpServer;
        private FileUpload _MailAttachment;
        private int _ServerPort = 25;
        private MailAddress _FromName;
        private String _EmailPwd;
        private MailAddress _ToName;
        private String _Subject;
        private String _MailBody;
        private Boolean _MailBodyHtml = true;
        private bool _SSL = false;

        /// <summary>
        /// 是否使用SSL
        /// </summary>
        public bool SSL
        {
            get { return _SSL; }
            set { _SSL = value; }
        }

        /// <summary>
        /// SMTM服务器
        /// </summary>
        public String SmtpServer
        {
            get
            {
                return _SmtpServer;
            }
            set
            {
                _SmtpServer = value;
            }
        }

        /// <summary>
        /// 用于SMTP事务的端口
        /// </summary>
        public int ServerPort
        {
            get
            {
                return _ServerPort;
            }
            set
            {
                _ServerPort = value;
            }
        }

        /// <summary>
        /// 发件人
        /// </summary>
        public MailAddress FromName
        {
            get
            {
                return _FromName;
            }
            set
            {
                _FromName = value;
            }
        }

        /// <summary>
        /// Email密码
        /// </summary>
        public String EmailPwd
        {
            get
            {
                return _EmailPwd;
            }
            set
            {
                _EmailPwd = value;
            }
        }

        /// <summary>
        /// 收件人
        /// </summary>
        public MailAddress ToName
        {
            get
            {
                return _ToName;
            }
            set
            {
                _ToName = value;
            }
        }

        /// <summary>
        /// 主题
        /// </summary>
        public String Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                _Subject = value;
            }
        }

        /// <summary>
        /// 邮件正文
        /// </summary>
        public String MailBody
        {
            get
            {
                return _MailBody;
            }
            set
            {
                _MailBody = value;
            }
        }

        /// <summary>
        /// 指示邮件正文是否为 Html 格式的值
        /// </summary>
        public Boolean MailBodyHtml
        {
            get
            {
                return _MailBodyHtml;
            }
            set
            {
                _MailBodyHtml = value;
            }
        }

        /// <summary>
        /// 上传的文件
        /// </summary>
        public FileUpload MailAttachment
        {
            get
            {
                return _MailAttachment;
            }
            set
            {
                _MailAttachment = value;
            }
        }

        public void SendToEMail()
        {
            MailMessage MM = new MailMessage();
            MM.From = FromName;
            MM.To.Add(ToName);
            MM.Subject = Subject;

            MM.Body = MailBody;
            MM.Priority = MailPriority.High;
            MM.IsBodyHtml = MailBodyHtml;
            MM.BodyEncoding = System.Text.Encoding.UTF8;

            if (MailAttachment != null)
                MM.Attachments.Add(new Attachment(MailAttachment.PostedFile.InputStream, MailAttachment.FileName));


            SmtpClient SC = new SmtpClient(SmtpServer, ServerPort);
            SC.DeliveryMethod = SmtpDeliveryMethod.Network;
            SC.Timeout = 100000;
            SC.EnableSsl = SSL;
            SC.Credentials = new System.Net.NetworkCredential(FromName.Address, EmailPwd);
            SC.Send(MM);
        }

        /// <summary>
        /// 异步发送邮件
        /// </summary>
        public void AsySendToEmail()
        {
            Action AsySend = new Action(SendToEMail);
            AsySend.BeginInvoke(null, null);
        }

        //MailMessage属性
        //
        //AlternateViews  获取用于存储邮件正文的替代形式的附件集合。  
        //Attachments  获取用于存储附加到此电子邮件的数据的附件集合。  
        //Bcc  获取包含此电子邮件的密件抄送 (BCC) 收件人的地址集合。  
        //Body  获取或设置邮件正文。  
        //BodyEncoding  获取或设置用于邮件正文的编码。  
        //CC  获取包含此电子邮件的抄送 (CC) 收件人的地址集合。  
        //DeliveryNotificationOptions  获取或设置此电子邮件的发送通知。  
        //From  获取或设置此电子邮件的发信人地址。  
        //Headers  获取与此电子邮件一起传输的电子邮件标头。  
        //IsBodyHtml  获取或设置指示邮件正文是否为 Html 格式的值。  
        //Priority  获取或设置此电子邮件的优先级。  
        //ReplyTo  获取或设置邮件的回复地址。  
        //Sender  获取或设置此电子邮件的发件人地址。  
        //Subject  获取或设置此电子邮件的主题行。  
        //SubjectEncoding  获取或设置此电子邮件的主题内容使用的编码。  
        //To  获取包含此电子邮件的收件人的地址集合。  

    }
}
