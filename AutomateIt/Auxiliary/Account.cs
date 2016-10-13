namespace automateit.Auxiliary
{
    public class Account
    {
        public string ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Account(string id, string login, string password) {
            ID = id;
            Login = login;
            Password = password;
        }
    }
}
