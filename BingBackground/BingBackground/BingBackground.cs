namespace BingBackground
{
  using System.Globalization;
  using System.Linq;
  using System.Threading;
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.IO;

  [SuppressMessage("ReSharper", "FunctionNeverReturns")]
  class BingBackground
  {
    private static string[] FilePaths;
    private static object locker = new object();

    private static void Main(string[] args)
    {
      FilePaths = Directory.GetFiles(
        Directory.GetParent(BackgroundHandler.ImgSaveFolder).ToString(),
        "*.*",
        searchOption: SearchOption.AllDirectories);
      new Thread(UpdateBackgroundFromWeb).Start();
      if (args.Any() && args[0] == "1" && args.Count() > 1) new Thread(() => ChangeBackgroundLocal(int.Parse(args[1]))).Start();
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
        lock (locker)
        {
          FilePaths = Directory.GetFiles(
            Directory.GetParent(BackgroundHandler.ImgSaveFolder).ToString(),
            "*.*",
            SearchOption.AllDirectories);
        }

      }
    }

    private static void ChangeBackgroundLocal(int minus)
    {
      var rand = new Random(int.Parse(DateTime.Now.ToString(CultureInfo.InvariantCulture)));
      while (true)
      {
        lock (locker)
        {
          var randIndex = rand.Next(FilePaths.Count());
          BackgroundHandler.SetBackground(FilePaths[randIndex]);
        }
        Thread.Sleep(minus);
      }
    }
  }

}