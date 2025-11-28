using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorAppAccountManager.Migrations
{
    /// <inheritdoc />
    public partial class FixLampTemplateStaticIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "123afbd4-67a0-4744-aed7-76675d2b846a");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "500c1347-01c3-48a9-845b-d6ff57c3dcc1");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "571bbae7-17dd-4619-85f9-d6ed1a26c1b5");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "5ccf6dce-7b72-4adb-bb1f-32bd7fef4c97");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "8421c8a4-f13c-47f1-861f-fb2ea063d985");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "b359277f-c794-476b-b714-fbdaaadb1eba");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "b4020f11-18d9-4a23-bb87-39213388c016");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "b9618d88-5fdf-4a4e-b506-43478aae618b");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "c3f05435-e5b4-46e5-af47-b7815b75105f");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "d3460946-44c0-4c0b-b5df-abea4dbb4cc9");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "d9728219-9010-474d-b999-e9b93f837d1c");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "e36c9118-ce9a-4852-b09f-f96f30121c14");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "e7fa78b3-2b14-43ae-8103-6bdd101db365");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "f8a0e833-be53-4aa5-9c41-bc13335ed611");

            migrationBuilder.InsertData(
                table: "LampTemplates",
                columns: new[] { "Id", "Name", "SortOrder" },
                values: new object[,]
                {
                    { "1", "Զ��", 1 },
                    { "10", "��λ�õ�", 10 },
                    { "11", "������", 11 },
                    { "12", "������", 12 },
                    { "13", "��λ�ƶ���", 13 },
                    { "14", "���յ�", 14 },
                    { "2", "����", 2 },
                    { "3", "ǰ����", 3 },
                    { "4", "�����ʻ��", 4 },
                    { "5", "ǰλ�õ�", 5 },
                    { "6", "ǰת���", 6 },
                    { "7", "�ǵ�", 7 },
                    { "8", "�ƶ���", 8 },
                    { "9", "��ת���", 9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.InsertData(
                table: "LampTemplates",
                columns: new[] { "Id", "Name", "SortOrder" },
                values: new object[,]
                {
                    { "123afbd4-67a0-4744-aed7-76675d2b846a", "ǰת���", 6 },
                    { "500c1347-01c3-48a9-845b-d6ff57c3dcc1", "������", 11 },
                    { "571bbae7-17dd-4619-85f9-d6ed1a26c1b5", "�ǵ�", 7 },
                    { "5ccf6dce-7b72-4adb-bb1f-32bd7fef4c97", "�ƶ���", 8 },
                    { "8421c8a4-f13c-47f1-861f-fb2ea063d985", "���յ�", 14 },
                    { "b359277f-c794-476b-b714-fbdaaadb1eba", "��λ�õ�", 10 },
                    { "b4020f11-18d9-4a23-bb87-39213388c016", "�����ʻ��", 4 },
                    { "b9618d88-5fdf-4a4e-b506-43478aae618b", "����", 2 },
                    { "c3f05435-e5b4-46e5-af47-b7815b75105f", "ǰλ�õ�", 5 },
                    { "d3460946-44c0-4c0b-b5df-abea4dbb4cc9", "Զ��", 1 },
                    { "d9728219-9010-474d-b999-e9b93f837d1c", "������", 12 },
                    { "e36c9118-ce9a-4852-b09f-f96f30121c14", "��λ�ƶ���", 13 },
                    { "e7fa78b3-2b14-43ae-8103-6bdd101db365", "��ת���", 9 },
                    { "f8a0e833-be53-4aa5-9c41-bc13335ed611", "ǰ����", 3 }
                });
        }
    }
}
