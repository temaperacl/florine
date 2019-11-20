using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SkiaSharp;

namespace Florine.Droid
{
    class AndroidDataStore : FlorineSkiaSharpForms.ISkiaSharpFlorineDataSource
    {
        private Android.Content.Res.AssetManager _assets;
        public AndroidDataStore(Android.Content.Res.AssetManager AssetSource)
        {
            _assets = AssetSource;
        }
        public List<string> Identifiers(string SubSet)
        {
            return new List<string>(_assets.List(SubSet));
        }
        public SKManagedStream GetStream(string Identifier)
        {
            //string[] AssetList = _assets.List(Identifier);
           // if (AssetList.Count() > 0)
           // {
                try
                {
                    return new SKManagedStream(_assets.Open(Identifier), true);
                }
                catch
                {
                    return null;
                }
            //}
            //return null;
        }
        public byte[] GetBytes(string Identifier)
        {
            try
            {
                using (System.IO.Stream s = _assets.Open(Identifier))
                {
                    if (null != s)
                    {
                        byte[] Result = new byte[500000];
                        s.Read(Result, 0, 500000);
                        return Result;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            
        }
    }
}