
<<<<<<< Updated upstream
=======
using MiniOrm.Data.Metadata;
using MiniOrm.Migrations.Commands;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        var runner = new MigrationRunner();
        runner.Run(args);
    }
}
>>>>>>> Stashed changes
