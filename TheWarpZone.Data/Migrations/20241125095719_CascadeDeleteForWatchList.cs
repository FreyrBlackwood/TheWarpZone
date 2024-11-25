using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheWarpZone.Data.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteForWatchList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaLists_Movies_MovieId",
                table: "UserMediaLists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaLists_TVShows_TVShowId",
                table: "UserMediaLists");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaLists_Movies_MovieId",
                table: "UserMediaLists",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaLists_TVShows_TVShowId",
                table: "UserMediaLists",
                column: "TVShowId",
                principalTable: "TVShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaLists_Movies_MovieId",
                table: "UserMediaLists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMediaLists_TVShows_TVShowId",
                table: "UserMediaLists");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaLists_Movies_MovieId",
                table: "UserMediaLists",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMediaLists_TVShows_TVShowId",
                table: "UserMediaLists",
                column: "TVShowId",
                principalTable: "TVShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
