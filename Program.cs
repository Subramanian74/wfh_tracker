using System;
using System.Text;
using System.Globalization;
using System.IO;
namespace wfh_tracker;

class Program
{
  /// <summary>
  /// Function to validate an integer user input.
  /// </summary>
  /// <param name="input">string input</param>
  /// <returns>boolean based on if the string can be converted to an integer</returns>
  public bool inputValidator(string input)
  {
    int temp;
    return int.TryParse(input.ToString(), out temp);
  }

  /// <summary>
  /// Function to validate input work hours.
  /// </summary>
  /// <param name="input">integer input</param>
  /// <returns>boolean based on if the number is between 0 and 24</returns>
  public bool inputHoursValidator(int input)
  {
    if (input < 0 || input > 24) return false;

    return true;
  }

  /// <summary>
  /// Fetch a set number of records from a CSV file.
  /// </summary>
  /// <param name="filePath">Path of the file to fetch records from</param>
  /// <param name="numberOfRecordsToFetch">Integer input to fetch a certain number of records</param>
  /// <returns>StringBuilder type value with fetched records</returns>
  public StringBuilder fetchRecords(string filePath, int numberOfRecordsToFetch)
  {
    var reader = new StreamReader(filePath);
    string tempLine;
    var fileContent = new StringBuilder();
    int iterator = 0;

    while ((tempLine = reader.ReadLine()) != null)
    {
      iterator++;
      fileContent.AppendLine(tempLine);
      if (iterator == numberOfRecordsToFetch)
      {
        break;
      }
    }
    reader.Close();

    return fileContent;
  }

  /// <summary>
  /// Writes records to a specified file
  /// Inserts new records to the top.
  /// </summary>
  /// <param name="filePath">File location within the project structure</param>
  /// <param name="employeeData">Employee information to write</param>
  /// <param name="week">The week to enter the records for</param>
  /// <param name="businessDays">Number of days to enter records for</param>
  public void writeRecords(string filePath, Employee[] employeeData, int week, string[] businessDays)
  {
    var reader = new StreamReader(filePath);
    string tempLine;
    var fileContent = new StringBuilder();

    for (int outer = 0; outer < employeeData.Length; outer++)
    {
      string line = "week " + week + ", " + employeeData[outer].id + ", " + employeeData[outer].name + ", ";

      for (int inner = 0; inner < businessDays.Length - 1; inner++)
      {
        line += employeeData[outer].hoursWorked[inner] + ", ";
      }
      line += employeeData[outer].hoursWorked[businessDays.Length - 1];
      fileContent.AppendLine(line);
    }

    while ((tempLine = reader.ReadLine()) != null)
    {
      fileContent.AppendLine(tempLine);
    }
    reader.Close();

    // Re-writing the whole file to keep the new records at the top.
    File.WriteAllText(filePath, String.Empty);
    File.AppendAllText(filePath, fileContent.ToString());
  }

  /// <summary>
  /// Displays if an employee has worked for insufficient hours or has overworked, for a specific day.
  /// </summary>
  /// <param name="hoursWorked">Integer value for an employee's number of hours worked</param>
  /// <param name="day">The day to display</param>
  public void printHoursWorkedReport(int hoursWorked, string day)
  {
    if (hoursWorked < 4)
    {
      Console.WriteLine("Insufficient hours worked on " + day);
    }
    else if (hoursWorked > 10)
    {
      Console.WriteLine("Too many hours worked on " + day);
    }
  }

  /// <summary>
  /// Displays if an employee has worked for insufficient hours or has overworked, for a week.
  /// </summary>
  /// <param name="totalHoursWorked">Integer value for an employee's number of hours worked</param>
  public void printTotalHoursWorkedReport(int totalHoursWorked)
  {
    if (totalHoursWorked < 30)
    {
      Console.WriteLine("You didn’t do enough work this week");
    }
    else if (totalHoursWorked > 40)
    {
      Console.WriteLine("You are working too hard!!");
    }
  }

