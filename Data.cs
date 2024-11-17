using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

/*public class Info
{
    public int InfoId { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }

    public int Id { get; set; }
    public Person Person { get; set; }
}*/