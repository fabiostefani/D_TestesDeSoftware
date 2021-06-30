using fabiostefani.io.Catalogo.Domain;
using Microsoft.EntityFrameworkCore;

namespace fabiostefani.io.Catalogo.Data.Mappings
{
    public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(x=>x.Id);

            builder.Property(x=>x.Nome)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId);

            builder.ToTable("Categorias");

        }
    }
}