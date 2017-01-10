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

        public virtual DbSet<Generation> Generations { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<SavedGame> SavedGames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .Property(e => e.PlayerName)
                .IsUnicode(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.SavedGames)
                .WithOptional(e => e.Player)
                .HasForeignKey(e => e.Player_id);

            modelBuilder.Entity<SavedGame>()
                .HasMany(e => e.Generations)
                .WithOptional(e => e.SavedGame)
                .HasForeignKey(e => e.SavedGame_id);

            modelBuilder.Entity<SavedGame>()
                .HasMany(e => e.Players)
                .WithOptional(e => e.SavedGame)
                .HasForeignKey(e => e.SavedGame_id);
        }
    }
}
