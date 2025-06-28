using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.API.Persistance.Migrations
{
	/// <inheritdoc />
	public partial class afterDropping : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					IsDefault = table.Column<bool>(type: "bit", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
					AccessFailedCount = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Polls",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Summary = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
					IsPublished = table.Column<bool>(type: "bit", nullable: false),
					StartsAt = table.Column<DateOnly>(type: "date", nullable: false),
					EndsAt = table.Column<DateOnly>(type: "date", nullable: false),
					CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Polls", x => x.Id);
					table.ForeignKey(
						name: "FK_Polls_AspNetUsers_CreatedById",
						column: x => x.CreatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Polls_AspNetUsers_UpdatedById",
						column: x => x.UpdatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "RefreshTokens",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RefreshTokens", x => new { x.UserId, x.Id });
					table.ForeignKey(
						name: "FK_RefreshTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Questions",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					PollId = table.Column<int>(type: "int", nullable: false),
					CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Questions", x => x.Id);
					table.ForeignKey(
						name: "FK_Questions_AspNetUsers_CreatedById",
						column: x => x.CreatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Questions_AspNetUsers_UpdatedById",
						column: x => x.UpdatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Questions_Polls_PollId",
						column: x => x.PollId,
						principalTable: "Polls",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Votes",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					PollId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Votes", x => x.Id);
					table.ForeignKey(
						name: "FK_Votes_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Votes_Polls_PollId",
						column: x => x.PollId,
						principalTable: "Polls",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Answers",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					QuestionId = table.Column<int>(type: "int", nullable: false),
					CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Answers", x => x.Id);
					table.ForeignKey(
						name: "FK_Answers_AspNetUsers_CreatedById",
						column: x => x.CreatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Answers_AspNetUsers_UpdatedById",
						column: x => x.UpdatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Answers_Questions_QuestionId",
						column: x => x.QuestionId,
						principalTable: "Questions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "VoteAnswers",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					VoteId = table.Column<int>(type: "int", nullable: false),
					QuestionId = table.Column<int>(type: "int", nullable: false),
					AnswerId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_VoteAnswers", x => x.Id);
					table.ForeignKey(
						name: "FK_VoteAnswers_Answers_AnswerId",
						column: x => x.AnswerId,
						principalTable: "Answers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_VoteAnswers_Questions_QuestionId",
						column: x => x.QuestionId,
						principalTable: "Questions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_VoteAnswers_Votes_VoteId",
						column: x => x.VoteId,
						principalTable: "Votes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
				values: new object[,]
				{
					{ "01979187-c392-77be-89d1-ffe5c2623fae", "01979187-c392-77be-89d1-ffe6910b4102", false, false, "Admin", "ADMIN" },
					{ "01979187-c392-77be-89d1-ffe7d2df25d4", "01979187-c392-77be-89d1-ffe8850dfea6", true, false, "Member", "MEMBER" }
				});

			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
				values: new object[] { "01978f6e-267c-7c96-b179-d2e08d73f69b", 0, "01078f6e-267c-7c96-b179-d2e127b3bb2c", "Admin@survey-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEOm19rPH9+gX4oopeWRXkyo+gPyFA3OBPZVkNori56HqKq38sCBNntaiBa7n8sADpA==", null, false, "35744942C16C49B9B2300A03094FAF85", false, "Admin@survey-basket.com" });

			migrationBuilder.InsertData(
				table: "AspNetRoleClaims",
				columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
				values: new object[,]
				{
					{ 1, "permission", "polls:read", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 2, "permission", "polls:add", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 3, "permission", "polls:update", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 4, "permission", "polls:delete", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 5, "permission", "questions:read", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 6, "permission", "questions:add", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 7, "permission", "questions:update", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 8, "permission", "users:read", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 9, "permission", "users:add", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 10, "permission", "users:update", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 11, "permission", "users:delete", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 12, "permission", "roles:read", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 13, "permission", "roles:add", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 14, "permission", "roles:update", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 15, "permission", "roles:delete", "01979187-c392-77be-89d1-ffe5c2623fae" },
					{ 16, "permission", "results:read", "01979187-c392-77be-89d1-ffe5c2623fae" }
				});

			migrationBuilder.InsertData(
				table: "AspNetUserRoles",
				columns: new[] { "RoleId", "UserId" },
				values: new object[] { "01979187-c392-77be-89d1-ffe5c2623fae", "01978f6e-267c-7c96-b179-d2e08d73f69b" });

			migrationBuilder.CreateIndex(
				name: "IX_Answers_CreatedById",
				table: "Answers",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Answers_QuestionId_Content",
				table: "Answers",
				columns: new[] { "QuestionId", "Content" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Answers_UpdatedById",
				table: "Answers",
				column: "UpdatedById");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_Polls_CreatedById",
				table: "Polls",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Polls_Title",
				table: "Polls",
				column: "Title",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Polls_UpdatedById",
				table: "Polls",
				column: "UpdatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Questions_CreatedById",
				table: "Questions",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Questions_PollId_Content",
				table: "Questions",
				columns: new[] { "PollId", "Content" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Questions_UpdatedById",
				table: "Questions",
				column: "UpdatedById");

			migrationBuilder.CreateIndex(
				name: "IX_VoteAnswers_AnswerId",
				table: "VoteAnswers",
				column: "AnswerId");

			migrationBuilder.CreateIndex(
				name: "IX_VoteAnswers_QuestionId",
				table: "VoteAnswers",
				column: "QuestionId");

			migrationBuilder.CreateIndex(
				name: "IX_VoteAnswers_VoteId_QuestionId",
				table: "VoteAnswers",
				columns: new[] { "VoteId", "QuestionId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Votes_PollId_UserId",
				table: "Votes",
				columns: new[] { "PollId", "UserId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Votes_UserId",
				table: "Votes",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AspNetRoleClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserLogins");

			migrationBuilder.DropTable(
				name: "AspNetUserRoles");

			migrationBuilder.DropTable(
				name: "AspNetUserTokens");

			migrationBuilder.DropTable(
				name: "RefreshTokens");

			migrationBuilder.DropTable(
				name: "VoteAnswers");

			migrationBuilder.DropTable(
				name: "AspNetRoles");

			migrationBuilder.DropTable(
				name: "Answers");

			migrationBuilder.DropTable(
				name: "Votes");

			migrationBuilder.DropTable(
				name: "Questions");

			migrationBuilder.DropTable(
				name: "Polls");

			migrationBuilder.DropTable(
				name: "AspNetUsers");
		}
	}
}
