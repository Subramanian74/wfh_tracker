namespace wfh_tracker
{
  public class Employee
  {
    public int id;
    public string name;
    public List<int> hoursWorked;

    public Employee(int id, string name, List<int> hoursWorked)
    {
      this.id = id;
      this.name = name;
      this.hoursWorked = hoursWorked;
    }
  }
}
