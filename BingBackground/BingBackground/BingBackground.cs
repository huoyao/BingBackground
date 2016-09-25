namespace BingBackground
{
  using System.Globalization;
  using System.Linq;
  using System.Threading;
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.IO;
  using System.Linq.Expressions;

  [SuppressMessage("ReSharper", "FunctionNeverReturns")]
  class BingBackground
  {
    private static string[] filePaths;
    private static readonly object Locker = new object();

    private static void Main(string[] args)
    {
      if (!Directory.Exists(BackgroundHandler.ImgSaveFolder))
        Directory.CreateDirectory(BackgroundHandler.ImgSaveFolder);
      filePaths = Directory.GetFiles(
        Directory.GetParent(BackgroundHandler.ImgSaveFolder).ToString(),
        "*.jpg", SearchOption.AllDirectories);
      new Thread(UpdateBackgroundFromWeb).Start();
      new Thread(WingsPapersCopy).Start();
      if (Properties.Settings.Default.RunMode == 1)
      {
        new Thread(() => ChangeLocalBackground(Properties.Settings.Default.BGChangeInterval)).Start();
      }
    }

    private static void UpdateBackgroundFromWeb()
    {
      while (true)
      {
        var urlBase = BackgroundHandler.GetBackgroundUrlBase();
        while (urlBase == BackgroundHandler.UrlBase)
        {
          Thread.Sleep(30 * 60 * 1000);  //30 miniutes
          urlBase = BackgroundHandler.GetBackgroundUrlBase();
        }
        BackgroundHandler.UrlBase = urlBase;
        using (var sw = new StreamWriter(BackgroundHandler.BackgroundRecPath, false))
        {
          sw.Write(urlBase);
        }
        var background = BackgroundHandler.DownloadBackground(urlBase);
        BackgroundHandler.SaveBackground(background);
        BackgroundHandler.SetBackground(BackgroundHandler.GetBackgroundImagePath());
        lock (Locker)
        {
          filePaths = Directory.GetFiles(
            Directory.GetParent(BackgroundHandler.ImgSaveFolder).ToString(),
            "*.*",
            SearchOption.AllDirectories);
        }
      }
    }

    private static void ChangeLocalBackground(int minus)
    {
      int seed;
      int.TryParse(DateTime.Now.ToString(CultureInfo.InvariantCulture), out seed);
      var rand = new Random(seed);
      while (true)
      {
        lock (Locker)
        {
          if (!filePaths.Any()) continue;
          var randIndex = rand.Next(filePaths.Count());
          BackgroundHandler.SetBackground(filePaths[randIndex]);
        }
        Thread.Sleep(minus * 60 * 1000);
      }
    }

    private static void WingsPapersCopy()
    {
      while (true)
      {
        LockScreenBgSaver.ImageCopyHandler(
          Properties.Settings.Default.LockScreenBgDir,
          Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        Thread.Sleep(30 * 60 * 1000);  //30 minus
      }
    }
  }
}