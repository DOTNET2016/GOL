namespace GOL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Generation")]
    public partial class Generation
    {
        [Key]
        public int Gen_id { get; set; }

        public int GenNumber { get; set; }

        public int Cell_X { get; set; }

        public int Cell_Y { get; set; }

        public bool IsAlive { get; set; }

        public int? SavedGame_id { get; set; }

        public virtual SavedGames SavedGames { get; set; }
    }
}
