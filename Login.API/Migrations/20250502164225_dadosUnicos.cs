using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Login.API.Migrations
{
    /// <inheritdoc />
    public partial class dadosUnicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TblUser_Email",
                table: "TblUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblUser_Username",
                table: "TblUser",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TblUser_Email",
                table: "TblUser");

            migrationBuilder.DropIndex(
                name: "IX_TblUser_Username",
                table: "TblUser");
        }
    }
}
