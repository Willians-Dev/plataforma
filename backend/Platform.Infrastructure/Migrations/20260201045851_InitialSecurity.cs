using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "seguridad");

            migrationBuilder.CreateTable(
                name: "modulos",
                schema: "seguridad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    icono = table.Column<string>(type: "text", nullable: false),
                    ruta = table.Column<string>(type: "text", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "seguridad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "seguridad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    apellido = table.Column<string>(type: "text", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rol_modulos",
                schema: "seguridad",
                columns: table => new
                {
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false),
                    modulo_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol_modulos", x => new { x.rol_id, x.modulo_id });
                    table.ForeignKey(
                        name: "FK_rol_modulos_modulos_modulo_id",
                        column: x => x.modulo_id,
                        principalSchema: "seguridad",
                        principalTable: "modulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rol_modulos_roles_rol_id",
                        column: x => x.rol_id,
                        principalSchema: "seguridad",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_roles",
                schema: "seguridad",
                columns: table => new
                {
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_roles", x => new { x.usuario_id, x.rol_id });
                    table.ForeignKey(
                        name: "FK_usuario_roles_roles_rol_id",
                        column: x => x.rol_id,
                        principalSchema: "seguridad",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_roles_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalSchema: "seguridad",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_modulos_codigo",
                schema: "seguridad",
                table: "modulos",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rol_modulos_modulo_id",
                schema: "seguridad",
                table: "rol_modulos",
                column: "modulo_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_nombre",
                schema: "seguridad",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_roles_rol_id",
                schema: "seguridad",
                table: "usuario_roles",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_usuario",
                schema: "seguridad",
                table: "usuarios",
                column: "usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rol_modulos",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "usuario_roles",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "modulos",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "seguridad");
        }
    }
}
