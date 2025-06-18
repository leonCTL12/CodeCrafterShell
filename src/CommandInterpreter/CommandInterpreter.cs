using System.Diagnostics;

namespace codecrafters_shell.CommandInterpreter;

public class CommandInterpreter : ICommandInterpreter
{
    private const string EchoCommand = "echo";
    private const string TypeCommand = "type";
    private const string ExitCommand = "exit";

    private const string PathEnvironmentVariable = "PATH";

    private static readonly HashSet<string> ValidCommands =
    [
        EchoCommand,
        TypeCommand,
        ExitCommand
    ];

    // The boolean return type is for indicating whether the shell should continue running.
    public bool InterpretCommand(string commandString)
    {
        var command = new Queue<string>(commandString.Split(' '));

        if (TryRunProgram(command)) return true;
        var action = command.Dequeue();

        if (action == ExitCommand)
            //Ignore if it is exit 0 or not for now
            return false; // Indicates that the shell should stop running.
        if (action == EchoCommand)
        {
            ExecuteEchoCommand(command);
            return true;
        }

        if (action == TypeCommand)
        {
            ExecuteTypeCommand(command);
            return true;
        }

        Console.WriteLine($"{action}: command not found");
        return true;
    }

    private void ExecuteTypeCommand(Queue<string> command)
    {
        if (command.Count == 0)
        {
            Console.WriteLine("No command provided for type command.");
            return;
        }

        var action = command.Dequeue();
        if (ValidCommands.Contains(action))
        {
            Console.WriteLine($"{action} is a shell builtin");

            return;
        }

        var paths = GetEnvironmentPaths(action);
        foreach (var path in paths)
        {
            if (File.Exists(path))
            {
                Console.WriteLine($"{action} is {path}");
                return;
            }
        }


        Console.WriteLine($"{action}: not found");
    }

    private void ExecuteEchoCommand(Queue<string> command)
    {
        if (command.Count == 0)
        {
            Console.WriteLine("No arguments provided for echo command.");
            return;
        }

        var message = string.Join(" ", command);
        Console.WriteLine(message);
    }

    //The bool is for indicating whether the path is correct
    private bool TryRunProgram(Queue<string> command)
    {
        var executableName = command.Dequeue();

        if (File.Exists(executableName))
        {
            RunProgram(executableName, command);
            return true;
        }

        var paths = GetEnvironmentPaths(executableName);
        foreach (var path in paths)
        {
            if (!File.Exists(path))
            {
                continue;
            }

            RunProgram(executableName, command);
            return true;
        }

        return false;
    }

    private void RunProgram(string executablePath, Queue<string> arguementsQueue)
    {
        List<string> argumentList = [];
        while (arguementsQueue.Count > 0)
        {
            argumentList.Add(arguementsQueue.Dequeue());
        }

        var process = Process.Start(new ProcessStartInfo(executablePath, argumentList));

        process?.WaitForExit();
    }

    private List<string> GetEnvironmentPaths(string fileName)
    {
        var pathEnvironmentVariable = Environment.GetEnvironmentVariable(PathEnvironmentVariable);
        var paths = pathEnvironmentVariable?.Split(':').ToList() ?? [];
        return paths.Distinct().Select(path => Path.Combine(path, fileName)).ToList();
    }
}