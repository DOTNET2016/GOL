namespace GOL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GoLContext : DbContext
    {
        public GoLContext()
            : base("name=GoLContext")
        {
        }

        public virtual DbSet<Generation> Generations { get; set; }
        public virtual DbSet<PlayersTable> PlayersTables { get; set; }
        public virtual DbSet<SavedGame> SavedGames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayersTable>()
                .Property(e => e.PlayerName)
                .IsUnicode(false);
        }
    }
}
