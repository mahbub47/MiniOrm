using MiniOrm.Migrations.Commands;

public class Program
{
    public static void Main(string[] args)
    {
        var runner = new MigrationRunner();
        runner.Run(args);
    }
}
