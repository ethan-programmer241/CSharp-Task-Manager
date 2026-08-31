using System.Text.Json;

List<Task> tasks;

if (File.Exists("tasks.json")){
  string jsonFromFile = File.ReadAllText("tasks.json");
  tasks = JsonSerializer.Deserialize<List<Task>>(jsonFromFile) ?? new List<Task>();
}
else{
  tasks = new List<Task>();
}

void DisplayList(){
    for (int i = 0; i < tasks.Count; i++){
      Console.WriteLine($"Task {i + 1}.");
      tasks[i].Display();
    }
}

int GetTaskIndex(){
  while (true){
    string input = Console.ReadLine()!;

    if (!int.TryParse(input, out int taskNumber)){
      Console.WriteLine("Please choose a number.");
    }
    else{
      taskNumber--;

      if (taskNumber < 0 || taskNumber >= tasks.Count){
        Console.WriteLine("That task does not exist, please choose one that does.");
      }
      else{
        return taskNumber;
      }
    }
  }
}

while (true){
  Console.WriteLine("====================");
  Console.WriteLine("     TASK MANAGER   ");
  Console.WriteLine("====================");
  Console.WriteLine();

  Console.WriteLine("1. View Tasks");
  Console.WriteLine("2. Add Task");
  Console.WriteLine("3. Complete Task");
  Console.WriteLine("4. Toggle Completion");
  Console.WriteLine("5. Delete Task");
  Console.WriteLine("6. Configure Task");
  Console.WriteLine("7. Exit");
  Console.WriteLine();

  int userChoice;

  while (true){
    Console.Write("What would you like to do? ");
    string input = Console.ReadLine()!;

    if (int.TryParse(input, out userChoice)){
      if (userChoice < 1 || userChoice > 7){
        Console.WriteLine("Please pick a valid option.");
      }
      else{
        break;
      }
    }
    else{
      Console.WriteLine("Please choose a number.");
    }
  }
  Console.WriteLine();

  if (userChoice == 1){
    DisplayList();
  }
  else if (userChoice == 2){
    Console.Write("Task name: ");
    string newTaskName = Console.ReadLine()!;

    Console.Write("Task description: ");
    string newTaskDescription = Console.ReadLine()!;

    Priority priority;

    while (true){
      Console.Write("How much priority: High, Medium, or Low? ");
      string newTaskPriority = Console.ReadLine()!;

      if (Enum.TryParse<Priority>(newTaskPriority, true, out priority)){
        break;
      }

      Console.WriteLine("Invalid Priority");
    }

    Task newTask = new Task(newTaskName, newTaskDescription, false, priority);

    tasks.Add(newTask);
  }
  else if (userChoice == 3){
    DisplayList();

    Console.Write("Which task have you completed? ");
    int taskIndex = GetTaskIndex(); 

    tasks[taskIndex].Complete();
  }
  else if (userChoice == 4){
    DisplayList();

    Console.Write("Which task would you like to toggle the completion? ");
    int taskIndex = GetTaskIndex();

    tasks[taskIndex].ToggleComplete();
  }
  else if (userChoice == 5){
    DisplayList();

    Console.Write("Which task would you like to delete: ");
    int taskIndex = GetTaskIndex();

    tasks.RemoveAt(taskIndex);
  }
  else if (userChoice == 6){
    DisplayList();

    Console.Write("Which task would you like to edit: ");
    int taskIndex = GetTaskIndex();

    Console.Write("New Task name: ");
    string newTaskName = Console.ReadLine()!;

    Console.Write("New Task description: ");
    string newTaskDescription = Console.ReadLine()!;

    Priority priority;

    string newTaskPriority;

    while (true){
      Console.Write("How much priority: High, Medium, or Low? ");
      newTaskPriority = Console.ReadLine()!;

      if (Enum.TryParse<Priority>(newTaskPriority, true, out priority)){
        break;
      }

      Console.WriteLine("Invalid Priority");
    }

    tasks[taskIndex].Name = newTaskName;
    tasks[taskIndex].Description = newTaskDescription;
    tasks[taskIndex].PriorityLevel = priority;
  }
  else if (userChoice == 7){
    string json = JsonSerializer.Serialize(tasks);
    File.WriteAllText("tasks.json", json);
    break;
  }
}

enum Priority{
  Low,
  Medium,
  High
}

class Task
{
  public string Name { get; set; }
  public string Description { get; set; } 
  public bool Completed { get; private set; } 
  public Priority PriorityLevel {get; set; }

  public Task(string name, string description, bool completed, Priority priorityLevel){
    Name = name;
    Description = description;
    Completed = completed;
    PriorityLevel = priorityLevel;
  }

  public void Complete(){
    Completed = true;
  }

  public void ToggleComplete(){
    /*if (Completed == false)
    {
      Completed = true;
    }
    else
    {
      Completed = false;
    } */

    Completed = !Completed;
  }

  public void Display(){
    Console.WriteLine($"Name: {Name}");
    Console.WriteLine($"Description: {Description}");
    Console.WriteLine($"Completed: {Completed}");
    Console.WriteLine($"Priority: {PriorityLevel}");
    Console.WriteLine();
  }
}
