using Microsoft.EntityFrameworkCore.Migrations;

namespace CLICommander.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commands", // name from the CLICommanderContext class
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false) 
                        .Annotation("SqlServer:Identity", "1, 1"), // id will automatically increase by 1 for each row inserted, starting at 1
                    HowTo = table.Column<string>(maxLength: 300, nullable: false),
                    Line = table.Column<string>(nullable: false),
                    Platform = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id); // primary key as convention
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commands");
        }
    }
}
