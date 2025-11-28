using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorAppAccountManager.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificateRecordReferenceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "2060e2c7-206e-42b6-b06d-b10445538766");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "21396db1-b58e-487c-87a3-5cb236886c74");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "39b662a7-c881-4987-a586-c8b961019e7a");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "3ccfa2d1-af6d-444c-a29d-18e7afd5b699");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "402ff923-8511-4540-8d10-303159ab2c49");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "44d5657c-76ec-4f78-bf92-69884bb68c1f");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "4771daac-aa51-4fb3-84ca-914c102f8eaf");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "565e672f-816e-440c-9ed6-c1ef13daf926");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "725360b6-d583-422b-b9b2-ff99516b1e60");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "7dff7a10-ce2a-4aa2-a820-62ecd29498ff");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "8ae1955c-ee9d-45c4-97d7-7a37cebcc9d6");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "b693de39-9cca-4820-8979-2de7a5d4cc8f");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "da2944d3-4670-41b4-8202-ee160507f8ec");

            migrationBuilder.DeleteData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "f139c4dd-4f36-4e28-8517-f1aa0657082e");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCertificateRecordId",
                table: "CertificateRecords",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "UsingReference",
                table: "CertificateRecords",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRecords_ReferenceCertificateRecordId",
                table: "CertificateRecords",
                column: "ReferenceCertificateRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateRecords_CertificateRecords_ReferenceCertificateRe~",
                table: "CertificateRecords",
                column: "ReferenceCertificateRecordId",
                principalTable: "CertificateRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificateRecords_CertificateRecords_ReferenceCertificateRe~",
                table: "CertificateRecords");

            migrationBuilder.DropIndex(
                name: "IX_CertificateRecords_ReferenceCertificateRecordId",
                table: "CertificateRecords");

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

            migrationBuilder.DropColumn(
                name: "ReferenceCertificateRecordId",
                table: "CertificateRecords");

            migrationBuilder.DropColumn(
                name: "UsingReference",
                table: "CertificateRecords");

            migrationBuilder.InsertData(
                table: "LampTemplates",
                columns: new[] { "Id", "Name", "SortOrder" },
                values: new object[,]
                {
                    { "2060e2c7-206e-42b6-b06d-b10445538766", "昼间行驶灯", 4 },
                    { "21396db1-b58e-487c-87a3-5cb236886c74", "制动灯", 8 },
                    { "39b662a7-c881-4987-a586-c8b961019e7a", "前位置灯", 5 },
                    { "3ccfa2d1-af6d-444c-a29d-18e7afd5b699", "倒车灯", 11 },
                    { "402ff923-8511-4540-8d10-303159ab2c49", "后位置灯", 10 },
                    { "44d5657c-76ec-4f78-bf92-69884bb68c1f", "后转向灯", 9 },
                    { "4771daac-aa51-4fb3-84ca-914c102f8eaf", "远光", 1 },
                    { "565e672f-816e-440c-9ed6-c1ef13daf926", "前转向灯", 6 },
                    { "725360b6-d583-422b-b9b2-ff99516b1e60", "近光", 2 },
                    { "7dff7a10-ce2a-4aa2-a820-62ecd29498ff", "前雾灯", 3 },
                    { "8ae1955c-ee9d-45c4-97d7-7a37cebcc9d6", "牌照灯", 14 },
                    { "b693de39-9cca-4820-8979-2de7a5d4cc8f", "高位制动灯", 13 },
                    { "da2944d3-4670-41b4-8202-ee160507f8ec", "后雾灯", 12 },
                    { "f139c4dd-4f36-4e28-8517-f1aa0657082e", "角灯", 7 }
                });
        }
    }
}
