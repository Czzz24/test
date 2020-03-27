using LumiSoft.Net.AUTH;
using LumiSoft.Net.Mail;
using LumiSoft.Net.Mime;
using LumiSoft.Net.MIME;
using LumiSoft.Net.SMTP.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    public class LumisoftHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Send(Tools.EmailModel model, string subject, string mailBody, Dictionary<string, string> toList, Dictionary<string, string> ccList, Dictionary<string, byte[]> mailAttachment)
        {
            bool sended = false;
            using (SMTP_Client client = new SMTP_Client())
            {
                client.Connect(model.SmtpServer, model.ServerPort, model.SSL);
                client.EhloHelo(model.SmtpServer);
                var authhh = new AUTH_SASL_Client_Plain(model.FromAddress, model.EmailPwd);
                client.Auth(authhh);
                //client.Authenticate(username, password);
                //string text = client.GreetingText;
                client.MailFrom(model.FromAddress, -1);
                foreach (string address in toList.Keys)
                {
                    client.RcptTo(address);
                }

                //采用Mail_Message类型的Stream
                Mail_Message m = Create_PlainText_Html_Attachment_Image(toList, ccList, model.FromAddress, model.FromDisplayName, subject, mailBody, mailAttachment);
                using (MemoryStream stream = new MemoryStream())
                {
                    m.ToStream(stream, new MIME_Encoding_EncodedWord(MIME_EncodedWordEncoding.Q, Encoding.UTF8), Encoding.UTF8);
                    stream.Position = 0;
                    client.SendMessage(stream);

                    sended = true;
                }
                if (m != null)
                {
                    m.Dispose();
                }

                client.Disconnect();
            }
            return sended;
        }

        /// <summary>
        /// 构造Mail_Message格式的邮件操作
        /// </summary>
        /// <param name="tomails"></param>
        /// <param name="ccmails"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailFromDisplay"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <param name="notifyEmail"></param>
        /// <param name="plaintTextTips"></param>
        /// <returns></returns>
        private Mail_Message Create_PlainText_Html_Attachment_Image(Dictionary<string, string> tomails, Dictionary<string, string> ccmails, string mailFrom, string mailFromDisplay,
         string subject, string body, Dictionary<string, byte[]> attachments, string notifyEmail = "", string plaintTextTips = "")
        {
            Mail_Message msg = new Mail_Message();
            msg.MimeVersion = "1.0";
            msg.MessageID = MIME_Utils.CreateMessageID();
            msg.Date = DateTime.Now;
            msg.Subject = subject;
            msg.From = new Mail_t_MailboxList();
            msg.From.Add(new Mail_t_Mailbox(mailFromDisplay, mailFrom));
            msg.To = new Mail_t_AddressList();
            foreach (string address in tomails.Keys)
            {
                string displayName = tomails[address];
                msg.To.Add(new Mail_t_Mailbox(displayName, address));
            }
            if (ccmails != null && ccmails.Count > 0)
            {
                msg.Cc = new Mail_t_AddressList();
                foreach (string address in ccmails.Keys)
                {
                    string displayName = ccmails[address];
                    msg.Cc.Add(new Mail_t_Mailbox(displayName, address));
                }
            }

            //设置回执通知
            if (!string.IsNullOrEmpty(notifyEmail) && ValidateUtil.IsEmail(notifyEmail))
            {
                msg.DispositionNotificationTo.Add(new Mail_t_Mailbox(notifyEmail, notifyEmail));
            }

            #region MyRegion

            //--- multipart/mixed -----------------------------------
            MIME_h_ContentType contentType_multipartMixed = new MIME_h_ContentType(MIME_MediaTypes.Multipart.mixed);
            contentType_multipartMixed.Param_Boundary = Guid.NewGuid().ToString().Replace('-', '.');
            MIME_b_MultipartMixed multipartMixed = new MIME_b_MultipartMixed(contentType_multipartMixed);
            msg.Body = multipartMixed;

            //--- multipart/alternative -----------------------------
            MIME_Entity entity_multipartAlternative = new MIME_Entity();
            MIME_h_ContentType contentType_multipartAlternative = new MIME_h_ContentType(MIME_MediaTypes.Multipart.alternative);
            contentType_multipartAlternative.Param_Boundary = Guid.NewGuid().ToString().Replace('-', '.');
            MIME_b_MultipartAlternative multipartAlternative = new MIME_b_MultipartAlternative(contentType_multipartAlternative);
            entity_multipartAlternative.Body = multipartAlternative;
            multipartMixed.BodyParts.Add(entity_multipartAlternative);

            //--- text/plain ----------------------------------------
            MIME_Entity entity_text_plain = new MIME_Entity();
            MIME_b_Text text_plain = new MIME_b_Text(MIME_MediaTypes.Text.plain);
            entity_text_plain.Body = text_plain;

            //普通文本邮件内容，如果对方的收件客户端不支持HTML，这是必需的
            string plainTextBody = "如果你邮件客户端不支持HTML格式，或者你切换到“普通文本”视图，将看到此内容";
            if (!string.IsNullOrEmpty(plaintTextTips))
            {
                plainTextBody = plaintTextTips;
            }

            text_plain.SetText(MIME_TransferEncodings.QuotedPrintable, Encoding.UTF8, plainTextBody);
            multipartAlternative.BodyParts.Add(entity_text_plain);

            //--- text/html -----------------------------------------
            string htmlText = body;//"<html>这是一份测试邮件，<img src=\"cid:test.jpg\">来自<font color=red><b>LumiSoft.Net</b></font></html>";
            MIME_Entity entity_text_html = new MIME_Entity();
            MIME_b_Text text_html = new MIME_b_Text(MIME_MediaTypes.Text.html);
            entity_text_html.Body = text_html;
            text_html.SetText(MIME_TransferEncodings.QuotedPrintable, Encoding.UTF8, htmlText);
            multipartAlternative.BodyParts.Add(entity_text_html);

            //--- application/octet-stream -------------------------
            WebClient client = new WebClient();
            if (attachments != null)
                foreach (string attach in attachments.Keys)
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream(attachments[attach]))
                        {
                            multipartMixed.BodyParts.Add(Mail_Message.CreateAttachment(stream, attach));
                        }
                    }
                    catch (Exception ex)
                    {
                        //LogTextHelper.Error(ex);
                    }
                }

            #endregion

            return msg;
        }

        /// <summary>
        /// 构造Mime格式的操作
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailFromDisplay"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <param name="embedImages"></param>
        /// <param name="notifyEmail"></param>
        /// <param name="plaintTextTips"></param>
        /// <param name="replyEmail"></param>
        /// <returns></returns>
        private Mime Create_Html_Attachment_Image(string mailTo, string mailFrom, string mailFromDisplay,
            string subject, string body, List<string> attachments, Dictionary<string, string> embedImages, string notifyEmail = "", string plaintTextTips = "",
            string replyEmail = "")
        {
            Mime m = new Mime();
            MimeEntity mainEntity = m.MainEntity;

            mainEntity.From = new AddressList();
            mainEntity.From.Add(new MailboxAddress(mailFromDisplay, mailFrom));
            mainEntity.To = new AddressList();
            mainEntity.To.Add(new MailboxAddress(mailTo, mailTo));
            mainEntity.Subject = subject;
            mainEntity.ContentType = MediaType_enum.Multipart_mixed;

            //设置回执通知
            if (!string.IsNullOrEmpty(notifyEmail) && ValidateUtil.IsEmail(notifyEmail))
            {
                mainEntity.DSN = notifyEmail;
            }

            //设置统一回复地址
            if (!string.IsNullOrEmpty(replyEmail) && ValidateUtil.IsEmail(replyEmail))
            {
                mainEntity.ReplyTo = new AddressList();
                mainEntity.ReplyTo.Add(new MailboxAddress(replyEmail, replyEmail));
            }

            MimeEntity textEntity = mainEntity.ChildEntities.Add();
            textEntity.ContentType = MediaType_enum.Text_html;
            textEntity.ContentTransferEncoding = ContentTransferEncoding_enum.QuotedPrintable;
            textEntity.DataText = body;

            //附件
            foreach (string attach in attachments)
            {
                MimeEntity attachmentEntity = mainEntity.ChildEntities.Add();
                attachmentEntity.ContentType = MediaType_enum.Application_octet_stream;
                attachmentEntity.ContentDisposition = ContentDisposition_enum.Attachment;
                attachmentEntity.ContentTransferEncoding = ContentTransferEncoding_enum.Base64;
                FileInfo file = new FileInfo(attach);
                attachmentEntity.ContentDisposition_FileName = file.Name;
                attachmentEntity.DataFromFile(attach);
            }

            //嵌入图片
            foreach (string key in embedImages.Keys)
            {
                MimeEntity attachmentEntity = mainEntity.ChildEntities.Add();
                attachmentEntity.ContentType = MediaType_enum.Application_octet_stream;
                attachmentEntity.ContentDisposition = ContentDisposition_enum.Inline;
                attachmentEntity.ContentTransferEncoding = ContentTransferEncoding_enum.Base64;
                string imageFile = embedImages[key];
                FileInfo file = new FileInfo(imageFile);
                attachmentEntity.ContentDisposition_FileName = file.Name;

                //string displayName = Path.GetFileNameWithoutExtension(fileName);
                attachmentEntity.ContentID = key;//BytesTools.BytesToHex(Encoding.Default.GetBytes(fileName));

                attachmentEntity.DataFromFile(imageFile);
            }

            return m;
        }
    }

    public class EmailModel
    {
        private String _SmtpServer;
        private int _ServerPort = 25;
        private String _FromAddress;
        private String _FromDisplayName;
        private String _EmailPwd;
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
            get { return _SmtpServer; }
            set { _SmtpServer = value; }
        }

        /// <summary>
        /// 用于SMTP事务的端口
        /// </summary>
        public int ServerPort
        {
            get { return _ServerPort; }
            set { _ServerPort = value; }
        }

        /// <summary>
        /// 发件人
        /// </summary>
        public String FromAddress
        {
            get { return _FromAddress; }
            set { _FromAddress = value; }
        }
        public String FromDisplayName
        {
            get { return _FromDisplayName; }
            set { _FromDisplayName = value; }
        }

        /// <summary>
        /// Email密码
        /// </summary>
        public String EmailPwd
        {
            get { return _EmailPwd; }
            set { _EmailPwd = value; }
        }
    }
}
