using System.Collections.Generic;
using System.Windows;
using ThGameMgr.Ex.Plugin;

namespace ThGameMgr.MARISA
{
    public class MarisaMain : GameFilesPluginBase
    {
        public override string Name => "MARISA for ๛วง EX";

        public override string Version => "0.1.0-beta";

        public override string Developer => "์นไ/๛วงJญ";

        public override string Description => "Multi Advanced Replay files Import and Sort Add-on for ๛วง EX";

        public override string CommandName => "vCt@Cฬ๊วม(MARISA)";

        public Window? MainWindow { get; set; }

        public override void Main(List<string> availableGamesList, Dictionary<string, string> availableGameFilesDictionary)
        {
            ImportReplayFilesDialog importReplayFilesDialog = new()
            {
                AvailableGameFilesDictionary = availableGameFilesDictionary
            };
            if (this.MainWindow != null)
                importReplayFilesDialog.Owner = this.MainWindow;

            importReplayFilesDialog.ShowDialog();
        }
    }
}
