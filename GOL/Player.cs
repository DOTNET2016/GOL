namespace GOL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Player")]
    public partial class Player
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Player()
        {
            SavedGames = new HashSet<SavedGame>();
        }

        public int id { get; set; }

        [StringLength(25)]
        public string PlayerName { get; set; }

        public int? SavedGame_id { get; set; }

        public virtual SavedGame SavedGame { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SavedGame> SavedGames { get; set; }
    }
}
