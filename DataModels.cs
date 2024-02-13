namespace wfh_tracker
{
  public class Employee
  {
    public int id;
    public string name;
    public int[] hoursWorked;

    public Employee(int id, string name, int[] hoursWorked)
    {
      this.id = id;
      this.name = name;
      this.hoursWorked = hoursWorked;
    }
  }
}
