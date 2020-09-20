using Microsoft.EntityFrameworkCore;
using SocialNetworkBLL.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SocialNetworkBLL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SocialNetworkDLL
{
    public class SocialNetworkContext : IdentityDbContext
    {
        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options) : base(options)
        {

        }
            public virtual DbSet<Usuario> Usuarios { get; set; }
            public virtual DbSet<Comentario> Comentarios { get; set; }
            public virtual DbSet<Perfil> Perfis { get; set; }
            public virtual DbSet<Post> Posts { get; set; }
            public virtual DbSet<FileData> FileDatas { get; set; }
            public virtual DbSet<Amizade> Amizades { get; set; }

        public class DesingTimeDbContextFactory : IDesignTimeDbContextFactory<SocialNetworkContext>
        {
            public SocialNetworkContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../SocialNetworkAPI/appsettings.json").Build();

                var builder = new DbContextOptionsBuilder<SocialNetworkContext>();
                var connectionString = configuration.GetConnectionString("DatabaseConnection");
                builder.UseSqlServer(connectionString);

                return new SocialNetworkContext(builder.Options);
            }
        }
    }
}
