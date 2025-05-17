using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meal_menu_api.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvitedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvitedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupInvitations_AspNetUsers_InvitedByUserId",
                        column: x => x.InvitedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupInvitations_AspNetUsers_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupInvitations_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvitations_GroupId",
                table: "GroupInvitations",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvitations_InvitedByUserId",
                table: "GroupInvitations",
                column: "InvitedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvitations_InvitedUserId",
                table: "GroupInvitations",
                column: "InvitedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupInvitations");
        }
    }
}
