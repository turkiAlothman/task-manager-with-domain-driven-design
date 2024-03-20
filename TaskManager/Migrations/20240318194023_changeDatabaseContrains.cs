using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class changeDatabaseContrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_tasks_taskId",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_tasks_taskId",
                table: "Activities",
                column: "taskId",
                principalTable: "tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_tasks_taskId",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_tasks_taskId",
                table: "Activities",
                column: "taskId",
                principalTable: "tasks",
                principalColumn: "Id");
        }
    }
}
