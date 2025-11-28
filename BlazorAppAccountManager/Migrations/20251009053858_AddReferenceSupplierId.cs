using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorAppAccountManager.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceSupplierId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "003715f4-b0ac-4d2f-8257-dd72631b4e79");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "021f6c80-334b-4aac-9d67-cbe5d0fa9244");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "07f3c786-4c84-4e6f-8f17-55b43960c387");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "3d4e3a1a-3c2c-47a6-943d-52f763b9273e");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "6312b0a7-20e5-4e8a-883d-683b27cac8ec");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "68f62872-1f3e-4499-b50e-adb6e2661ffa");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "7fdce48a-99d5-4c6c-be5b-7630b3017bfd");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "9fcc3052-160e-4122-b824-56f4e5dc693a");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "af5a16ab-de57-4473-b353-348a207a2d47");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "cc8cf0b5-6610-48a7-abf5-04afff058d36");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "d06b6873-417b-4612-9e32-400afd33d14d");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "e2864d8b-62d2-4600-8119-35f1f1173fd0");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "ee0b1621-f601-41c0-90b2-503e70f92c5b");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "f4f0342a-fac3-4718-b648-34633c49b929");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceSupplierId",
                table: "Suppliers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "LampTemplates",
                columns: new[] { "Id", "Name", "SortOrder" },
                values: new object[,]
                {
                    { "0e494f64-e9ea-4043-a8d0-7795fa9267bd", "昼间行驶灯", 4 },
                    { "2a5536fa-944c-45a1-9cbb-7505508a563a", "前雾灯", 3 },
                    { "33505677-58ef-4df7-9ffa-9ddd7583cb31", "前转向灯", 6 },
                    { "3d103bd5-a398-4921-874a-5a54c45f4374", "倒车灯", 11 },
                    { "59044fb6-57b2-4cd2-a14e-51a9e78ef362", "前位置灯", 5 },
                    { "6860b153-d4ea-48df-8426-65434314b6b5", "高位制动灯", 13 },
                    { "69371252-eda4-487d-bd24-0c093e634a55", "角灯", 7 },
                    { "7bb6f600-048d-4200-a02b-e46bc5154560", "近光", 2 },
                    { "c8193240-6476-4df1-860e-6c2dafc3a4bf", "后雾灯", 12 },
                    { "d24aef23-0dbb-431d-ada1-db1c13c76ef3", "牌照灯", 14 },
                    { "f0c14242-2e12-405e-8243-604d23f9f949", "制动灯", 8 },
                    { "f2139425-2c93-40cf-ac5b-9ac6a9a027ee", "远光", 1 },
                    { "f67b463b-dedf-4139-aebc-e32331e3f9e9", "后位置灯", 10 },
                    { "fd72848f-1c01-4ea6-bad0-805edc9f7ec2", "后转向灯", 9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "0e494f64-e9ea-4043-a8d0-7795fa9267bd");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "2a5536fa-944c-45a1-9cbb-7505508a563a");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "33505677-58ef-4df7-9ffa-9ddd7583cb31");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "3d103bd5-a398-4921-874a-5a54c45f4374");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "59044fb6-57b2-4cd2-a14e-51a9e78ef362");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "6860b153-d4ea-48df-8426-65434314b6b5");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "69371252-eda4-487d-bd24-0c093e634a55");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "7bb6f600-048d-4200-a02b-e46bc5154560");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "c8193240-6476-4df1-860e-6c2dafc3a4bf");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "d24aef23-0dbb-431d-ada1-db1c13c76ef3");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "f0c14242-2e12-405e-8243-604d23f9f949");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "f2139425-2c93-40cf-ac5b-9ac6a9a027ee");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "f67b463b-dedf-4139-aebc-e32331e3f9e9");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "fd72848f-1c01-4ea6-bad0-805edc9f7ec2");

            migrationBuilder.DropColumn(
                name: "ReferenceSupplierId",
                table: "Suppliers");

            migrationBuilder.InsertData(
                table: "LampTemplates",
                columns: new[] { "Id", "Name", "SortOrder" },
                values: new object[,]
                {
                    { "003715f4-b0ac-4d2f-8257-dd72631b4e79", "倒车灯", 11 },
                    { "021f6c80-334b-4aac-9d67-cbe5d0fa9244", "前雾灯", 3 },
                    { "07f3c786-4c84-4e6f-8f17-55b43960c387", "角灯", 7 },
                    { "3d4e3a1a-3c2c-47a6-943d-52f763b9273e", "昼间行驶灯", 4 },
                    { "6312b0a7-20e5-4e8a-883d-683b27cac8ec", "牌照灯", 14 },
                    { "68f62872-1f3e-4499-b50e-adb6e2661ffa", "后转向灯", 9 },
                    { "7fdce48a-99d5-4c6c-be5b-7630b3017bfd", "制动灯", 8 },
                    { "9fcc3052-160e-4122-b824-56f4e5dc693a", "前转向灯", 6 },
                    { "af5a16ab-de57-4473-b353-348a207a2d47", "前位置灯", 5 },
                    { "cc8cf0b5-6610-48a7-abf5-04afff058d36", "高位制动灯", 13 },
                    { "d06b6873-417b-4612-9e32-400afd33d14d", "远光", 1 },
                    { "e2864d8b-62d2-4600-8119-35f1f1173fd0", "后位置灯", 10 },
                    { "ee0b1621-f601-41c0-90b2-503e70f92c5b", "近光", 2 },
                    { "f4f0342a-fac3-4718-b648-34633c49b929", "后雾灯", 12 }
                });
        }
    }
}
