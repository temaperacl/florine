﻿using System;
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

        public SKImage Hair { get { return _hair; } set { _hair = value; } }  
        private SKImage _hair = ResourceLoader.LoadImage("Images/customization/01_hair/50%/bob_50.png");
        public SKImage Face { get { return _face; } set { _face = value; } }
        private SKImage _face = ResourceLoader.LoadImage("Images/customization/02_faces/50%/face1_50.png");
        public SKImage Clothes { get { return _clothes; } set { _clothes = value; } }
        private SKImage _clothes = ResourceLoader.LoadImage("Images/customization/03_clothes/Box/50%/businesscasual1_box_50.png");
        public SKImage Body { get { return _body; } set { _body = value; } }
        private SKImage _body = ResourceLoader.LoadImage("Images/customization/04_bodyshapes/50%/boxshape_50.png");
        public SKImage Wings { get { return _wings; } set { _wings = value; } }
        private SKImage _wings = ResourceLoader.LoadImage("Images/customization/05_wings/50%/basicwings_50.png");
        
        public SKPaint WingPaint { get; set; }
        public SKPaint BodyPaint { get; set; }
        public SKPaint HairPaint { get; set; }        
        protected override void DrawImage(SKCanvas canvas, SKRect finalBoundingBox, SKPaint paint = null)
        {
            canvas.Clear();

            canvas.DrawImage(_wings, finalBoundingBox, WingPaint);
            canvas.DrawImage(_body, finalBoundingBox, BodyPaint);
            canvas.DrawImage(_face, finalBoundingBox, paint);
            canvas.DrawImage(_clothes, finalBoundingBox, paint);
            canvas.DrawImage(_hair, finalBoundingBox, HairPaint);
        }
        
    }
}
