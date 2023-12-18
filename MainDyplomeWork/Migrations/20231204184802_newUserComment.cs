using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartReservationCinema.Migrations
{
    public partial class newUserComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Films_IdFilm",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_IdUser",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_IdUser",
                table: "Comments",
                newName: "IX_Comments_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_IdFilm",
                table: "Comments",
                newName: "IX_Comments_IdFilm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Films_IdFilm",
                table: "Comments",
                column: "IdFilm",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Films_IdFilm",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_IdUser",
                table: "Comment",
                newName: "IX_Comment_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_IdFilm",
                table: "Comment",
                newName: "IX_Comment_IdFilm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Films_IdFilm",
                table: "Comment",
                column: "IdFilm",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_IdUser",
                table: "Comment",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
