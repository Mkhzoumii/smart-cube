public class UserForLoginConformationDto
{
    public byte[] hash_password { get; set; }
    public byte[] salt_password { get; set; }

    public UserForLoginConformationDto()
    {
        hash_password = hash_password ?? new byte[0];
        salt_password = salt_password ?? new byte[0];
    }
}
