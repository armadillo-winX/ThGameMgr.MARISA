using System.Collections.Generic;
using System.Windows;
using ThGameMgr.Ex.Plugin;

namespace ThGameMgr.MARISA
{
    public class MarisaMain : GameFilesPluginBase
    {
        public override string Name => "MARISA for 東方管制塔 EX";

        public override string Version => "0.1.0";

        public override string Developer => "珠音茉白/東方管制塔開発部";

        public override string Description => "Multi Advanced Replay files Import and Sort Add-on for 東方管制塔 EX";

        public override string CommandName => "リプレイファイルの一括追加(MARISA)";

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
