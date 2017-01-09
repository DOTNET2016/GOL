namespace GOL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GOLContext : DbContext
    {
        public GOLContext()
            : base("name=GOLContext")
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
        }
    }
}
