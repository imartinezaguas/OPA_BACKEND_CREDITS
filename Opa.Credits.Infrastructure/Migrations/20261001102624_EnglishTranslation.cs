using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opa.Credits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnglishTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_creditos_asociados_asociadoid",
                table: "creditos");

            migrationBuilder.DropForeignKey(
                name: "fk_historialcreditos_creditos_creditoid",
                table: "historialcreditos");

            migrationBuilder.DropIndex(
                name: "ix_creditos_numerocredito",
                table: "creditos");

            migrationBuilder.RenameColumn(
                name: "usuario",
                table: "historialcreditos",
                newName: "user");

            migrationBuilder.RenameColumn(
                name: "observacion",
                table: "historialcreditos",
                newName: "observation");

            migrationBuilder.RenameColumn(
                name: "fechacambio",
                table: "historialcreditos",
                newName: "changedate");

            migrationBuilder.RenameColumn(
                name: "estadonuevo",
                table: "historialcreditos",
                newName: "previousstatus");

            migrationBuilder.RenameColumn(
                name: "estadoanterior",
                table: "historialcreditos",
                newName: "newstatus");

            migrationBuilder.RenameColumn(
                name: "creditoid",
                table: "historialcreditos",
                newName: "creditid");

            migrationBuilder.RenameIndex(
                name: "ix_historialcreditos_creditoid",
                table: "historialcreditos",
                newName: "ix_historialcreditos_creditid");

            migrationBuilder.RenameColumn(
                name: "valorsolicitado",
                table: "creditos",
                newName: "requestedvalue");

            migrationBuilder.RenameColumn(
                name: "tipocredito",
                table: "creditos",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "tasainteres",
                table: "creditos",
                newName: "interestrate");

            migrationBuilder.RenameColumn(
                name: "numerocuotas",
                table: "creditos",
                newName: "numberofinstallments");

            migrationBuilder.RenameColumn(
                name: "numerocredito",
                table: "creditos",
                newName: "paymentmethod");

            migrationBuilder.RenameColumn(
                name: "formapago",
                table: "creditos",
                newName: "credittype");

            migrationBuilder.RenameColumn(
                name: "fechasolicitud",
                table: "creditos",
                newName: "updatedate");

            migrationBuilder.RenameColumn(
                name: "fechaactualizacion",
                table: "creditos",
                newName: "requestdate");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "creditos",
                newName: "creditnumber");

            migrationBuilder.RenameColumn(
                name: "asociadoid",
                table: "creditos",
                newName: "associateid");

            migrationBuilder.RenameIndex(
                name: "ix_creditos_asociadoid",
                table: "creditos",
                newName: "ix_creditos_associateid");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "asociados",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "identificacion",
                table: "asociados",
                newName: "identification");

            migrationBuilder.RenameIndex(
                name: "ix_asociados_identificacion",
                table: "asociados",
                newName: "ix_asociados_identification");

            migrationBuilder.CreateIndex(
                name: "ix_creditos_creditnumber",
                table: "creditos",
                column: "creditnumber",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_creditos_asociados_associateid",
                table: "creditos");

            migrationBuilder.DropForeignKey(
                name: "fk_historialcreditos_creditos_creditid",
                table: "historialcreditos");

            migrationBuilder.DropIndex(
                name: "ix_creditos_creditnumber",
                table: "creditos");

            migrationBuilder.RenameColumn(
                name: "user",
                table: "historialcreditos",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "previousstatus",
                table: "historialcreditos",
                newName: "estadonuevo");

            migrationBuilder.RenameColumn(
                name: "observation",
                table: "historialcreditos",
                newName: "observacion");

            migrationBuilder.RenameColumn(
                name: "newstatus",
                table: "historialcreditos",
                newName: "estadoanterior");

            migrationBuilder.RenameColumn(
                name: "creditid",
                table: "historialcreditos",
                newName: "creditoid");

            migrationBuilder.RenameColumn(
                name: "changedate",
                table: "historialcreditos",
                newName: "fechacambio");

            migrationBuilder.RenameIndex(
                name: "ix_historialcreditos_creditid",
                table: "historialcreditos",
                newName: "ix_historialcreditos_creditoid");

            migrationBuilder.RenameColumn(
                name: "updatedate",
                table: "creditos",
                newName: "fechasolicitud");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "creditos",
                newName: "tipocredito");

            migrationBuilder.RenameColumn(
                name: "requestedvalue",
                table: "creditos",
                newName: "valorsolicitado");

            migrationBuilder.RenameColumn(
                name: "requestdate",
                table: "creditos",
                newName: "fechaactualizacion");

            migrationBuilder.RenameColumn(
                name: "paymentmethod",
                table: "creditos",
                newName: "numerocredito");

            migrationBuilder.RenameColumn(
                name: "numberofinstallments",
                table: "creditos",
                newName: "numerocuotas");

            migrationBuilder.RenameColumn(
                name: "interestrate",
                table: "creditos",
                newName: "tasainteres");

            migrationBuilder.RenameColumn(
                name: "credittype",
                table: "creditos",
                newName: "formapago");

            migrationBuilder.RenameColumn(
                name: "creditnumber",
                table: "creditos",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "associateid",
                table: "creditos",
                newName: "asociadoid");

            migrationBuilder.RenameIndex(
                name: "ix_creditos_associateid",
                table: "creditos",
                newName: "ix_creditos_asociadoid");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "asociados",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "identification",
                table: "asociados",
                newName: "identificacion");

            migrationBuilder.RenameIndex(
                name: "ix_asociados_identification",
                table: "asociados",
                newName: "ix_asociados_identificacion");

            migrationBuilder.CreateIndex(
                name: "ix_creditos_numerocredito",
                table: "creditos",
                column: "numerocredito",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_creditos_asociados_asociadoid",
                table: "creditos",
                column: "asociadoid",
                principalTable: "asociados",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_historialcreditos_creditos_creditoid",
                table: "historialcreditos",
                column: "creditoid",
                principalTable: "creditos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
