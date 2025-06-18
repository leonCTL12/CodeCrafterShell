namespace codecrafters_shell.CommandInterpreter;

public interface ICommandInterpreter
{
    bool InterpretCommand(string commandString);
}