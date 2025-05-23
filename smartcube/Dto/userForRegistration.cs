namespace smartcube.Dto
{
    public class UserForRegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserForRegistrationDto()
        {
            Email = Email ?? " ";
            Password = Password ?? " ";
            PasswordConfirm = PasswordConfirm ?? " ";
            FirstName = FirstName ?? " ";
            LastName = LastName ?? " ";
        }
    }
}
