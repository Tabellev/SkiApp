using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skiAppDatamodel
{
    public class SkiEntities : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SkiDay> SkiDays { get; set; }

        public DbSet<Lift> Lifts { get; set; }

        public DbSet<Slope> Slopes { get; set; }


        public SkiEntities()
            : base(@"Data Source=donau.hiof.no;Initial Catalog=isabelev;Persist Security Info=True;User ID=isabelev;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false;  // Fix One - avoid cycles
        }

        public System.Data.Entity.DbSet<skiAppDatamodel.Destination> Destinations { get; set; }

        public System.Data.Entity.DbSet<skiAppDatamodel.DestinationInfoType> DestinationInfoTypes { get; set; }

        public System.Data.Entity.DbSet<skiAppDatamodel.OpeningHours> OpeningHours { get; set; }

    }
}
