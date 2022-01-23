using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace s3717205_a2.Migrations
{
    public partial class AddCheckConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Transaction_CH_Transaction_Amount",
                table: "Transaction",
                sql: "Amount > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Transaction_CH_Transaction_TransactionType",
                table: "Transaction",
                sql: "TransactionType in ('D', 'W', 'T', 'S', 'B')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payee_CH_Payee_Address",
                table: "Payee",
                sql: "len(Address) <= 50");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payee_CH_Payee_Name",
                table: "Payee",
                sql: "len(Name) <= 50");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payee_CH_Payee_State",
                table: "Payee",
                sql: "State in ('VIC', 'SA', 'WA', 'QLD', 'TAS', 'NT', 'NSW', 'ACT')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payee_CH_Payee_Suburb",
                table: "Payee",
                sql: "len(Suburb) <= 50");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Login_CH_Login_LoginID",
                table: "Login",
                sql: "len(LoginID) = 8");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Login_CH_Login_PasswordHash",
                table: "Login",
                sql: "len(PasswordHash) = 64");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Customer_CH_Customer_CustomerID",
                table: "Customer",
                sql: "len(CustomerID) = 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Customer_CH_Customer_Name",
                table: "Customer",
                sql: "len(Name) <= 50");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BillPay_CH_BillPay_Amount",
                table: "BillPay",
                sql: "Amount > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BillPay_CH_BillPay_Period",
                table: "BillPay",
                sql: "Period in ('O', 'M')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Account_CH_Account_AccountNumber",
                table: "Account",
                sql: "len(AccountNumber) = 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Account_CH_Account_AccountType",
                table: "Account",
                sql: "AccountType in ('C', 'S')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Account_CH_Account_Balance",
                table: "Account",
                sql: "Balance >= 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Transaction_CH_Transaction_Amount",
                table: "Transaction");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Transaction_CH_Transaction_TransactionType",
                table: "Transaction");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payee_CH_Payee_Address",
                table: "Payee");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payee_CH_Payee_Name",
                table: "Payee");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payee_CH_Payee_State",
                table: "Payee");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payee_CH_Payee_Suburb",
                table: "Payee");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Login_CH_Login_LoginID",
                table: "Login");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Login_CH_Login_PasswordHash",
                table: "Login");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Customer_CH_Customer_CustomerID",
                table: "Customer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Customer_CH_Customer_Name",
                table: "Customer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BillPay_CH_BillPay_Amount",
                table: "BillPay");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BillPay_CH_BillPay_Period",
                table: "BillPay");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Account_CH_Account_AccountNumber",
                table: "Account");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Account_CH_Account_AccountType",
                table: "Account");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Account_CH_Account_Balance",
                table: "Account");
        }
    }
}
