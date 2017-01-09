namespace GOL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SavedGame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SavedGame()
        {
            Generations = new HashSet<Generation>();
        }

        [Key]
        public int SavedGame_id { get; set; }

        public int Cell_X { get; set; }

        public int Cell_Y { get; set; }

        public bool IsAlive { get; set; }

        public int? Player_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Generation> Generations { get; set; }

        public virtual Player Player { get; set; }
    }
}
