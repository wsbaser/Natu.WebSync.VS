using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using automateit.utils;
using ActiveUp.Net.Mail;
using NUnit.Framework;

namespace selenium.core.Auxiliary {
    public class MailClient {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useSsl;

        private Imap4Client _client;
        private CancellationTokenSource _cancel;

        public MailClient(string host, int port, bool useSsl) {
            _host = host;
            _port = port;
            _useSsl = useSsl;
        }

        public void DeleteAll() {
            var mailbox = _client.SelectMailbox("inbox");
            for (int i = mailbox.MessageCount; i > 0; i--)
                mailbox.DeleteMessage(i, true);
        }

        public void Connect(string email, string password) {
            _client = new Imap4Client();
            if (_useSsl)
                _client.ConnectSsl(_host, _port);
            else
                _client.Connect(_host, _port);
            _client.Login(email, password);
        }


        public string GetLetterBody(string titlePattern) {
            return GetLetter(titlePattern)?.BodyHtml.Text;
        }

        /// <summary>
        /// Найти первое письмо, у которого заголовок соответствует указанному паттерну
        /// </summary>
        public Message GetLetter(string titlePattern) {
            foreach (Message message in GetAllMails("inbox"))
                if (RegexHelper.IsMatch(message.Subject, titlePattern))
                    return message;
            return null;
        }

        public IEnumerable<Message> GetAllMails(string mailBox) {
            return GetMails(mailBox, "ALL").Cast<Message>();
        }

        public IEnumerable<Message> GetUnreadMails(string mailBox) {
            return GetMails(mailBox, "UNSEEN").Cast<Message>();
        }

        private MessageCollection GetMails(string mailBox, string searchPhrase) {
            Mailbox mails = _client.SelectMailbox(mailBox);
            MessageCollection messages = mails.SearchParse(searchPhrase);
            return messages;
        }
    }

    [TestFixture]
    public class TestMailClient {
        [Test]
        public void DeleteMesages() {
            var gMailClient = GMailClient.Connect("quantumart.selenium@gmail.com", "ViolettaLebedeva85");
            gMailClient.DeleteAll();
        }
    }

}