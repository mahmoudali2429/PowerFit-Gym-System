using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerFitDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedPropertySpecialitiestoSpecialtiesInTrainerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialities",
                table: "Trainers",
                newName: "Specialties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialties",
                table: "Trainers",
                newName: "Specialities");
        }
    }
}
