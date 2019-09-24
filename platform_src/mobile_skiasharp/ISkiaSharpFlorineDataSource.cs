using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    public interface ISkiaSharpFlorineDataSource
    {
        byte[] GetBytes(string Identifier);
        SKManagedStream GetStream(string Identifier);
        /*(
         * SKBitmap bitmap = null;
using (var assetStream = Assets.Open("image.png"))
using (var managedStream = new SKManagedStream(assetStream))
{
  bitmap = SKBitmap.Decode(managedStream);
}*/
    }
}
