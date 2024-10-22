using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace baitaplon.Models;

public partial class EazydealsContext : DbContext
{
    public EazydealsContext()
    {
    }

    public EazydealsContext(DbContextOptions<EazydealsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

	public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Wishlist> Wishlist { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PHUONGANH;Database=eazydeals;uid=sa;pwd=1234$;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__accounts__3213E83F3479CD5C");

            entity.ToTable("accounts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(1)
                .HasColumnName("active");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("gender");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("image");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasDefaultValue(0)
                .HasColumnName("role");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Cid).HasName("PK__category__D837D05F8DD8BFA7");

            entity.ToTable("category");

            entity.Property(e => e.Cid).HasColumnName("cid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83F298CF735");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Orderid)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderid");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("paymentType");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("status");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__orders__userId__5EBF139D");
        });

        modelBuilder.Entity<OrderedProduct>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__ordered___C2FFCF13E5FDD4C8");

            entity.ToTable("ordered_product");

            entity.Property(e => e.Oid).HasColumnName("oid");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("name");
            entity.Property(e => e.Orderid)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("orderid");
            entity.Property(e => e.Price)
                .HasMaxLength(45)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderedProducts)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("FK__ordered_p__order__66603565");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__product__DD37D91A41E41A6D");

            entity.ToTable("product");

            entity.Property(e => e.Pid).HasColumnName("pid");
            entity.Property(e => e.Cid)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("cid");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.Discount)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("discount");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("quantity");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK__product__cid__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
