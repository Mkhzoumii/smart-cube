namespace smartcube.Dto
{
    public class RegistrationDto
    {
        public string email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public RegistrationDto()
        {
            email = email ?? " ";
            Password = Password ?? " ";
            PasswordConfirm = PasswordConfirm ?? " ";
            first_name = first_name ?? " ";
            last_name = last_name ?? " ";
        }
    }
}
