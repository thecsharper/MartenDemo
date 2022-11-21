namespace MartenDemo.Helpers
{
    public class UserRepository
    {
        public User Get(string username, string password)
        {
            var users = new List<User>()
            {
                new User() {
                             Id = 1,
                             Username = "test",
                             Password = "423423rFAsas£!^^34rrsdf",
                             Role = "user"
                           }
            };


            var result = users.FirstOrDefault(user => user.Username!.ToLower() == username && user.Password == password);
            
            if (result != null)
            {
                return result;
            }

            return new User();
        }
    }
}
