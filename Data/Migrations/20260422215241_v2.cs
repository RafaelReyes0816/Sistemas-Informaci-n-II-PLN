using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AlmacenMis.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Productos",
                table: "Productos");

            migrationBuilder.RenameTable(
                name: "Productos",
                newName: "Producto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Producto",
                table: "Producto",
                column: "id_Producto");

            migrationBuilder.CreateTable(
                name: "Almacen",
                columns: table => new
                {
                    almacen_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Código = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Almacen", x => x.almacen_id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    id_proveedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Código = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.id_proveedor);
                });

            migrationBuilder.CreateTable(
                name: "Producto_Almacen",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    producto_id = table.Column<int>(type: "integer", nullable: false),
                    almacen_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto_Almacen", x => x.id);
                    table.ForeignKey(
                        name: "FK_Producto_Almacen_Almacen_almacen_id",
                        column: x => x.almacen_id,
                        principalTable: "Almacen",
                        principalColumn: "almacen_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Almacen_Producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "Producto",
                        principalColumn: "id_Producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    producto_id = table.Column<int>(type: "integer", nullable: false),
                    almacen_id = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.id);
                    table.ForeignKey(
                        name: "FK_Stock_Almacen_almacen_id",
                        column: x => x.almacen_id,
                        principalTable: "Almacen",
                        principalColumn: "almacen_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stock_Producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "Producto",
                        principalColumn: "id_Producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producto_Proveedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    producto_id = table.Column<int>(type: "integer", nullable: false),
                    proveedor_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto_Proveedor", x => x.id);
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor_Producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "Producto",
                        principalColumn: "id_Producto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor_Proveedor_proveedor_id",
                        column: x => x.proveedor_id,
                        principalTable: "Proveedor",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producto_Proveedor_Almacen",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    producto_id = table.Column<int>(type: "integer", nullable: false),
                    proveedor_id = table.Column<int>(type: "integer", nullable: false),
                    almacen_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto_Proveedor_Almacen", x => x.id);
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor_Almacen_Almacen_almacen_id",
                        column: x => x.almacen_id,
                        principalTable: "Almacen",
                        principalColumn: "almacen_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor_Almacen_Producto_producto_id",
                        column: x => x.producto_id,
                        principalTable: "Producto",
                        principalColumn: "id_Producto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor_Almacen_Proveedor_proveedor_id",
                        column: x => x.proveedor_id,
                        principalTable: "Proveedor",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Almacen_almacen_id",
                table: "Producto_Almacen",
                column: "almacen_id");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Almacen_producto_id",
                table: "Producto_Almacen",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Proveedor_producto_id",
                table: "Producto_Proveedor",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Proveedor_proveedor_id",
                table: "Producto_Proveedor",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Proveedor_Almacen_almacen_id",
                table: "Producto_Proveedor_Almacen",
                column: "almacen_id");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Proveedor_Almacen_producto_id",
                table: "Producto_Proveedor_Almacen",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Proveedor_Almacen_proveedor_id",
                table: "Producto_Proveedor_Almacen",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_almacen_id",
                table: "Stock",
                column: "almacen_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_producto_id",
                table: "Stock",
                column: "producto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producto_Almacen");

            migrationBuilder.DropTable(
                name: "Producto_Proveedor");

            migrationBuilder.DropTable(
                name: "Producto_Proveedor_Almacen");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "Almacen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Producto",
                table: "Producto");

            migrationBuilder.RenameTable(
                name: "Producto",
                newName: "Productos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productos",
                table: "Productos",
                column: "id_Producto");
        }
    }
}
