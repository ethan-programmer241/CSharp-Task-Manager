using System.Text.Json;

List<Task> tasks;

// Check if existing tasks exist
if (File.Exists("tasks.json")){
  string jsonFromFile = File.ReadAllText("tasks.json");
  tasks = JsonSerializer.Deserialize<List<Task>>(jsonFromFile) ?? new List<Task>();
}
else{
  // Otherwise create new ones
  tasks = new List<Task>();
}

// Display current tasks
void DisplayList(){
    for (int i = 0; i < tasks.Count; i++){
      Console.WriteLine($"{i + 1}, {tasks[i].Name}");
    }
}

bool HasTasks() {
  if (tasks.Count == 0) {
    Console.WriteLine("You have no tasks yet.");
    return false;
  }
  return true;
}
// Asks user to choose a task
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

void ViewTask() {
  DisplayList();

  Console.Write("What task would you like to view? ");
  int taskIndex = GetTaskIndex(); 

  tasks[taskIndex].Display();
}

// Adds a task to the task list
void AddTask() {
  Console.Write("Task name: ");
  string newTaskName = Console.ReadLine()!;

  Console.Write("Task description: ");
  string newTaskDescription = Console.ReadLine()!;

  Priority priority = GetPriority();
  DateTime dueDate = GetDueDate();

  Task newTask = new Task(newTaskName, newTaskDescription, false, priority, dueDate);

  tasks.Add(newTask);
}

// Completes a task
void CompleteTask() {
  DisplayList();

  Console.Write("Which task have you completed? ");
  int taskIndex = GetTaskIndex(); 

  tasks[taskIndex].Complete();
}

// Toggles a task's completion
void ToggleCompletion() {
  DisplayList();

  Console.Write("Which task would you like to toggle the completion? ");
  int taskIndex = GetTaskIndex();

  tasks[taskIndex].ToggleComplete(); 
}
  
// Deletes a task
void DeleteTask() {
  DisplayList();

  Console.Write("Which task would you like to delete: ");
  int taskIndex = GetTaskIndex();

  tasks.RemoveAt(taskIndex);
}

// Configure a task
void ConfigureTask() {
  DisplayList();

  Console.Write("Which task would you like to edit: ");
  int taskIndex = GetTaskIndex();

  Console.Write("New Task name: ");
  string newTaskName = Console.ReadLine()!;

  Console.Write("New Task description: ");
  string newTaskDescription = Console.ReadLine()!;
  
  Priority newPriority = GetPriority();
  DateTime dueDate = GetDueDate();

  tasks[taskIndex].Name = newTaskName;
  tasks[taskIndex].Description = newTaskDescription;
  tasks[taskIndex].PriorityLevel = newPriority;
  tasks[taskIndex].DueDate = dueDate;
}

// Gets a valid priority from the user
Priority GetPriority() {
  string newTaskPriority;
  Priority priority;

  // Keep asking until the user enters a valid priority
  while (true){
    Console.Write("How much priority: High, Medium, or Low? ");
    newTaskPriority = Console.ReadLine()!;

    if (Enum.TryParse<Priority>(newTaskPriority, true, out priority)){
      return priority;
    }
    Console.WriteLine("Invalid Priority");
  }
}

// Gets a due date from the user
DateTime GetDueDate() {
  DateTime dueDate;

  while (true){
    Console.Write("When do you want it to be due by? dd/MM/yyyy: ");
    string userDueDate = Console.ReadLine()!;

    if (DateTime.TryParse(userDueDate, out dueDate)){
      return dueDate;
    }

    Console.WriteLine("Invalid date");
  }
}

List<Task> SearchTasks() {
  Console.Write("What would you like to search for? ");
  string taskToSearch = Console.ReadLine()!;

  List<Task> result = new List<Task>();

  for (int i = 0; i < tasks.Count; i++){
    if (tasks[i].Name == taskToSearch) {
      result.Add(tasks[i]);
    }
  }

  return result;
}
// Main loop
while (true){
  // Display options on what to do
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

  // Asks user to select an action
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

  // Performs the action chosen by the user
  if (userChoice == 1){
    if (HasTasks()) {
      ViewTask();
    }
  }
  else if (userChoice == 2){
    AddTask();
  }
  else if (userChoice == 3){
    if (HasTasks()) {
      CompleteTask();
    }
  }
  else if (userChoice == 4){
    if (HasTasks()) {
      ToggleCompletion();
    }
  }
  else if (userChoice == 5){
    if (HasTasks()) {
      DeleteTask();
    }
  }
  else if (userChoice == 6){
    if (HasTasks()) {
      ConfigureTask();
    }
  }
  else if (userChoice == 7){
    string json = JsonSerializer.Serialize(tasks);
    File.WriteAllText("tasks.json", json);
    break;
  }
}

// Different priority levels
enum Priority{
  Low,
  Medium,
  High
}

// What each task contains
class Task
{
  public string Name { get; set; }
  public string Description { get; set; } 
  public bool Completed { get; private set; } 
  public Priority PriorityLevel {get; set; }
  public DateTime DueDate { get; set; }

  public Task(string name, string description, bool completed, Priority priorityLevel, DateTime dueDate){
    Name = name;
    Description = description;
    Completed = completed;
    PriorityLevel = priorityLevel; 
    DueDate = dueDate;
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
    if (DueDate == DateTime.Today && Completed == false) {
      Console.WriteLine("DUE TODAY");
    } else if (DueDate < DateTime.Today && Completed == false) {
      Console.WriteLine("OVERDUE");
    } else {
      Console.WriteLine($"Due date: {DueDate:dd/MM/yyyy}");
    }

    Console.WriteLine();
  }
}
