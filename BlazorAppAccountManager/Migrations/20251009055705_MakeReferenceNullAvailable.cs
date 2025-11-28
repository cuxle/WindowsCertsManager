using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorAppAccountManager.Migrations
{
    /// <inheritdoc />
    public partial class MakeReferenceNullAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceSupplierId",
                table: "Suppliers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceCertifateId",
                table: "Certificates",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "ReferenceSupplierId",
                keyValue: null,
                column: "ReferenceSupplierId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceSupplierId",
                table: "Suppliers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Certificates",
                keyColumn: "ReferenceCertifateId",
                keyValue: null,
                column: "ReferenceCertifateId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceCertifateId",
                table: "Certificates",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
