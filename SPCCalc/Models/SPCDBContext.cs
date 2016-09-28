using SPCCalc.Models.Mold;
using SPCCalc.Models.Wirebond;
using System.Data.Entity;

namespace SPCCalc.Models
{
    public class SPCDBContext : DbContext
    {
        //SENSOR(KE_KT_SIP_KA)
        public DbSet<MachineList_MoldSensor_KEKTSIPKA> MachineList_MoldSensor_KEKTSIPKA { get; set; }

        //DieAttach > Bondline
        public DbSet<MachineList_DieAttach_Bondline> MachineList_DieAttach_Bondline { get; set; }

        //MoldGTS
        public DbSet<ViewModelGtsMachines> ViewModelGtsMachines { get; set; }
        public DbSet<ViewModelGts> ViewModelGts { get; set; }

        //MoldGTS2
        public DbSet<ViewModelGts2> ViewModelGts2 { get; set; }

        //MoldSensor
        public DbSet<ViewModelSensorMachines> ViewModelSensorMachines { get; set; }
        public DbSet<ViewModelUaMatrix> ViewModelUaMatrix { get; set; }

        //MoldKMatrixTOKTKN
        public DbSet<ViewModelKMatrixMachines> ViewModelKMatrixMachines { get; set; }
        public DbSet<ViewModelKMatrix> ViewModelKMatrix { get; set; }


        //MoldCurrentSensor
        public DbSet<ViewModelCurrentSensorMachines> ViewModelCurrentSensorMachines { get; set; }
        public DbSet<ViewModelCurrentSensor> ViewModelCurrentSensor { get; set; }


        //MoldSohed
        public DbSet<ViewModelSohedMachines> ViewModelSohedMachines { get; set; }
        public DbSet<ViewModelSohed> ViewModelSohed { get; set; }


        //WirebondGts
        public DbSet<ViewModelWirebondGtsMachines> ViewModelWirebondGtsMachines { get; set; }
        public DbSet<ViewModelGtsDataSetup> ViewModelGtsDataSetup { get; set; }
        public DbSet<ViewModelWirebondGtsBs> ViewModelWirebondGtsBs { get; set; }
        public DbSet<ViewModelWirebondGtsBsPbo> ViewModelWirebondGtsBsPbo { get; set; }
        public DbSet<ViewModelWirebondGtsWp> ViewModelWirebondGtsWp { get; set; }
        public DbSet<ViewModelWirebondGtsWpPbo> ViewModelWirebondGtsWpPbo { get; set; }


        //WirebondSohed
        public DbSet<ViewModelWirebondSohedMachines> ViewModelWirebondSohedMachines { get; set; }
        public DbSet<ViewModelSohedDataSetup> ViewModelSohedDataSetup { get; set; }
        public DbSet<ViewModelWirebondSohedBs> ViewModelWirebondSohedBs { get; set; }
        public DbSet<ViewModelWirebondSohedBsPbo> ViewModelWirebondSohedBsPbo { get; set; }
        public DbSet<ViewModelWirebondSohedWp> ViewModelWirebondSohedWp { get; set; }
        public DbSet<ViewModelWirebondSohedWpPbo> ViewModelWirebondSohedWpPbo { get; set; }


        //WirebondSensor
        public DbSet<ViewModelWirebondSensorMachines> ViewModelWirebondSensorMachines { get; set; }
        public DbSet<ViewModelSensorDataSetup> ViewModelSensorDataSetup { get; set; }
        public DbSet<ViewModelWirebondSensorBs> ViewModelWirebondSensorBs { get; set; }
        public DbSet<ViewModelWirebondSensorBsPbo> ViewModelWirebondSensorBsPbo { get; set; }
        public DbSet<ViewModelWirebondSensorWp> ViewModelWirebondSensorWp { get; set; }
        public DbSet<ViewModelWirebondSensorWpPbo> ViewModelWirebondSensorWpPbo { get; set; }


        //WirebondSL
        public DbSet<ViewModelWirebondSLMachines> ViewModelWirebondSLMachines { get; set; }
        public DbSet<ViewModelSLDataSetup> ViewModelSLDataSetup { get; set; }
        public DbSet<ViewModelWirebondSLBs> ViewModelWirebondSLBs { get; set; }
        public DbSet<ViewModelWirebondSLWp> ViewModelWirebondSLWp { get; set; }

        public SPCDBContext() : base("name=SPCContext") { }
    }
}