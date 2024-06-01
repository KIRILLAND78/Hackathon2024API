using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hackathon2024API.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanChange",
                table: "AspNetUsers",
                newName: "MandatoryEncryption");

            migrationBuilder.AddColumn<bool>(
                name: "Encrypted",
                table: "UserFiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "ImageQuality",
                table: "AspNetUsers",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<long>(
                name: "MaxFileSizeMb",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "FileExtentions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileExtentions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileExtentionUser",
                columns: table => new
                {
                    FileExtentionId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileExtentionUser", x => new { x.FileExtentionId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_FileExtentionUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileExtentionUser_FileExtentions_FileExtentionId",
                        column: x => x.FileExtentionId,
                        principalTable: "FileExtentions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "ImageQuality", "MaxFileSizeMb", "PasswordHash" },
                values: new object[] { "22ef6da8-fd6f-4401-b474-6a07af601cc3", (byte)0, 0L, "AQAAAAIAAYagAAAAEFAIEr8/mQ42gjn2cXgn743ZUePPXERx6b5/XNGNOAm3+/MLEGIbsPIAFcNTOJ2lkQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_FileExtentionUser_UsersId",
                table: "FileExtentionUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileExtentionUser");

            migrationBuilder.DropTable(
                name: "FileExtentions");

            migrationBuilder.DropColumn(
                name: "Encrypted",
                table: "UserFiles");

            migrationBuilder.DropColumn(
                name: "ImageQuality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxFileSizeMb",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "MandatoryEncryption",
                table: "AspNetUsers",
                newName: "CanChange");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5586802b-4d14-45b8-9950-a48ca9fa777a", "AQAAAAIAAYagAAAAEC6nHLZi5oj5bZoC4bzwW91zmCZSCpr5K91F65xwgabMpnZN2VDTB1wlD1dikizFlg==" });
        }
    }
}
