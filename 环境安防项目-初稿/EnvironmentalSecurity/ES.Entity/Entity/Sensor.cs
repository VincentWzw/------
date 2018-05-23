using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ES.Entity
{
    [Table("Sensor")]

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public partial class Sensor
    {
        public Sensor()
        {
            SensorData = new HashSet<SensorData>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public byte? Type { get; set; }

        public byte? Address { get; set; }

        public int? CoordinatorId { get; set; }

        private string _DataStr = "1";
        public string DataStr {
            get { return _DataStr; }
            set { _DataStr = value; }
        }

        public virtual Coordinator Coordinator { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SensorData> SensorData { get; set; }
     
        
    }

}
