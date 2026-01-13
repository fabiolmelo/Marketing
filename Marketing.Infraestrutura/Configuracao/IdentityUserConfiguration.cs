using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<UsuarioEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioEntity> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Email);
            builder.Property(u => u.UserName).HasMaxLength(150);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(150);

            builder.Property(u => u.Email).HasMaxLength(200);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(200);

            builder.HasIndex(u => u.NormalizedEmail)
                .HasDatabaseName("IX_Usuarios_Email");
        }
    }
}