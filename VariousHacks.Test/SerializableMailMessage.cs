using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VariousHacks.Tests
{
    public class SerializableMailMessage : MailMessage, ISerializable
    {
        public SerializableMailMessage() : base()
        {
        }
        
        public SerializableMailMessage(SerializationInfo info, StreamingContext context) : base()
        {
            this.From = new MailAddress(info.GetString("sender"), info.GetString("senderName"));
            this.Subject = info.GetString("subject");
            this.Body = info.GetString("body");

            Action<SerializationInfo, string> addRecipient = (serializationInfo, data) =>
            {
                var recipients = info.GetString(data);
                if (string.IsNullOrEmpty(recipients))
                    return;

                recipients.Split(new string[] { ";" }, StringSplitOptions.None)
                .Select(recipientAddress => new MailAddress(recipientAddress))
                .ToList()
                .ForEach(recipient =>
                {
                    if (data == "to")
                        this.To.Add(recipient);
                    else if (data == "cc")
                        this.CC.Add(recipient);
                    else if (data == "bcc")
                        this.Bcc.Add(recipient);
                });
            };

            addRecipient(info, "to");
            addRecipient(info, "cc");
            addRecipient(info, "bcc");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("sender", this.From.Address);
            info.AddValue("senderName", this.From.DisplayName);
            info.AddValue("to", string.Join(";", this.To.Select(recipient => recipient.Address)));
            info.AddValue("cc", string.Join(";", this.CC.Select(cc => cc.Address)));
            info.AddValue("bcc", string.Join(";", this.Bcc.Select(bcc => bcc.Address)));
            info.AddValue("subject", this.Subject);
            info.AddValue("body", this.Body);
        }
    }
}
