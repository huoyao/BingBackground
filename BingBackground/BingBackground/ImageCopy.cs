namespace BingBackground
{
  using System.Drawing;
  using System.IO;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  class ImageCopy
  {
    private const string        SourceDir =
      @"C:\Users\leta\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";
    private const string          DestDir = @"D:\pictures\BackGroundImages\WingPaper";
    private const string FilesToBeDeleted = DestDir + @"\ToBeDeleted.txt";
    private const string     FilesDeleted = DestDir + @"\Deleted.txt";

    static ImageCopy()
    {
      string[] strEmpty = { string.Empty };
      if (!File.Exists(FilesToBeDeleted))
        File.WriteAllLines(FilesToBeDeleted, strEmpty);
      if (!File.Exists(FilesDeleted))
        File.WriteAllLines(FilesDeleted, strEmpty);
    }

    public static void ImageCopyHandler()
    {
      DeleteFiles();
      DetectFilesToBeDeleted();
      CopyImage();
      Thread.Yield();
    }

    private static void CopyImage(string sourceDir = SourceDir, string destDir = DestDir)
    {
      var filesDeleted = File.ReadAllLines(FilesDeleted);
      foreach (var file in Directory.GetFiles(sourceDir))
      {
        var imageName = Path.GetFileNameWithoutExtension(file);
        var destFile = string.Format("{0}\\{1}.jpg", destDir, imageName);
        if (!File.Exists(destFile) && !filesDeleted.Contains(destFile))
          File.Copy(file, destFile, true);
      }
    }

    private static void DetectFilesToBeDeleted(string destDir = DestDir)
    {
      var filesToBeDeleted = Directory.GetFiles(destDir, "*.jpg").Where(file => !IsHDImage(file)).ToList();
      File.WriteAllLines(FilesToBeDeleted, filesToBeDeleted);
    }

    private static void DeleteFiles()
    {
      var files = File.ReadAllLines(FilesToBeDeleted);
      var filesRemain = new List<string>();
      var filesDeleted = new List<string>();
      foreach (var file in files)
      {
        try
        {
          File.Delete(file);
          filesDeleted.Add(file);
        }
        catch (Exception)
        {
          filesRemain.Add(file);
        }
      }
      File.WriteAllLines(FilesToBeDeleted, filesRemain);
      File.AppendAllLines(FilesDeleted, filesDeleted);
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
  }
}
