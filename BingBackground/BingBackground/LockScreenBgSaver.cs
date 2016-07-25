namespace BingBackground
{
  using System.Drawing;
  using System.IO;
  using System;
  using System.Collections.Generic;
  using System.Configuration;
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
      //DeleteTempFiles();
      CopyImage();
      //ClassifyTempFiles();
    }

    private static void Init(string source,string dest)
    {
      sourceDir = source;
      tempDir = Path.Combine(dest, @"tempDir");
      destDir = Path.Combine(dest,@"BackGroundImages",@"WingPaper");
      phoneBgDir = Path.Combine(dest, @"PhoneBg");
      filesProcessed = Path.Combine(phoneBgDir, @"ImgsProcessed.txt");

      if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
      if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
      if (!Directory.Exists(phoneBgDir)) Directory.CreateDirectory(phoneBgDir);

      if (!File.Exists(filesProcessed))
        File.WriteAllText(filesProcessed, string.Empty);
    }

    private static void CopyImage()
    {
      var processedFiles = File.ReadAllLines(filesProcessed);
      foreach (var file in Directory.GetFiles(sourceDir))
      {
        var imageName = Path.GetFileNameWithoutExtension(file);
        var destFile =Path.Combine(destDir,imageName+".jpg");
        var phoneFile = Path.Combine(phoneBgDir, imageName + ".jpg");
        if (!processedFiles.Contains(phoneFile))
        {
          File.Copy(file, phoneFile, true);
          if(IsHDImage(phoneFile) && !File.Exists(destFile))
            File.Move(phoneFile,destFile);
          else if(!IsPhoneImage(phoneFile))
            File.Delete(phoneFile);
          using (var fileHandler=File.AppendText(filesProcessed))
          {
            fileHandler.WriteLine(phoneFile);
          }
        }
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
        using (var bitMap=new Bitmap(file))
        {
          Image img=new Bitmap(bitMap);
          return img.Width >= 1920 && img.Height >= 1080;
        }
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
        using (var bitMap=new Bitmap(file))
        {
          Image img = new Bitmap(bitMap);
          return img.Width >= 1080 && img.Height >= 1920 && img.Height > img.Width;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}
