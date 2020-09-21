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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Amizade>()
                .HasOne(e => e.Perfil)
                .WithMany(e => e.Seguindo)
                .HasForeignKey(e => e.PerfilId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Amizade>()
                .HasOne(e => e.PerfilSeguido)
                .WithMany(e => e.Seguidores)
                .HasForeignKey(e => e.PerfilSeguidoId)
                .OnDelete(DeleteBehavior.NoAction);
            
            
            // corrige o erro de cascade
            // pq o comentario ja seria deletado pelo post
            builder.Entity<Comentario>()
                .HasOne(e => e.Perfil)
                .WithMany()
                .HasForeignKey(e => e.PerfilId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
    
    
}
