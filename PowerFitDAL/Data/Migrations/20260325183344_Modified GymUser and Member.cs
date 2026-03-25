using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerFitDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedGymUserandMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Trainers");
        }
    }
}
