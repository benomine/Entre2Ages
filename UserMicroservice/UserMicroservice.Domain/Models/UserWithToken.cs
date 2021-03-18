namespace UserMicroservice.Domain.Models
{
    public class UserWithToken : User
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public UserWithToken(User user)
        {
            this.Id = user.Id;
            this.Email = user.Email;            
            this.Name = user.Name;
            this.Phone = user.Phone;
            this.Phone2 = user.Phone2;
            this.City = user.City;
            this.Address = user.Address;
            this.ZipCode = user.ZipCode;
        }
    }
}