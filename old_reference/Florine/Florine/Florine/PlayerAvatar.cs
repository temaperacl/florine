using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace Florine
{
    class PlayerAvatar : AspectImage
    {
        public enum AvatarElement
        {
            Hair,
            Pants,
            Shirt,
            Shoes,
            Skin,
            Wings
        }
        
        private SKImage _hair = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Hair.png");
        private SKImage _pants = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Pants.png");
        private SKImage _shirt = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Shirt.png");
        private SKImage _shoes = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Shoes.png");
        private SKImage _skin = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Skin.png");
        private SKImage _wings = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Wings.png");

        public Dictionary<AvatarElement, SKColor> ColorScheme = new Dictionary<AvatarElement, SKColor>()
        {
            { AvatarElement.Hair, new SKColor(84,47,78) },
            { AvatarElement.Pants, new SKColor(62, 96, 65) },
            { AvatarElement.Shirt, new SKColor(173, 156, 94) },
            { AvatarElement.Shoes, new SKColor(58, 44, 19) },
            { AvatarElement.Skin, new SKColor(224, 189, 151) },
            { AvatarElement.Wings, new SKColor(227, 227, 158) },
        };



        public PlayerAvatar()
        {
            baseImage = _hair;
        }

        private static PlayerAvatar _avatar = new PlayerAvatar();
        public static PlayerAvatar Avatar { get { return _avatar; } }
        private static SKPaint ColoredFilter(SKColor Base, float r, float g, float b)
        {
            return new SKPaint()
            {
                ColorFilter = SKColorFilter.CreateColorMatrix(new float[]
                { //84,47,78
                    Base.Red / r, 0f, 0f, 0, 0,
                    0f, Base.Green / g, 0f, 0, 0,
                    0f, 0f, Base.Blue / b, 0, 0,
                    0,     0,     0,     1, 0
                })
            };
        }
        private static List<AvatarElement> _drawOrder = new List<AvatarElement>()
        {
            AvatarElement.Wings,
            AvatarElement.Skin,
            AvatarElement.Pants,
            AvatarElement.Shirt,
            AvatarElement.Shoes,
            AvatarElement.Hair,
        };
        private Dictionary<AvatarElement, SKImage> _images = new Dictionary<AvatarElement, SKImage>()
        {
            { AvatarElement.Hair, ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Hair.png")  },
            { AvatarElement.Pants, ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Pants.png") },
            { AvatarElement.Shirt, ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Shirt.png") },
            { AvatarElement.Shoes, ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Shoes.png") },
            { AvatarElement.Skin, ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Skin.png") },
            { AvatarElement.Wings, ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Char_Wings.png") },
        };
        private Dictionary<AvatarElement, AspectImage> _zoomImages = new Dictionary<AvatarElement, AspectImage>()
        {
           { AvatarElement.Hair, new AspectImage() {   baseImage = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Zoom.Char_Hair.png") } },
           { AvatarElement.Pants, new AspectImage() {  baseImage = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Zoom.Char_Pants.png") } },
           { AvatarElement.Shirt, new AspectImage() {   baseImage = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Zoom.Char_Shirt.png") } },
           { AvatarElement.Shoes, new AspectImage() {  baseImage = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Zoom.Char_Shoes.png") } },
           { AvatarElement.Skin, new AspectImage() {   baseImage = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Zoom.Char_Skin.png") } },
           { AvatarElement.Wings, new AspectImage() {  baseImage = ResourceLoader.LoadImage("Florine.Data.Img.Avatar.Zoom.Char_Wings.png") } },
        };
        private static Dictionary<AvatarElement, float> _baselineColors = new Dictionary<AvatarElement, float>()
        {
            { AvatarElement.Hair, 61f  },
            { AvatarElement.Pants, 82f },
            { AvatarElement.Shirt, 154f },
            { AvatarElement.Shoes, 45f },
            { AvatarElement.Skin, 195f },
            { AvatarElement.Wings, 219f },
        };

        public void DrawElement(AvatarElement ae, SKCanvas canvas, SKRect p, SKColor color, bool autoscale = false)
        {
            float baseColor = _baselineColors[ae];
            SKRect boundingBox = p;
            if(autoscale)
            {
                boundingBox = RatioBox(p);
            }
            canvas.DrawImage(_images[ae], boundingBox, ColoredFilter(color, baseColor, baseColor, baseColor));
        }
        public void DrawZoomElement(AvatarElement ae, SKCanvas canvas, SKRect p, SKColor color, bool autoscale = false)
        {
            float baseColor = _baselineColors[ae];
            _zoomImages[ae].Draw(canvas, p, ColoredFilter(color, baseColor, baseColor, baseColor));
        }
        protected override void DrawImage(SKCanvas canvas, SKRect finalBoundingBox, SKPaint paint = null)
        {
            foreach(AvatarElement ae in _drawOrder)
            {
                if (null != paint)
                {
                    DrawElement(ae, canvas, finalBoundingBox, paint.Color);
                }
                else
                {
                    DrawElement(ae, canvas, finalBoundingBox, ColorScheme[ae]);
                }
            }
        }


    }
}
