using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace ThGameMgr.MARISA
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ImportReplayFilesDialog : Window
    {
        public ImportReplayFilesDialog()
        {
            InitializeComponent();
        }

        private readonly static Dictionary<string, string> _gameIdDictionary
            = new()
            {
                { "th6", "Th06" },
                { "th7", "Th07" },
                { "th8", "Th08" },
                { "th9", "Th09" },
                { "th10", "Th10" },
                { "th11", "Th11" },
                { "th12", "Th12" },
                { "th13", "Th13" },
                { "th14", "Th14" },
                { "th15", "Th15" },
                { "th16", "Th16" },
                { "th17", "Th17" },
                { "th18", "Th18" },
                { "th19", "Th19" }
            };

        public Dictionary<string, string>? AvailableGameFilesDictionary { get; set; }

        public string GetGameId(string replayFilePath)
        {
            string replayName = Path.GetFileNameWithoutExtension(replayFilePath);
            return _gameIdDictionary[replayName.Split('_')[0]];
        }

        public string GetGameFilePath(string gameId)
        {
            if (this.AvailableGameFilesDictionary != null)
            {
                if (this.AvailableGameFilesDictionary.TryGetValue(gameId, out string? gameFilePath))
                {
                    return gameFilePath;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string? GetReplayDirectory(string gameId)
        {
            string shanghaiAliceAppData = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\ShanghaiAlice";
            string gamePath = GetGameFilePath(gameId);

            if (gamePath == null)
            {
                return null;
            }
            else
            {
                string? gameDirectory = Path.GetDirectoryName(gamePath);

                if (
                    gameId == "Th06" ||
                    gameId == "Th07" ||
                    gameId == "Th08" ||
                    gameId == "Th09" ||
                    gameId == "Th10" ||
                    gameId == "Th11" ||
                    gameId == "Th12")
                {
                    return $"{gameDirectory}\\replay";
                }
                else
                {
                    return $"{shanghaiAliceAppData}\\{gameId.ToLower()}\\replay";
                }
            }
        }

        public string Import(string replayFilePath)
        {
            string gameId = GetGameId(replayFilePath);
            string? replayDirectory = GetReplayDirectory(gameId);
            string replayName = Path.GetFileNameWithoutExtension(replayFilePath);
            if (Directory.Exists(replayDirectory))
            {
                try
                {
                    string newReplayFile = $"{replayDirectory}\\{replayName}.rpy";
                    int i = 0;
                    while (File.Exists(newReplayFile))
                    {
                        i++;
                        newReplayFile = $"{replayDirectory}\\{replayName}-{i}.rpy";
                    }

                    File.Move(replayFilePath, newReplayFile);
                    return $"成功:{newReplayFile}";
                }
                catch (Exception ex)
                {
                    return $"エラー:{ex.Message}";
                }
            }
            else
            {
                return $"取り込み先ディレクトリが存在しませんでした。Game:{gameId}";
            }
        }

        private async void ImportButtonClick(object sender, RoutedEventArgs e)
        {
            if (ReplayFilesListBox.Items.Count > 0)
            {
                string[] replayFiles = new string[ReplayFilesListBox.Items.Count];

                int i = 0;
                foreach (object replayFileItem in ReplayFilesListBox.Items)
                {
                    replayFiles[i] = (string)replayFileItem;
                    i++;
                }

                ImportButton.IsEnabled = false;
                ImportReplayFilesMenuItem.IsEnabled = false;
                await Task.Run(() =>
                {
                    foreach (string replayFile in replayFiles)
                    {
                        string message = Import(replayFile);

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ReplayFilesListBox.Items.Remove(replayFile);
                            OutputBox.Text += $"{message}\n";
                        }
                        ));
                    }
                });
                ImportButton.IsEnabled = true;
                ImportReplayFilesMenuItem.IsEnabled = true;

                MessageBox.Show(this, "リプレイファイルを取り込みました。", "リプレイファイルの一括追加",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(this, "取り込むリプレイファイルがありません。", "リプレイファイルの取り込み",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void WindowPreviewDragOver(object sender, DragEventArgs e)
        {
            //ファイルがドラッグされたとき、カーソルをドラッグ中のアイコンに変更し、そうでない場合は何もしない。
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void WindowPreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) // ドロップされたものがファイルかどうか確認する。
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string path in paths)
                {
                    ReplayFilesListBox.Items.Add(path);
                }
            }
        }

        private void CloseMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutMenuItemClick(object sender, RoutedEventArgs e)
        {
            MarisaMain marisaMain = new();
            string information
                = $"{marisaMain.Name} Version.{marisaMain.Version}\n{marisaMain.Description}\nby {marisaMain.Developer}";

            MessageBox.Show(this, information, "バージョン情報",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