  /// <summary>
  /// Displays a summary after a weekly report has been entered.
  /// </summary>
  /// <param name="limit">The number of employees</param>
  /// <param name="employeeWeeklyHours">Integer array of weekly hours worked, per employee</param>
  public void printFullSummary(int limit, int[] employeeWeeklyHours)
  {
    Console.WriteLine("");
    Console.WriteLine("****************************************************************");
    Console.WriteLine("");
    Console.WriteLine("          * * * Weekly Employee Report * * *");
    int lessThan30Hours = 0;
    int moreThan40Hours = 0;
    int between37And40Hours = 0;
    for (int loop = 0; loop < limit; loop++)
    {
      if (employeeWeeklyHours[loop] < 30)
      {
        lessThan30Hours++;
      }
      else if (employeeWeeklyHours[loop] > 40)
      {
        moreThan40Hours++;
      }
      else if (employeeWeeklyHours[loop] >= 37 && employeeWeeklyHours[loop] <= 40)
      {
        between37And40Hours++;
      }
    }
    Console.WriteLine("Number of Employees who worked Less than 30 hours this week: " + lessThan30Hours);
    Console.WriteLine("Number of Employees who worked more than 40 hours this week: " + moreThan40Hours);
    Console.WriteLine("Number of Employees who worked Between 37-39 hours this week: " + between37And40Hours);
    Console.WriteLine("");
  }

