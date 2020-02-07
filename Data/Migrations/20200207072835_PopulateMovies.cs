using Microsoft.EntityFrameworkCore.Migrations;

namespace Vidly.Data.Migrations
{
    public partial class PopulateMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Movies (Id, Name, GenreId, DateAdded, ReleaseDate, NumberInStock) VALUES (1, 'Shrek!', 5, '2001/6/30', '2001/6/30', 10)");
            migrationBuilder.Sql("INSERT INTO Movies (Id, Name, GenreId, DateAdded, ReleaseDate, NumberInStock) VALUES (2, 'Wall-E', 3, '2008/6/21', '2008/6/21', 5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
