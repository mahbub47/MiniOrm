
using MiniOrm.Models;

// # Step 01 : Define Entites with attributes
//             - Create simple entity class with properties and decorate it with attributes to specify table and column mappings, primary keys, etc.
//               (You can Follow the demo product and order entities in the Models folder as a reference.)
//
//             - [Table] at the class level to specify the corresponding database table name.
//               (It is not mendatory to apply you can also give the specific name if you want otherwise it will take the class name)
//
//             - [Column] at the property level to specify the corresponding database column name.
//               (It is not mendatory to apply you can also give the specific name if you want otherwise it will take the property name)
//
//             - [PrimaryKey] to indicate the primary key property of the entity.
//               (if you dont want to use it then make sure your one column is named as Id and it will be considered as primary key by default.)
//               (Make sure you dont use this Attribute while having another property has the name Id. This lead to a multiple PK issue)


// # Step 02 : Create DbContext class and register all DbSet properties for your entities
//              (You can Follow the Demo AppDbContext or you can create your new one)
//              (Please make sure you DbContext name is AppDbContext, Anything else will not going to work)


// # Step 03 : Instantiate your DbContext and Run Migrations
//              - First set yout Environment variable MINIORM_CONN to your Postgres connection string.
//                (Example: Host=localhost;Database=miniorm_db;Username=postgres;Password=secret)
//              - Then instantiate your DbContext by passing the connection string from the environment variable.

var connectionString = Environment.GetEnvironmentVariable("MINIORM_CONN")
    ?? throw new InvalidOperationException(
        "Set MINIORM_CONN to your Postgres connection string.\n" +
        "Example: Host=localhost;Database=miniorm_db;Username=postgres;Password=secret"); ;

var db = new AppDbContext(connectionString!);

// [Now please craete the database manually on your Postgres server make sure the name of the database matches the connection string database name. Now you good to go to run migrations]
//              - Before going to the next step please Run Migrations to create the database and tables based on your entities
//              - Go to the MiniOrm.Migration project directory and run the following commands:
//                 01. dotnet run -- migrations add <Your_Migration_Name> (To create migrstion file)
//                 02. dotnet run -- migrations apply (To apply migration to the database and create tables, add column, alter column, drop column and drop table)
//                 03. dotnet run -- migrations list (To see all migrations with marker [Pending] and [Applied])
//                 04. dotnet run -- migrations rollback (To rollback the last applied migration, it will drop the table, remove column, alter column and create table based on the last applied migration)
//              - After successfully running the migrations you should see the database and tables created in your Postgres database based on your entities.


// # Step 04 : Insert entities via DbSet
//              - 

var keyBoard = new Product
{
    Name = "Keyboard",
    Price = 500m,
    InStock = true
};

var keyBoardId = await db.Products!.InsertAsync(keyBoard);
//Output: Id: 1, ProductName: Keyboard, Price: 500, Discount: NULL, IsAvailable: True, 


// # Step 05 : Query, update, delete — verify final state

var found = await db.Products!.FindByIdAsync(1);  // make sure you pass a valid id that exists in your database
// Output: Id: 1, ProductName: Keyboard, Price: 500, Discount: NULL, IsAvailable: True,

found.Price = 450m;
found.Discount = 10m;

await db.Products!.UpdateAsync(found);
// Output: Id: 1, ProductName: Keyboard, Price: 450, Discount: 10, IsAvailable: True,

var allProducts = await db.Products!.GetAllAsync();
// all products in the database with their details

await db.Products!.DeleteAsync(1);
// After deletion, if you try to find the same product by id it should return null or not found

