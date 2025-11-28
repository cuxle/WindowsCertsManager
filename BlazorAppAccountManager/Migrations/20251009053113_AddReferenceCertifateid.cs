using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorAppAccountManager.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceCertifateid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "0cf3e608-37a9-41fe-ab86-e34adb4e60a9");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "2a42acec-88c5-4060-a5fb-0a51d3ef5130");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "2db40052-9759-4bad-94d5-9bce431220fd");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "32df7824-0175-4a33-be5e-e983f0978f4b");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "417eb786-21b0-4b64-8277-0f8e9468f951");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "47be5b72-b281-43fe-967e-7be285c5e33b");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "52d5c430-0f16-405e-81b1-90ce298df0ad");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "59e4b053-52c0-4c9b-9cc4-dcbfd7cbfe23");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "5d3d151d-3ea6-414f-9201-72610c82cf80");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "a0cb51f8-947d-42cf-be75-0e464eee780d");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "a10882a9-8285-4bc4-8cda-4391ab2665ab");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "b5ab7609-74d8-4158-aa26-a0ab14d295f0");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "c03bdac0-6ceb-489d-83c8-8fc17e1273b1");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "cf61be4f-f726-4467-b921-21e08e92ab2e");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCertifateId",
                table: "Certificates",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ReferenceCertifateId",
                table: "Certificates");

            migrationBuilder.InsertData(
                table: "LampTemplates",
                columns: new[] { "Id", "Name", "SortOrder" },
                values: new object[,]
                {
                    { "0cf3e608-37a9-41fe-ab86-e34adb4e60a9", "制动灯", 8 },
                    { "2a42acec-88c5-4060-a5fb-0a51d3ef5130", "后位置灯", 10 },
                    { "2db40052-9759-4bad-94d5-9bce431220fd", "角灯", 7 },
                    { "32df7824-0175-4a33-be5e-e983f0978f4b", "前转向灯", 6 },
                    { "417eb786-21b0-4b64-8277-0f8e9468f951", "昼间行驶灯", 4 },
                    { "47be5b72-b281-43fe-967e-7be285c5e33b", "后转向灯", 9 },
                    { "52d5c430-0f16-405e-81b1-90ce298df0ad", "高位制动灯", 13 },
                    { "59e4b053-52c0-4c9b-9cc4-dcbfd7cbfe23", "远光", 1 },
                    { "5d3d151d-3ea6-414f-9201-72610c82cf80", "倒车灯", 11 },
                    { "a0cb51f8-947d-42cf-be75-0e464eee780d", "前雾灯", 3 },
                    { "a10882a9-8285-4bc4-8cda-4391ab2665ab", "前位置灯", 5 },
                    { "b5ab7609-74d8-4158-aa26-a0ab14d295f0", "后雾灯", 12 },
                    { "c03bdac0-6ceb-489d-83c8-8fc17e1273b1", "牌照灯", 14 },
                    { "cf61be4f-f726-4467-b921-21e08e92ab2e", "近光", 2 }
                });
        }
    }
}
