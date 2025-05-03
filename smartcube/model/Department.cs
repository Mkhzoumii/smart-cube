using smartcube.Model;

namespace smartcube.model;
public class Department
{
    public Department()
    {
        Users = new HashSet<Users>();
    }

    public int Id { get; set; }
    public string Name { get; set; }


    public ICollection<Users> Users { get; set; }
}
