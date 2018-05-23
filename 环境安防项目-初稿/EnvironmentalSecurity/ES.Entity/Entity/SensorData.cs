using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace ES.Entity
{
    [Table("SensorData")]

    public partial class SensorData
    {
        public int Id { get; set; }

        public int? SensorId { get; set; }

        [Column(TypeName = "text")]
        public byte[] Data { get; set; }

        public DateTime? Time { get; set; }

        public virtual Sensor Sensor { get; set; }
    }
}
