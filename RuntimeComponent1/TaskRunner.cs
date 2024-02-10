using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.Connectivity;

namespace RuntimeComponent1
{
  public sealed class TaskRunner : IBackgroundTask
  {
    BackgroundTaskDeferral _Deferral;
    const string NetworkChangedTaskName = "NetworkChangedTask";

    public void Run(IBackgroundTaskInstance taskInstance)
    {
      _Deferral = taskInstance.GetDeferral();
      System.Diagnostics.Debug.WriteLine("Network state changed");
      ExploreNetworkInfo();

      var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
      var key = taskInstance.Task.TaskId.ToString();
      settings.Values[key] = "Some information";

      _Deferral.Complete();
    }

    private void ExploreNetworkInfo()
    {
      var Profiles = NetworkInformation.GetConnectionProfiles();
      bool FoundInternet = false;

      foreach (var P in Profiles)
      {
        var Level = P.GetNetworkConnectivityLevel();
        var Wiring = (P.IsWlanConnectionProfile || P.IsWwanConnectionProfile) ? "Wireless" : "Ethernet Wired";
        if (Level == NetworkConnectivityLevel.InternetAccess)
        {
          FoundInternet = true;
          System.Diagnostics.Debug.WriteLine($"{P.ProfileName} {Wiring}");
        }
      }
      if (!FoundInternet)
        System.Diagnostics.Debug.WriteLine("No internet access profiles");
    }
  }
}
