using codecrafters_shell.CommandInterpreter;

namespace codecrafters_shell;

public class Shell:IShell
{
    private ICommandInterpreter _commandInterpreter;
    public Shell(ICommandInterpreter commandInterpreter)
    {
        _commandInterpreter = commandInterpreter;
    }

    public void Run()
    {
        while (true)
        {
            Console.Write("$ ");
            var command = Console.ReadLine();

            if (!_commandInterpreter.InterpretCommand(command))
            {
                break;
            }
        }
    }
}