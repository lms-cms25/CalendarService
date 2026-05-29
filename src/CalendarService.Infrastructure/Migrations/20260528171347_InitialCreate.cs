using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classroom",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classroom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "student_courses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomEvent",
                columns: table => new
                {
                    ClassroomsId = table.Column<string>(type: "TEXT", nullable: false),
                    EventsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomEvent", x => new { x.ClassroomsId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_ClassroomEvent_Classroom_ClassroomsId",
                        column: x => x.ClassroomsId,
                        principalTable: "Classroom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomEvent_Event_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiveClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EventId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveClass_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomEvent_EventsId",
                table: "ClassroomEvent",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveClass_EventId",
                table: "LiveClass",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_courses_UserId",
                table: "student_courses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomEvent");

            migrationBuilder.DropTable(
                name: "LiveClass");

            migrationBuilder.DropTable(
                name: "student_courses");

            migrationBuilder.DropTable(
                name: "Classroom");

            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
