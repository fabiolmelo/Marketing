using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
   public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable("Funcoes");

        builder.Property(r => r.Name).HasMaxLength(150);
        builder.Property(r => r.NormalizedName).HasMaxLength(150);

        builder.HasIndex(r => r.NormalizedName)
            .HasDatabaseName("IX_Funcoes_NormalizedName")
            .IsUnique();
    }
}
}