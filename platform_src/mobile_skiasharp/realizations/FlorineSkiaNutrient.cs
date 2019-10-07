using System;
using System.Collections.Generic;
using System.Text;
using Florine;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    class FlorineSkiaNutrient : Nutrient
    {
        public SKColor RingColor { get; set; }
        public FlorineSkiaNutrient(Nutrient n)
        {
            Name = n.Name;
            Class = n.Class;
            Units = n.Units;
            DailyTarget = n.DailyTarget;
            switch (Name) {
                case "Protein":
                    RingColor = new SKColor(0xcc, 0, 0);
                    break;
                case "Carbohydrates": //#e69138
                    RingColor = new SKColor(0xe6, 0x91, 0x38);
                    break;
                case "Fat":
                    RingColor = new SKColor(0xcc, 0xcc, 0xcc);
                    break;
                case "Fiber": //93c47d
                    RingColor = new SKColor(0x93, 0xc4, 0x7d);
                    break;
                case "Folic Acid":
                    RingColor = new SKColor(0x6a, 0xa8, 0x4f);
                    break;
                case "Vitamin D":
                    RingColor = new SKColor(0, 0xff, 0xff);
                    break;
                case "Calcium":
                    RingColor = new SKColor(0xff, 0xff, 0xff);
                    break;
                case "Iron":
                    RingColor = new SKColor(0xff, 0, 0);
                    break;
                case "Potassium":
                    RingColor = new SKColor(0x99, 0, 0xff);
                    break;
                case "Vitamin B12":
                    RingColor = new SKColor(0xff, 0x22, 0xcc);
                    break;
                case "Vitamin A":
                    RingColor = new SKColor(0xff, 0x99, 0);
                    break;
                default:
                    RingColor = new SKColor(255, 0, 255);
                    break;
            }
        }
               
    }
}
