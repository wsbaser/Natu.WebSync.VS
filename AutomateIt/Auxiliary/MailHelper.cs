using System.Threading;
using ActiveUp.Net.Mail;
using NUnit.Framework;
using selenium.core.Auxiliary;

namespace automateit.Auxiliary {
    public class MailHelper {
        public static void DeleteMessages(Account emailAccount) {
            DeleteMessages(emailAccount.Login, emailAccount.Password);
        }

        public static void DeleteMessages(string email, string password) {
            var client = GMailClient.Connect(email, password);
            client.DeleteAll();
        }

        public static Message GetMessage(Account mailAccount, string titlePattern) {
            return GetMessage(mailAccount.Login, mailAccount.Password, titlePattern);
        }

        public static Message GetMessage(string email, string password, string titlePattern) {
            var client = CreateClient(email, password);
            Message letter = null;
            for (int i = 0; i < 300; i++) {
                letter = client.GetLetter(titlePattern);
                if (letter != null)
                    break;
                Thread.Sleep(900);
            }
            return letter;
        }

        private static GMailClient CreateClient(string email, string password) {
            var client = GMailClient.Connect(email, password);
            return client;
        }
    }

    [TestFixture]
    public class MailHelperTests {
        [Test]
        public void DeleteAllTest() {
            MailHelper.GetMessage("s.westkm.mail@gmail.com", "Diana88@", "Email from West KM");
        }
    }
}