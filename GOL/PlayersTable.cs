namespace GOL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlayersTable")]
    public partial class PlayersTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlayersTable()
        {
            SavedGames = new HashSet<SavedGame>();
        }

        [Key]
        public int Player_id { get; set; }

        [Required]
        [StringLength(20)]
        public string PlayerName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SavedGame> SavedGames { get; set; }
    }
}
