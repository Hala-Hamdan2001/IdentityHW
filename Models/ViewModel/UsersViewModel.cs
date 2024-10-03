namespace Identity1.Models.ViewModel
{
    public class UsersViewModel
    {
        public string Id {  get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
