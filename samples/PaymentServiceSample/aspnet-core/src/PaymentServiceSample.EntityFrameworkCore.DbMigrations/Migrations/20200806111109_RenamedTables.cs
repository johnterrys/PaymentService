﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class RenamedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentServicePaymentItems_PaymentServicePayments_PaymentId",
                table: "PaymentServicePaymentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentServiceWeChatPayRefundRecords",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentServiceWeChatPayPaymentRecords",
                table: "PaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentServiceRefunds",
                table: "PaymentServiceRefunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentServicePayments",
                table: "PaymentServicePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentServicePaymentItems",
                table: "PaymentServicePaymentItems");

            migrationBuilder.RenameTable(
                name: "PaymentServiceWeChatPayRefundRecords",
                newName: "EasyAbpPaymentServiceWeChatPayRefundRecords");

            migrationBuilder.RenameTable(
                name: "PaymentServiceWeChatPayPaymentRecords",
                newName: "EasyAbpPaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.RenameTable(
                name: "PaymentServiceRefunds",
                newName: "EasyAbpPaymentServiceRefunds");

            migrationBuilder.RenameTable(
                name: "PaymentServicePayments",
                newName: "EasyAbpPaymentServicePayments");

            migrationBuilder.RenameTable(
                name: "PaymentServicePaymentItems",
                newName: "EasyAbpPaymentServicePaymentItems");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentServiceWeChatPayRefundRecords_PaymentId",
                table: "EasyAbpPaymentServiceWeChatPayRefundRecords",
                newName: "IX_EasyAbpPaymentServiceWeChatPayRefundRecords_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentServiceWeChatPayPaymentRecords_PaymentId",
                table: "EasyAbpPaymentServiceWeChatPayPaymentRecords",
                newName: "IX_EasyAbpPaymentServiceWeChatPayPaymentRecords_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentServicePaymentItems_PaymentId",
                table: "EasyAbpPaymentServicePaymentItems",
                newName: "IX_EasyAbpPaymentServicePaymentItems_PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpPaymentServiceWeChatPayRefundRecords",
                table: "EasyAbpPaymentServiceWeChatPayRefundRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpPaymentServiceWeChatPayPaymentRecords",
                table: "EasyAbpPaymentServiceWeChatPayPaymentRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpPaymentServiceRefunds",
                table: "EasyAbpPaymentServiceRefunds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpPaymentServicePayments",
                table: "EasyAbpPaymentServicePayments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpPaymentServicePaymentItems",
                table: "EasyAbpPaymentServicePaymentItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EasyAbpPaymentServicePaymentItems_EasyAbpPaymentServicePayments_PaymentId",
                table: "EasyAbpPaymentServicePaymentItems",
                column: "PaymentId",
                principalTable: "EasyAbpPaymentServicePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EasyAbpPaymentServicePaymentItems_EasyAbpPaymentServicePayments_PaymentId",
                table: "EasyAbpPaymentServicePaymentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpPaymentServiceWeChatPayRefundRecords",
                table: "EasyAbpPaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpPaymentServiceWeChatPayPaymentRecords",
                table: "EasyAbpPaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpPaymentServiceRefunds",
                table: "EasyAbpPaymentServiceRefunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpPaymentServicePayments",
                table: "EasyAbpPaymentServicePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpPaymentServicePaymentItems",
                table: "EasyAbpPaymentServicePaymentItems");

            migrationBuilder.RenameTable(
                name: "EasyAbpPaymentServiceWeChatPayRefundRecords",
                newName: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.RenameTable(
                name: "EasyAbpPaymentServiceWeChatPayPaymentRecords",
                newName: "PaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.RenameTable(
                name: "EasyAbpPaymentServiceRefunds",
                newName: "PaymentServiceRefunds");

            migrationBuilder.RenameTable(
                name: "EasyAbpPaymentServicePayments",
                newName: "PaymentServicePayments");

            migrationBuilder.RenameTable(
                name: "EasyAbpPaymentServicePaymentItems",
                newName: "PaymentServicePaymentItems");

            migrationBuilder.RenameIndex(
                name: "IX_EasyAbpPaymentServiceWeChatPayRefundRecords_PaymentId",
                table: "PaymentServiceWeChatPayRefundRecords",
                newName: "IX_PaymentServiceWeChatPayRefundRecords_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_EasyAbpPaymentServiceWeChatPayPaymentRecords_PaymentId",
                table: "PaymentServiceWeChatPayPaymentRecords",
                newName: "IX_PaymentServiceWeChatPayPaymentRecords_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_EasyAbpPaymentServicePaymentItems_PaymentId",
                table: "PaymentServicePaymentItems",
                newName: "IX_PaymentServicePaymentItems_PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentServiceWeChatPayRefundRecords",
                table: "PaymentServiceWeChatPayRefundRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentServiceWeChatPayPaymentRecords",
                table: "PaymentServiceWeChatPayPaymentRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentServiceRefunds",
                table: "PaymentServiceRefunds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentServicePayments",
                table: "PaymentServicePayments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentServicePaymentItems",
                table: "PaymentServicePaymentItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentServicePaymentItems_PaymentServicePayments_PaymentId",
                table: "PaymentServicePaymentItems",
                column: "PaymentId",
                principalTable: "PaymentServicePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
