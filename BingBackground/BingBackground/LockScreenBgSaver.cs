namespace BingBackground
{
  using System.Drawing;
  using System.IO;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  class LockScreenBgSaver
  {
    private static string sourceDir;
    private static string tempDir;
    private static string destDir;
    private static string phoneBgDir;
    private static string filesProcessed;

    public static void ImageCopyHandler(string sourceImgDir,string destImgDir)
    {
      if (sourceImgDir == null || destImgDir==null)
      {
        Console.WriteLine("[Error] Input parameter is null!");
        return;
      }
      Init(sourceImgDir,destImgDir);
      DeleteTempFiles();
      CopyImageToTempFolder();
      ClassifyTempFiles();
    }

    private static void Init(string source,string dest)
    {
      sourceDir = source;
      tempDir = Path.Combine(dest, @"tempDir");
      destDir = Path.Combine(dest,@"BackGroundImage",@"WingPaper");
      phoneBgDir = Path.Combine(dest, @"PhoneBg");
      filesProcessed = Path.Combine(tempDir, @"ImgsProcessed.txt");

      if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
      if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
      if (!Directory.Exists(phoneBgDir)) Directory.CreateDirectory(phoneBgDir);

      if (!File.Exists(filesProcessed))
        File.WriteAllText(filesProcessed, string.Empty);
    }

    private static void CopyImageToTempFolder()
    {
      var processedFiles = File.ReadAllLines(filesProcessed);
      foreach (var file in Directory.GetFiles(sourceDir))
      {
        var imageName = Path.GetFileNameWithoutExtension(file);
        var tempFile = string.Format("{0}\\{1}.jpg", tempDir, imageName);
        if (!File.Exists(tempFile) && !processedFiles.Contains(tempFile))
          File.Copy(file, tempFile, true);
      }
    }

    private static void DeleteTempFiles()
    {
      var processedFiles = new List<string>();
      foreach (var file in Directory.GetFiles(tempDir,"*.jpg"))
      {
        try
        {
          File.Delete(file);
          processedFiles.Add(file);
        }
        catch (Exception)
        {
          //ignore
        }
        finally
        {
          File.AppendAllLines(filesProcessed,processedFiles);
        }
      }
    }

    private static void ClassifyTempFiles()
    {
      var processedFiles = new List<string>();
      foreach (var file in Directory.GetFiles(tempDir,"*.jpg"))
      {
        try
        {
          if (IsHDImage(file))
          {
            var destPath = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destPath, true);
          }
          else if (IsPhoneImage(file))
          {
            var destPath = Path.Combine(phoneBgDir, Path.GetFileName(file));
            File.Copy(file, destPath, true);
          }
          File.Delete(file);
          processedFiles.Add(file);
        }
        catch (Exception)
        {
          // ignored
        }
        finally
        {
          File.AppendAllLines(filesProcessed,processedFiles);
        }
      }
    }

    private static bool IsHDImage(string file)
    {
      try
      {
        var img = Image.FromFile(file, true);
        return img.Width >= 1920 && img.Height >= 1080;
      }
      catch (Exception)
      {
        return false;
      }
    }

    private static bool IsPhoneImage(string file)
    {
      try
      {
        var img = Image.FromFile(file, true);
        return img.Width >= 1080 && img.Height >= 1920 && img.Height>img.Width;
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}
