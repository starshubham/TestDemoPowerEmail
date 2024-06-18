namespace TestDemoPower.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        //[NotMapped]
        public Role Role { get; set; }  // Remove [NotMapped] attribute

    }
}
