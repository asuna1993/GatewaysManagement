using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GatewaysManagement.Data.Migrations
{
    public partial class SeedingTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var gatewayId1 = Guid.NewGuid();
            var gatewayId2 = Guid.NewGuid();
            var gatewayId3 = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "Gateways",
                columns: new[] { "Id", "SerialNumber", "Name", "IPAdress" },
                values: new object[,] { 
                    { gatewayId1, "123456789", "gateway1", "198.165.1.1" },
                    { gatewayId2, "456789123", "gateway2", "165.130.1.1" },
                    { gatewayId3, "678912345", "gateway3", "203.180.1.1" },

                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "UID", "Vendor", "CreatedAt", "Status", "GatewayId" },
                values: new object[,]{
                    { Guid.NewGuid(), "1", "vendor1", DateTime.UtcNow, 0, gatewayId1  },
                    { Guid.NewGuid(), "2", "vendor2", DateTime.UtcNow, 1, gatewayId1  },
                    { Guid.NewGuid(), "3", "vendor3", DateTime.UtcNow, 0, gatewayId1  },
                    { Guid.NewGuid(), "4", "vendor4", DateTime.UtcNow, 1, gatewayId1  },

                    { Guid.NewGuid(), "5", "vendor5", DateTime.UtcNow, 1, gatewayId2  },
                    { Guid.NewGuid(), "6", "vendor6", DateTime.UtcNow, 0, gatewayId2  },
                    { Guid.NewGuid(), "7", "vendor7", DateTime.UtcNow, 1, gatewayId2  },
                    { Guid.NewGuid(), "8", "vendor8", DateTime.UtcNow, 0, gatewayId2  },

                    { Guid.NewGuid(), "9", "vendor9", DateTime.UtcNow, 1, gatewayId3  },
                    { Guid.NewGuid(), "10", "vendor10", DateTime.UtcNow, 0, gatewayId3  },
                    { Guid.NewGuid(), "11", "vendor11", DateTime.UtcNow, 1, gatewayId3  },
                    { Guid.NewGuid(), "12", "vendor12", DateTime.UtcNow, 0, gatewayId3  },
                }
                );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
