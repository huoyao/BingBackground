﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundHandler.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BingBackground
{
  using System;
  using System.Drawing;
  using System.IO;
  using System.Net;
  using System.Runtime.InteropServices;
  using System.Windows.Forms;

  using Microsoft.Win32;

  using Newtonsoft.Json;

  public enum PicturePosition
  {
    Tile, 
    Center, 
    Stretch, 
    Fit, 
    Fill
  }

  class BackgroundHandler
  {
    public static string UrlBase;

    public static string BackgroundRecPath;

    public static string ImgSaveFolder;

    static BackgroundHandler()
    {
      BackgroundRecPath = Properties.Settings.Default.BackgroundRecFile;
      ImgSaveFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), 
        Properties.Settings.Default.ImgSaveFolder, 
        DateTime.Now.Year.ToString());
      using (var sr = new StreamReader(BackgroundRecPath))
      {
        UrlBase = sr.ReadLine();
      }
    }

    private static dynamic DownloadJson()
    {
      using (var webClient = new WebClient())
      {
        Console.WriteLine("Downloading JSON...");
        var jsonString = webClient.DownloadString(Properties.Settings.Default.DownloadSourcePath);
        return JsonConvert.DeserializeObject<dynamic>(jsonString);
      }
    }

    public static string GetBackgroundUrlBase()
    {
      dynamic jsonObject = DownloadJson();
      return Properties.Settings.Default.DownloadSite + jsonObject.images[0].urlbase;
    }

    /*
    private static string GetBackgroundTitle()
    {
      dynamic jsonObject = DownloadJson();
      string copyrightText = jsonObject.images[0].copyright;
      return copyrightText.Substring(0, copyrightText.IndexOf(" (", StringComparison.Ordinal));
    }
*/
    private static bool WebsiteExists(string url)
    {
      try
      {
        var request = WebRequest.Create(url);
        request.Method = "HEAD";
        var response = (HttpWebResponse)request.GetResponse();
        return response.StatusCode == HttpStatusCode.OK;
      }
      catch
      {
        return false;
      }
    }

    private static string GetResolutionExtension(string url)
    {
      var resolution = Screen.PrimaryScreen.Bounds;
      var widthByHeight = resolution.Width + "x" + resolution.Height;
      var potentialExtension = "_" + widthByHeight + ".jpg";
      if (WebsiteExists(url + potentialExtension))
      {
        Console.WriteLine("Background for " + widthByHeight + " found.");
        return potentialExtension;
      }
      else
      {
        Console.WriteLine("No background for " + widthByHeight + " was found.");
        Console.WriteLine("Using 1920x1080 instead.");
        return "_1920x1080.jpg";
      }
    }

    private static void SetProxy()
    {
      var proxyUrl = Properties.Settings.Default.Proxy;
      if (proxyUrl.Length <= 0)
      {
        return;
      }

      var webProxy = new WebProxy(proxyUrl, true) { Credentials = CredentialCache.DefaultCredentials };
      WebRequest.DefaultWebProxy = webProxy;
    }

    public static Image DownloadBackground(string url)
    {
      Console.WriteLine("Downloading background...");
      SetProxy();
      url += GetResolutionExtension(url);
      var request = WebRequest.Create(url);
      var reponse = request.GetResponse();
      var stream = reponse.GetResponseStream();
      return stream != null ? Image.FromStream(stream) : null;
    }

    public static string GetBackgroundImagePath()
    {
      Directory.CreateDirectory(ImgSaveFolder);
      return Path.Combine(ImgSaveFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".bmp");
    }

    public static void SaveBackground(Image background)
    {
      Console.WriteLine("Saving background...");
      background.Save(GetBackgroundImagePath(), System.Drawing.Imaging.ImageFormat.Bmp);
    }

    private static PicturePosition GetPosition()
    {
      var position = PicturePosition.Fit;
      switch (Properties.Settings.Default.Position)
      {
        case "Tile":
          position = PicturePosition.Tile;
          break;
        case "Center":
          position = PicturePosition.Center;
          break;
        case "Stretch":
          position = PicturePosition.Stretch;
          break;
        case "Fit":
          position = PicturePosition.Fit;
          break;
        case "Fill":
          position = PicturePosition.Fill;
          break;
      }

      return position;
    }

    internal sealed class NativeMethods
    {
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      internal static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }

    public static void SetBackground(string imgPath)
    {
      var style = GetPosition();
      Console.WriteLine("Setting background...");
      using (var key = Registry.CurrentUser.OpenSubKey(Path.Combine("Control Panel", "Desktop"), true))
      {
        if (key == null)
        {
          return;
        }

        switch (style)
        {
          case PicturePosition.Tile:
            key.SetValue("PicturePosition", "0");
            key.SetValue("TileWallpaper", "1");
            break;
          case PicturePosition.Center:
            key.SetValue("PicturePosition", "0");
            key.SetValue("TileWallpaper", "0");
            break;
          case PicturePosition.Stretch:
            key.SetValue("PicturePosition", "2");
            key.SetValue("TileWallpaper", "0");
            break;
          case PicturePosition.Fit:
            key.SetValue("PicturePosition", "6");
            key.SetValue("TileWallpaper", "0");
            break;
          case PicturePosition.Fill:
            key.SetValue("PicturePosition", "10");
            key.SetValue("TileWallpaper", "0");
            break;
        }
      }

      const int SetDesktopBackground = 20;
      const int UpdateIniFile = 1;
      const int SendWindowsIniChange = 2;
      NativeMethods.SystemParametersInfo(SetDesktopBackground, 0, imgPath, UpdateIniFile | SendWindowsIniChange);
    }
  }
}