  static void Main(string[] args)
  {
    // Constants
    const int NUMBER_OF_EMPLOYEES = 7;
    string[] businessDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
    string filePath = @"reports/master_employee_wfh_report.csv";
    Program program = new Program();

    // Variables
    int[] employeeWeeklyHours = new int[NUMBER_OF_EMPLOYEES]; // Data structure 2 - store number of hours worked by each employee, for the week
    Employee[] employeeData = new Employee[NUMBER_OF_EMPLOYEES];
    int week;
    string menuOptionInput;
    int menuOption = 0;

    Console.WriteLine("          * * * Phoenix IT Solutions * * *");
    Console.WriteLine("");
    Console.WriteLine("Work From Home Tracker System");
    Console.WriteLine("");
    Console.WriteLine("[1] Enter weekly report");
    Console.WriteLine("[2] Fetch records from weekly report");
    Console.WriteLine("[3] Exit");

    do
    {
      Console.Write("Enter your option: ");
      menuOptionInput = Console.ReadLine();
      Console.WriteLine("");

      if (!program.inputValidator(menuOptionInput))
      {
        Console.WriteLine("Invalid input. Please enter a valid option [1, 2 or 3]: ");
      }
      else
      {
        menuOption = int.Parse(menuOptionInput);

        switch (menuOption)
        {
          case 1:
            // Case to input weekly work from home details of every employee; and writing it to a CSV file.
            Console.WriteLine("          * * * Add employee working hours * * *");
            Console.WriteLine("");
            string weekInput = "";
            bool weakInputValidation;
            do
            {
              Console.Write("Enter current working week: ");
              weekInput = Console.ReadLine();
              if (!program.inputValidator(weekInput))
              {
                Console.WriteLine("Invalid input. Please enter a valid week number.");
                weakInputValidation = false;
              }
              else
              {
                weakInputValidation = true;
              }
            } while (!weakInputValidation);

            week = int.Parse(weekInput);
            Console.WriteLine("");

            for (int outer = 0; outer < NUMBER_OF_EMPLOYEES; outer++)
            {
              int tempId;
              string tempName;
              List<int> tempHoursWorked = new List<int>();

              Console.WriteLine("[Employee " + (outer + 1) + "]");

              string tempIdInput = "";
              bool tempIdInputValidation;
              do
              {
                Console.Write("Enter employee " + (outer + 1) + " id: ");
                tempIdInput = Console.ReadLine();
                if (!program.inputValidator(tempIdInput))
                {
                  Console.WriteLine("Invalid input. Please enter a valid number.");
                  tempIdInputValidation = false;
                }
                else
                {
                  tempIdInputValidation = true;
                }
              } while (!tempIdInputValidation);

              tempId = int.Parse(tempIdInput);

              Console.Write("Enter employee " + (outer + 1) + " name: ");
              tempName = Console.ReadLine();

              // Inner for-loop to input work hours for 5 days of the week.
              for (int inner1 = 0; inner1 < businessDays.Length; inner1++)
              {
                string tempHoursWorkedInput = "";
                bool tempHoursWorkedInputValidation;
                do
                {
                  Console.Write("Enter hours worked for " + businessDays[inner1] + ": ");
                  tempHoursWorkedInput = Console.ReadLine();
                  if (!program.inputValidator(tempHoursWorkedInput))
                  {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    tempHoursWorkedInputValidation = false;
                  }
                  else
                  {
                    bool isValidHours = program.inputHoursValidator(int.Parse(tempHoursWorkedInput));
                    if (!isValidHours)
                    {
                      Console.WriteLine("Invalid input. Please enter valid hours (between 0 and 24).");
                      tempHoursWorkedInputValidation = false;
                    }
                    else
                    {
                      tempHoursWorkedInputValidation = true;
                    }
                  }
                } while (!tempHoursWorkedInputValidation);

                tempHoursWorked.Add(int.Parse(tempHoursWorkedInput));
              }

              Employee obj = new Employee(tempId, tempName, tempHoursWorked);
              employeeData[outer] = obj;

              // Printing hours worked report.
              Console.WriteLine("**************************************");
              Console.WriteLine("Summary for Employee " + (outer + 1));
              for (int inner2 = 0; inner2 < employeeData[outer].hoursWorked.Count; inner2++)
              {
                program.printHoursWorkedReport(employeeData[outer].hoursWorked[inner2], businessDays[inner2]);
              }
              int totalHoursWorked = employeeData[outer].hoursWorked.Sum();
              employeeWeeklyHours[outer] = totalHoursWorked;
              program.printTotalHoursWorkedReport(totalHoursWorked);
              Console.WriteLine("Total hours worked for week " + week + ": " + totalHoursWorked + " hours");
              Console.WriteLine("");
              Console.WriteLine("");
            }

            // Displaying a full summary and saving to a CSV file.
            program.printFullSummary(NUMBER_OF_EMPLOYEES, employeeWeeklyHours);
            program.writeRecords(filePath, employeeData, week, businessDays);

            break;
          case 2:
            // Case to input a number and fetch the corresponding number of records.
            // If a user enters 0, return all records.
            int numberOfRecordsToFetch;
            string numberOfRecordsToFetchInput = "";
            bool numberOfRecordsToFetchInputValidation;
            do
            {
              Console.Write("Enter number of records to fetch (or 0 to fetch all records): ");
              numberOfRecordsToFetchInput = Console.ReadLine();
              if (!program.inputValidator(numberOfRecordsToFetchInput))
              {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                numberOfRecordsToFetchInputValidation = false;
              }
              else
              {
                numberOfRecordsToFetchInputValidation = true;
              }
            } while (!numberOfRecordsToFetchInputValidation);

            numberOfRecordsToFetch = int.Parse(numberOfRecordsToFetchInput);

            var fetchedRecords = new StringBuilder();
            fetchedRecords = program.fetchRecords(filePath, numberOfRecordsToFetch);
            if (fetchedRecords.Length == 0)
            {
              Console.WriteLine("No records found.");
            }
            else
            {
              Console.WriteLine("Fetched records: ");
              Console.WriteLine(fetchedRecords);
            }

            break;
          case 3:
            // Case exit the program.
            Console.WriteLine("Thank you.");
            Console.WriteLine("");
            Console.WriteLine("\u00a9 2024 Phoenix IT Solutions");
            break;
          default:
            Console.WriteLine("Invalid input. Please enter a valid option [1, 2 or 3]");
            break;
        }
      }
    } while (menuOption != 3);
  }
}
