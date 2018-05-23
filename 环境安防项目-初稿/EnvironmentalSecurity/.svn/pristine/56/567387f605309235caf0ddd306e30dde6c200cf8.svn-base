using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;


namespace ES.Entity
{
    
   public partial class ES_DB:DbContext
   {
       public ES_DB() : base("name=ES_DB")
       {
       }
      

        public virtual DbSet<Coordinator> Coordinator { get; set; }
       public virtual DbSet<Sensor> Sensor { get; set; }
       public virtual DbSet<SensorData> SensorData { get; set; }

       protected override void OnModelCreating(DbModelBuilder modelBuilder)
       {
           modelBuilder.Entity<SensorData>()
               .Property(e => e.Data)
               .HasMaxLength(9);

       }
   }
}
