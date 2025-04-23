using System.Collections.Generic;

public class CommandManager
{
    private static CommandManager instance;
    public static CommandManager Instance => instance ??= new CommandManager();

    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    private CommandManager() { }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }

    public void Undo()
    {
        if (commandHistory.Count > 0)
        {
            var command = commandHistory.Pop();
            command.Undo();
        }
    }

    public void Clear()
    {
        commandHistory.Clear();
    }
}