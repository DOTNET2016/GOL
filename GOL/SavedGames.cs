namespace GOL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SavedGames
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SavedGames()
        {
            Generation = new HashSet<Generation>();
        }

        [Key]
        public int SavedGame_id { get; set; }

        public int Cell_X { get; set; }

        public int Cell_Y { get; set; }

        public bool IsAlive { get; set; }

        public int? Player_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Generation> Generation { get; set; }

        public virtual PlayersTable PlayersTable { get; set; }
    }
}
