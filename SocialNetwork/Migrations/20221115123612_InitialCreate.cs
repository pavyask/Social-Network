using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Login);
                });

            migrationBuilder.CreateTable(
                name: "UserFriendRelation",
                columns: table => new
                {
                    UserLogin = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FriendLogin = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriendRelation", x => new { x.UserLogin, x.FriendLogin });
                    table.ForeignKey(
                        name: "FK_UserFriendRelation_User_FriendLogin",
                        column: x => x.FriendLogin,
                        principalTable: "User",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFriendRelation_User_UserLogin",
                        column: x => x.UserLogin,
                        principalTable: "User",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFriendRelation_FriendLogin",
                table: "UserFriendRelation",
                column: "FriendLogin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFriendRelation");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
