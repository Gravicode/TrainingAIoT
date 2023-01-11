// Decompiled with JetBrains decompiler
// Type: BrainPad.Image
// Assembly: BrainPad.Drivers, Version=2.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 3D2488AD-4B64-482F-8964-F4ED8586A515
// Assembly location: D:\experiment\DumpBrainpad\DumpBrainpad\bin\Debug\BrainPad.Drivers.dll

using System;

namespace BluinoNet
{
  public class Image
  {
    public int Height { get; internal set; }

    public int Width { get; internal set; }

    public double[] Data { get; internal set; }

    public Image(string img, int width, int height)
      : this(img, width, height, 1, 1, Image.Transform.None)
    {
    }

    public Image(
      string img,
      int width,
      int height,
      int hScale,
      int vScale,
      Image.Transform transform)
    {
      double[] data = new double[img.Length];
      for (int index = 0; index < img.Length; ++index)
        data[index] = img[index] != ' ' ? 1.0 : 0.0;
      this.CreateImage(data, width, height, hScale, vScale, transform);
    }

    public Image(double[] data, int width, int height)
      : this(data, width, height, 1, 1, Image.Transform.None)
    {
    }

    public Image(
      double[] data,
      int width,
      int height,
      int hScale,
      int vScale,
      Image.Transform transform)
    {
      this.CreateImage(data, width, height, hScale, vScale, transform);
    }

    private void CreateImage(
      double[] data,
      int width,
      int height,
      int hScale,
      int vScale,
      Image.Transform transform)
    {
      if (width * height != data.Length)
        throw new Exception("Incorrect image data size");
      this.Height = height * vScale;
      this.Width = width * hScale;
      this.Data = new double[this.Width * this.Height];
      for (int index1 = 0; index1 < this.Width; ++index1)
      {
        for (int index2 = 0; index2 < this.Height; ++index2)
        {
          switch (transform)
          {
            case Image.Transform.None:
              this.Data[index2 * this.Width + index1] = data[index2 / vScale * width + index1 / hScale];
              break;
            case Image.Transform.FlipHorizontal:
              this.Data[index2 * this.Width + (this.Width - index1 - 1)] = data[index2 / vScale * width + index1 / hScale];
              break;
            case Image.Transform.FlipVertical:
              this.Data[(this.Height - index2 - 1) * this.Width + index1] = data[index2 / vScale * width + index1 / hScale];
              break;
            case Image.Transform.Rotate90:
              this.Data[index1 * this.Height + this.Height - index2 - 1] = data[index2 / vScale * width + index1 / hScale];
              break;
            case Image.Transform.Rotate180:
              this.Data[(this.Height - index2 - 1) * this.Width + (this.Width - index1 - 1)] = data[index2 / vScale * width + index1 / hScale];
              break;
            case Image.Transform.Rotate270:
              this.Data[(this.Width - index1 - 1) * this.Height + index2] = data[index2 / vScale * width + index1 / hScale];
              break;
          }
        }
      }
      if (transform != Image.Transform.Rotate90 && transform != Image.Transform.Rotate270)
        return;
      int width1 = this.Width;
      this.Width = this.Height;
      this.Height = width1;
    }

    public enum Transform
    {
      None,
      FlipHorizontal,
      FlipVertical,
      Rotate90,
      Rotate180,
      Rotate270,
    }
  }
}
