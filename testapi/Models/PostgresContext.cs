using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace testapi.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banco> Bancos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<TarjetaMaestro> TarjetaMaestros { get; set; }

    public virtual DbSet<TransaccionesTarjeta> TransaccionesTarjetas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=2404");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Banco>(entity =>
        {
            entity.HasKey(e => e.CodBanco).HasName("banco_pkey");

            entity.ToTable("banco");

            entity.Property(e => e.CodBanco).HasColumnName("cod_banco");
            entity.Property(e => e.NombreBco).HasColumnName("nombre_bco");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CodCliente).HasName("cliente_pkey");

            entity.ToTable("cliente");

            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.NombreApellido).HasColumnName("nombre_apellido");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(15)
                .HasColumnName("numero_documento");
            entity.Property(e => e.TipoDocumento).HasColumnName("tipo_documento");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.CodMarca).HasName("marca_pkey");

            entity.ToTable("marca");

            entity.Property(e => e.CodMarca).HasColumnName("cod_marca");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
        });

        modelBuilder.Entity<TarjetaMaestro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tarjeta_maestro_pkey");

            entity.ToTable("tarjeta_maestro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodBanco).HasColumnName("cod_banco");
            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.CodMarca).HasColumnName("cod_marca");
            entity.Property(e => e.MontoLinea).HasColumnName("monto_linea");
            entity.Property(e => e.NumTarjeta)
                .HasMaxLength(16)
                .HasColumnName("num_tarjeta");
            entity.Property(e => e.Saldo).HasColumnName("saldo");
        });

        modelBuilder.Entity<TransaccionesTarjeta>(entity =>
        {
            entity.HasKey(e => e.CodTransacc).HasName("transacciones_tarjetas_pkey");

            entity.ToTable("transacciones_tarjetas");

            entity.Property(e => e.CodTransacc).HasColumnName("cod_transacc");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaHoraTransacc)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora_transacc");
            entity.Property(e => e.IdTarjeta).HasColumnName("id_tarjeta");
            entity.Property(e => e.MontoTransacccion).HasColumnName("monto_transacccion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
