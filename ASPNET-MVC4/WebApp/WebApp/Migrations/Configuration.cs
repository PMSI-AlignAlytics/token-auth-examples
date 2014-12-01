namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Models.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebApp.Models.AuthContext context)
        {
            //  This method will be called after migrating to the latest version.

            // AHSCLfFeM58BT8Uf5uemYjfzoiz6nFRHeFR/cHxnTdzDLVlu833cL1XQiHH7m4p/dA==
            // a9010b8f-7bd6-4bf1-a1ae-90f407501fc0


            context.Users.AddOrUpdate(
                new IdentityUser
                {
                    Id = "599cc3b4-b8c5-4427-b074-c08201227a30",
                    UserName = "iduser",
                    PasswordHash = "AHSCLfFeM58BT8Uf5uemYjfzoiz6nFRHeFR/cHxnTdzDLVlu833cL1XQiHH7m4p/dA==",
                    SecurityStamp = "a9010b8f-7bd6-4bf1-a1ae-90f407501fc0",
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                });
        }
    }
}
