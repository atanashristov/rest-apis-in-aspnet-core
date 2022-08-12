using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApi.Migrations
{
    public partial class SeedSongsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Duration", "Language", "Title" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 120, "en", "Song 1" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 240, "en", "Song 2" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), 360, "en", "Song 3" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), 480, "en", "Song 4" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), 600, "en", "Song 5" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), 720, "en", "Song 6" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), 840, "en", "Song 7" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), 960, "en", "Song 8" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), 1080, "en", "Song 9" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), 1200, "en", "Song 10" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), 1320, "en", "Song 11" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), 1440, "en", "Song 12" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), 1560, "en", "Song 13" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"));
        }
    }
}
