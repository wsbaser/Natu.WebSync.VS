namespace selenium.core.Auxiliary {
    public class GMailClient : MailClient {
        public GMailClient()
            : base("imap.gmail.com", 993, true) {
        }

        public static GMailClient Connect(string email, string password) {
            var gMailClient = new GMailClient();
            ((MailClient) gMailClient).Connect(email, password);
            return gMailClient;
        }
    }
}
