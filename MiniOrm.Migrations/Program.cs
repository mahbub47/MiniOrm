using MiniOrm.Migrations.Commands;

public class Program
{
    public async static Task Main(string[] args)
    {
        var runner = new MigrationRunner();
        await runner.Run(args);
    }
}
