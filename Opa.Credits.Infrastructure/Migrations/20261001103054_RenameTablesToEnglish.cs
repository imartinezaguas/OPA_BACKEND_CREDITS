using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opa.Credits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_creditos_asociados_associateid",
                table: "creditos");

            migrationBuilder.DropForeignKey(
                name: "fk_historialcreditos_creditos_creditid",
                table: "historialcreditos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_historialcreditos",
                table: "historialcreditos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_creditos",
                table: "creditos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asociados",
                table: "asociados");

            migrationBuilder.RenameTable(
                name: "historialcreditos",
                newName: "credithistories");

            migrationBuilder.RenameTable(
                name: "creditos",
                newName: "credits");

            migrationBuilder.RenameTable(
                name: "asociados",
                newName: "associates");

            migrationBuilder.RenameIndex(
                name: "ix_historialcreditos_creditid",
                table: "credithistories",
                newName: "ix_credithistories_creditid");

            migrationBuilder.RenameIndex(
                name: "ix_creditos_creditnumber",
                table: "credits",
                newName: "ix_credits_creditnumber");

            migrationBuilder.RenameIndex(
                name: "ix_creditos_associateid",
                table: "credits",
                newName: "ix_credits_associateid");

            migrationBuilder.RenameIndex(
                name: "ix_asociados_identification",
                table: "associates",
                newName: "ix_associates_identification");

            migrationBuilder.AddPrimaryKey(
                name: "pk_credithistories",
                table: "credithistories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_credits",
                table: "credits",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_associates",
                table: "associates",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_credithistories_credits_creditid",
                table: "credithistories",
                column: "creditid",
                principalTable: "credits",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_credits_associates_associateid",
                table: "credits",
                column: "associateid",
                principalTable: "associates",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_credithistories_credits_creditid",
                table: "credithistories");

            migrationBuilder.DropForeignKey(
                name: "fk_credits_associates_associateid",
                table: "credits");

            migrationBuilder.DropPrimaryKey(
                name: "pk_credits",
                table: "credits");

            migrationBuilder.DropPrimaryKey(
                name: "pk_credithistories",
                table: "credithistories");

            migrationBuilder.DropPrimaryKey(
                name: "pk_associates",
                table: "associates");

            migrationBuilder.RenameTable(
                name: "credits",
                newName: "creditos");

            migrationBuilder.RenameTable(
                name: "credithistories",
                newName: "historialcreditos");

            migrationBuilder.RenameTable(
                name: "associates",
                newName: "asociados");

            migrationBuilder.RenameIndex(
                name: "ix_credits_creditnumber",
                table: "creditos",
                newName: "ix_creditos_creditnumber");

            migrationBuilder.RenameIndex(
                name: "ix_credits_associateid",
                table: "creditos",
                newName: "ix_creditos_associateid");

            migrationBuilder.RenameIndex(
                name: "ix_credithistories_creditid",
                table: "historialcreditos",
                newName: "ix_historialcreditos_creditid");

            migrationBuilder.RenameIndex(
                name: "ix_associates_identification",
                table: "asociados",
                newName: "ix_asociados_identification");

            migrationBuilder.AddPrimaryKey(
                name: "pk_creditos",
                table: "creditos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_historialcreditos",
                table: "historialcreditos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asociados",
                table: "asociados",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_creditos_asociados_associateid",
                table: "creditos",
                column: "associateid",
                principalTable: "asociados",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_historialcreditos_creditos_creditid",
                table: "historialcreditos",
                column: "creditid",
                principalTable: "creditos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
