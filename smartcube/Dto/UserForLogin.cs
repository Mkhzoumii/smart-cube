
public class UserForLoginDto
{
   
    public string email { get; set; }
    public string Password { get; set; }

    public UserForLoginDto()
    {
        email = email ?? " ";
        Password = Password ?? " ";
    }
}
