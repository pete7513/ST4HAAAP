using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreEFTest.Migrations
{
    public partial class InitCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPR = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    Name = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Lastname = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Age = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    MobilNummer = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    Adress = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    zipcode = table.Column<int>(type: "int", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "StaffLogin",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Password = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    StaffStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffLogin", x => x.StaffID);
                });

            migrationBuilder.CreateTable(
                name: "EarCast",
                columns: table => new
                {
                    EarCastID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EarSide = table.Column<int>(type: "int", nullable: false),
                    CastDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarCast", x => x.EarCastID);
                    table.ForeignKey(
                        name: "FK_EarCast_Patient_PatientFK",
                        column: x => x.PatientFK,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralSpecs",
                columns: table => new
                {
                    HAGeneralSpecID = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    EarSide = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientFK = table.Column<int>(type: "int", nullable: false),
                    StaffLoginFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSpecs", x => x.HAGeneralSpecID);
                    table.ForeignKey(
                        name: "FK_GeneralSpecs_Patient_PatientFK",
                        column: x => x.PatientFK,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralSpecs_StaffLogin_StaffLoginFK",
                        column: x => x.StaffLoginFK,
                        principalTable: "StaffLogin",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TecnicalSpecs",
                columns: table => new
                {
                    HATechinalSpecID = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EarSide = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Printed = table.Column<bool>(type: "bit", nullable: false),
                    StaffLoginFK = table.Column<int>(type: "int", nullable: false),
                    PatientFK = table.Column<int>(type: "int", nullable: false),
                    GeneralSpecFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TecnicalSpecs", x => x.HATechinalSpecID);
                    table.ForeignKey(
                        name: "FK_TecnicalSpecs_GeneralSpecs_GeneralSpecFK",
                        column: x => x.GeneralSpecFK,
                        principalTable: "GeneralSpecs",
                        principalColumn: "HAGeneralSpecID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TecnicalSpecs_Patient_PatientFK",
                        column: x => x.PatientFK,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TecnicalSpecs_StaffLogin_StaffLoginFK",
                        column: x => x.StaffLoginFK,
                        principalTable: "StaffLogin",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RawEarPrints",
                columns: table => new
                {
                    EarPrintID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EarSide = table.Column<int>(type: "int", nullable: false),
                    PrintDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StaffLoginFK = table.Column<int>(type: "int", nullable: false),
                    TecnicalSpecFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawEarPrints", x => x.EarPrintID);
                    table.ForeignKey(
                        name: "FK_RawEarPrints_StaffLogin_StaffLoginFK",
                        column: x => x.StaffLoginFK,
                        principalTable: "StaffLogin",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RawEarPrints_TecnicalSpecs_TecnicalSpecFK",
                        column: x => x.TecnicalSpecFK,
                        principalTable: "TecnicalSpecs",
                        principalColumn: "HATechinalSpecID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RawEarScans",
                columns: table => new
                {
                    ScanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scan = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EarSide = table.Column<int>(type: "int", nullable: false),
                    ScanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StaffLoginFK = table.Column<int>(type: "int", nullable: false),
                    TecnicalSpecFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawEarScans", x => x.ScanID);
                    table.ForeignKey(
                        name: "FK_RawEarScans_StaffLogin_StaffLoginFK",
                        column: x => x.StaffLoginFK,
                        principalTable: "StaffLogin",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RawEarScans_TecnicalSpecs_TecnicalSpecFK",
                        column: x => x.TecnicalSpecFK,
                        principalTable: "TecnicalSpecs",
                        principalColumn: "HATechinalSpecID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EarCast_PatientFK",
                table: "EarCast",
                column: "PatientFK");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralSpecs_PatientFK",
                table: "GeneralSpecs",
                column: "PatientFK");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralSpecs_StaffLoginFK",
                table: "GeneralSpecs",
                column: "StaffLoginFK");

            migrationBuilder.CreateIndex(
                name: "IX_RawEarPrints_StaffLoginFK",
                table: "RawEarPrints",
                column: "StaffLoginFK");

            migrationBuilder.CreateIndex(
                name: "IX_RawEarPrints_TecnicalSpecFK",
                table: "RawEarPrints",
                column: "TecnicalSpecFK");

            migrationBuilder.CreateIndex(
                name: "IX_RawEarScans_StaffLoginFK",
                table: "RawEarScans",
                column: "StaffLoginFK");

            migrationBuilder.CreateIndex(
                name: "IX_RawEarScans_TecnicalSpecFK",
                table: "RawEarScans",
                column: "TecnicalSpecFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TecnicalSpecs_GeneralSpecFK",
                table: "TecnicalSpecs",
                column: "GeneralSpecFK");

            migrationBuilder.CreateIndex(
                name: "IX_TecnicalSpecs_PatientFK",
                table: "TecnicalSpecs",
                column: "PatientFK");

            migrationBuilder.CreateIndex(
                name: "IX_TecnicalSpecs_StaffLoginFK",
                table: "TecnicalSpecs",
                column: "StaffLoginFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EarCast");

            migrationBuilder.DropTable(
                name: "RawEarPrints");

            migrationBuilder.DropTable(
                name: "RawEarScans");

            migrationBuilder.DropTable(
                name: "TecnicalSpecs");

            migrationBuilder.DropTable(
                name: "GeneralSpecs");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "StaffLogin");
        }
    }
}
