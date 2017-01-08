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

        public virtual DbSet<Generation> Generation { get; set; }
        public virtual DbSet<PlayersTable> PlayersTable { get; set; }
        public virtual DbSet<SavedGames> SavedGames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayersTable>()
                .Property(e => e.PlayerName)
                .IsUnicode(false);
        }
    }
}
