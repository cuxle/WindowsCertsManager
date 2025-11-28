using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAppAccountManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLampTemplateChineseNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "1",
                column: "Name",
                value: "远光");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "10",
                column: "Name",
                value: "后位置灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "11",
                column: "Name",
                value: "后雾灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "12",
                column: "Name",
                value: "倒车灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "13",
                column: "Name",
                value: "后位置制动灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "14",
                column: "Name",
                value: "牌照灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "2",
                column: "Name",
                value: "近光");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "3",
                column: "Name",
                value: "前雾灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "4",
                column: "Name",
                value: "前转向信号灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "5",
                column: "Name",
                value: "前位置灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "6",
                column: "Name",
                value: "前转向灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "7",
                column: "Name",
                value: "侧灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "8",
                column: "Name",
                value: "制动灯");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "9",
                column: "Name",
                value: "后转向灯");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "1",
                column: "Name",
                value: "Զ��");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "10",
                column: "Name",
                value: "��λ�õ�");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "11",
                column: "Name",
                value: "������");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "12",
                column: "Name",
                value: "������");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "13",
                column: "Name",
                value: "��λ�ƶ���");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "14",
                column: "Name",
                value: "���յ�");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "2",
                column: "Name",
                value: "����");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "3",
                column: "Name",
                value: "ǰ����");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "4",
                column: "Name",
                value: "�����ʻ��");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "5",
                column: "Name",
                value: "ǰλ�õ�");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "6",
                column: "Name",
                value: "ǰת���");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "7",
                column: "Name",
                value: "�ǵ�");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "8",
                column: "Name",
                value: "�ƶ���");

            migrationBuilder.UpdateData(
                table: "LampTemplates",
                keyColumn: "Id",
                keyValue: "9",
                column: "Name",
                value: "��ת���");
        }
    }
}
