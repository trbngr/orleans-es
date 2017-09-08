using System;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace Host
{
    class Program
    {
        static void Main()
        {
            var azureTableCS = "DefaultEndpointsProtocol=https;AccountName=cs44de867d736aax49dfxbd9;AccountKey=I9QAt4VtgU6UTeJNAtxVCjFijmmMiJNjC032iMUJe2CvVkPxaLWnJYb0AfJuR5AtMpUO6LQ3z43EXOcy4Db+hg==";

            var clusterConfig = ClusterConfiguration.LocalhostPrimarySilo();
            clusterConfig.AddMemoryStorageProvider("Default");
            clusterConfig.AddMemoryStorageProvider("PubSubStore");
            clusterConfig.AddSimpleMessageStreamProvider("Default");

            clusterConfig.Globals.LivenessType = GlobalConfiguration.LivenessProviderType.AzureTable;
            clusterConfig.Globals.DataConnectionString = azureTableCS;
            clusterConfig.Globals.DataConnectionStringForReminders = azureTableCS;
            clusterConfig.Globals.DeploymentId = "OrleansTest";
            clusterConfig.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.AzureTable;

            var silo = new SiloHost("Test Silo", clusterConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Press Enter to close.");
            Console.ReadLine();

            silo.ShutdownOrleansSilo();
        }
    }
}