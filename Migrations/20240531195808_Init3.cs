using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon2024API.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "5586802b-4d14-45b8-9950-a48ca9fa777a", "ADMIN@MAIL.RU", "ADMIN USER", "AQAAAAIAAYagAAAAEC6nHLZi5oj5bZoC4bzwW91zmCZSCpr5K91F65xwgabMpnZN2VDTB1wlD1dikizFlg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "08f151b4-fdc2-4874-a3d7-a3fa9e67dabb", null, null, "AQAAAAIAAYagAAAAEKERy19dHQIbf8bkHCepwl+fmU/XNWdcomoPbSCMDzY8Xh25TA8EfCIKN+PwJDWMdw==" });
        }
    }
}
