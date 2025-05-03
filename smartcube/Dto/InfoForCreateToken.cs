namespace smartcube.Dto
{
    public class InfoForCreateToken
    {

        public string email { get; set; }
        public string first_name { get; set; }
        public int user_id { get; set; }

        public InfoForCreateToken()
        {
            email = email ?? " ";
            first_name = first_name ?? " ";
        }
    }
}