using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    class ResourceLoader
    {
        public static ISkiaSharpFlorineDataSource DataSource = null;
        public static Dictionary<string, SKImage> ImageList(string SubType)
        {
            //DataSource.G
            //List<string> Identifiers(string SubSet);
            Dictionary<string, SKImage> newDict = new Dictionary<string, SKImage>();
            foreach (string s in DataSource.Identifiers("Images/" + SubType))
            {
                string rs = "Images/" + SubType + "/" + s;
                newDict[rs] = LoadImage(rs);
            }
            return newDict;
        }
        public static SKBitmap LoadBitmap(string resourceID)
        {
            if (null == DataSource) { return null; }
            SKBitmap cereal;
            //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(ResourceLoader));
            byte [] data = DataSource.GetBytes(resourceID);
            if(null != data && data.Length > 0 ) {                
                cereal = SKBitmap.Decode(data);
                //SKCodecResult result = codec.GetPixels(cereal.Info, cereal.GetPixels());
                //if (result == SKCodecResult.Success)
                //{
                //    return cereal;
                //}
                
                return cereal;
            }
            return null;
        }
        public static SKImage LoadImage(string resourceID)
        {

            SKBitmap bmp = LoadBitmap(resourceID);
            if (null == bmp) { return null; }
            return SKImage.FromBitmap(bmp);
        }        
    }
}
