using Autofac;
using codecrafters_shell;
using codecrafters_shell.CommandInterpreter;

class Program
{
    static void Main()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Shell>().As<IShell>().SingleInstance();
        builder.RegisterType<CommandInterpreter>().As<ICommandInterpreter>().SingleInstance();
        var container = builder.Build();
        var shell = container.Resolve<IShell>();
        RunShell(shell);
    }

    static void RunShell(IShell shell)
    {
        shell.Run();
    }

}