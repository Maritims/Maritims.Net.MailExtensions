using Maritims.Net.MailExtensions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Mail;

namespace Maritims.Net.MailExtensions.Tests
{
    public class LetsSerialize
    {
        [Test]
        [TestCase("sender@example.com", "Jane Doe", "recipient@example.com", "John Doe", "A serializable message", "This message has been serialized and deserialized.")]
        public void SerializableMailMessage(string sender, string senderName, string recipient, string recipientName, string subject, string body)
        {
            var mailMessage = new SerializableMailMessage
            {
                From = new MailAddress(sender, senderName)
            };
            mailMessage.To.Add(new MailAddress(recipient, recipientName));
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            var serializedData = JsonConvert.SerializeObject(mailMessage);
            var deserializedData = JsonConvert.DeserializeObject<SerializableMailMessage>(serializedData);

            Assert.AreEqual(sender, deserializedData.From.Address);
            Assert.AreEqual(senderName, deserializedData.From.DisplayName);
            Assert.AreEqual(recipient, deserializedData.To[0].Address);
            Assert.AreEqual(subject, deserializedData.Subject);
            Assert.AreEqual(body, deserializedData.Body);
        }
    }
}
