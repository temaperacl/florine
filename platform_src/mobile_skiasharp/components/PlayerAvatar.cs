using System;
using System.Collections.Generic;
using System.Text;
using Florine;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    class PlayerAvatar : AspectImage
    {
        public PlayerAvatar(IImage img) : base() {
            baseImage = _body;
        }
        protected override bool NeedImage { get { return false; } }

        public enum AvatarElement
        {
            Hair,
            Pants,
            Shirt,
            Shoes,
            Skin,
            Wings
        }
                
        private SKImage _hair = ResourceLoader.LoadImage("Images/customization/01_hair/50%/bob_50.png");
        private SKImage _face = ResourceLoader.LoadImage("Images/customization/02_faces/50%/face1_50.png");
        private SKImage _clothes = ResourceLoader.LoadImage("Images/customization/03_clothes/Box/50%/businesscasual1_box_50.png");
        private SKImage _body = ResourceLoader.LoadImage("Images/customization/04_bodyshapes/50%/boxshape_50.png");
        private SKImage _wings = ResourceLoader.LoadImage("Images/customization/05_wings/50%/basicwings_50.png");
        
        protected override void DrawImage(SKCanvas canvas, SKRect finalBoundingBox, SKPaint paint = null)
        {            
            canvas.DrawImage(_wings, finalBoundingBox, paint);
            canvas.DrawImage(_body, finalBoundingBox, paint);
            canvas.DrawImage(_face, finalBoundingBox, paint);
            canvas.DrawImage(_clothes, finalBoundingBox, paint);
            canvas.DrawImage(_hair, finalBoundingBox, paint);
        }
        
    }
}
