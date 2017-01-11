namespace GOL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GContext : DbContext
    {
        public GContext()
            : base("name=GContext")
        {
        }

        public virtual DbSet<Generation> Generation { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<SavedGames> SavedGames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .Property(e => e.PlayerName)
                .IsUnicode(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.SavedGames)
                .WithOptional(e => e.Player)
                .HasForeignKey(e => e.Player_id);

            modelBuilder.Entity<SavedGames>()
                .HasMany(e => e.Generation)
                .WithOptional(e => e.SavedGames)
                .HasForeignKey(e => e.SavedGame_id);
        }
    }
}
