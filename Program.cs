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
void DisplayList(List<Task> taskList){
    for (int i = 0; i < taskList.Count; i++){
      Console.WriteLine($"{i + 1}. {taskList[i].Name}");
    }
}

// Checks if the user has any tasks
bool HasTasks() {
  if (tasks.Count == 0) {
    Console.WriteLine("You have no tasks yet.");
    return false;
  }
  return true;
}
// Asks user to choose a task
int GetTaskIndex(List<Task> taskList, bool allowBack = false){
  while (true){
    string input = Console.ReadLine()!;

    if (!int.TryParse(input, out int taskNumber)){
      Console.WriteLine("Please choose a number.");
    }
    else{
      if (!(allowBack || taskNumber == 0)) {
        taskNumber--;

        if (taskNumber < -1 || taskNumber >= taskList.Count){
          Console.WriteLine("That task does not exist, please choose one that does.");
        }
        else{
          return taskNumber;
        }
      }
      else {
        return -1;
      }
    }
  }
}

// Displays task details
void ViewTask(List<Task> viewedTask) {
  while (true) {
    DisplayList(viewedTask);
    Console.WriteLine();

    Console.Write("What task would you like to view? ");
    int taskIndex = GetTaskIndex(viewedTask); 
    Console.WriteLine();

    // Display task details
    Console.WriteLine("====================");
    Console.WriteLine($"     {viewedTask[taskIndex].Name}   ");
    Console.WriteLine("====================");
    Console.WriteLine();

    Console.WriteLine($"Description: {viewedTask[taskIndex].Description}");
    Console.WriteLine($"Completion: {viewedTask[taskIndex].Completed}");
    Console.WriteLine($"Priority: {viewedTask[taskIndex].PriorityLevel}");
    if (viewedTask[taskIndex].DueDate == DateTime.Today && viewedTask[taskIndex].Completed == false) {
      Console.WriteLine("DUE TODAY");
    } else if (viewedTask[taskIndex].DueDate < DateTime.Today && viewedTask[taskIndex].Completed == false) {
      Console.WriteLine("OVERDUE");
    } else {
      Console.WriteLine($"Due date: {viewedTask[taskIndex].DueDate:dd/MM/yyyy}");
    }
    Console.WriteLine();

    Console.WriteLine("1. View another task");
    Console.WriteLine("0. Go back");
    Console.WriteLine();

    Console.Write("What would you like to do? ");

    int choice;

    while (true) {
      string userInput = Console.ReadLine()!;
      Console.WriteLine();

      if (!int.TryParse(userInput, out choice)) {
        Console.WriteLine("Please pick a valid choice.");
      }
      else {
        break;
      }
    }

    if (choice == 1) {
      continue;
    }
    else if (choice == 0) {
      return;
    } else {
      Console.WriteLine("That's not an option.");
      Console.WriteLine();
      continue;
    }
  }
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
  DisplayList(tasks);

  Console.Write("Which task would you like to complete or type 0 to go back: ");
  int taskIndex = GetTaskIndex(tasks, true);

  if (taskIndex == -1){
    return;
  }

  tasks[taskIndex].Complete();
}

// Toggles a task's completion
void ToggleCompletion() {
  DisplayList(tasks);

  Console.Write("Which task would you like to toggle completion or type 0 to go back: ");
  int taskIndex = GetTaskIndex(tasks, true);

  if (taskIndex == -1){
    return;
  }

  tasks[taskIndex].ToggleComplete(); 
}
  
// Deletes a task
void DeleteTask() {
  DisplayList(tasks);

  Console.Write("Which task would you like to delete or type 0 to go back: ");
  int taskIndex = GetTaskIndex(tasks, true);

  if (taskIndex == -1){
    return;
  }

  tasks.RemoveAt(taskIndex);
}

// Configure a task
void ConfigureTask() {
  DisplayList(tasks);

  Console.Write("Which task would you like to edit or type 0 to go back: ");
  int taskIndex = GetTaskIndex(tasks, true);

  if (taskIndex == -1){
    return;
  }

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
    if (tasks[i].Name.Contains(taskToSearch, StringComparison.OrdinalIgnoreCase)) {
      result.Add(tasks[i]);
    }
  }
  return result;
}

void SearchTaskMenu() {
  while (true) {
    List<Task> results = SearchTasks();

    if (results.Count == 0) {
      Console.WriteLine("Nothing found.");
    }
    else {
      Console.WriteLine();
      ViewTask(results);
    }
    Console.WriteLine("1. Continue searching");
    Console.WriteLine("0. Go back");
    Console.Write("What would you like to do: ");

    
    while (true) {
      string choice = Console.ReadLine()!;

      if (int.TryParse(choice, out int chosenNumber)) {
        if (chosenNumber == 0) {
          return;
        }
        else if (chosenNumber == 1) {
          break;
        }
        else {
          Console.WriteLine("Not an option.");
          continue;
        }
      }
      else {
        Console.WriteLine("Please choose a number.");
      }
    }
  }
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
  Console.WriteLine("7. Search For Task");
  Console.WriteLine("0. Exit");
  Console.WriteLine();

  int userChoice;

  // Asks user to select an action
  while (true){
    Console.Write("What would you like to do? ");
    string input = Console.ReadLine()!;

    if (int.TryParse(input, out userChoice)){
      if (userChoice < 0 || userChoice > 7){
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
      ViewTask(tasks);
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
  else if (userChoice == 7) {
    if (HasTasks()) {
      SearchTaskMenu();
    }
  }
  else if (userChoice == 0){
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
    Completed = !Completed;
  }
}
