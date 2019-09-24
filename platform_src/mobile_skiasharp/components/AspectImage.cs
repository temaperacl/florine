using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public class AspectImage : IFlorineSkiaConnectable, Florine.IImage
    {
        public SKImage baseImage
        {
            get { return _image;  }
            set
            {
                _image = value;
                if(_image != null)
                {
                    imageRatio = ((float)(_image.Height)) / ((float) (_image.Width));
                }
            }
        }

        public int ImageKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private SKImage _image = null;
        private float imageRatio = 1f;

        public enum ScalingType
        {
            None,
            Zoom,
            Stretch
        };

        public virtual void ConnectCanvasView(SKCanvasView CV) {
            CV.PaintSurface += (sender, e) => {
                Draw(
                    e.Surface.Canvas,
                    new SKRect(
                        0, 0, e.Info.Width, e.Info.Height
                    )
                );
            };
        }

        protected virtual void DrawImage(SKCanvas canvas, SKRect finalBoundingBox, SKPaint paint = null)
        {
            if (_image == null) { return; }
            canvas.DrawImage(_image, finalBoundingBox, paint);
        }
        protected SKRect RatioBox(SKRect boundingBox)
        {
            if (_image == null) { return new SKRect(); }
            
            if (scaling == ScalingType.Stretch)
            {
                return boundingBox;
            }
            else if (scaling == ScalingType.None)
            {
                return new SKRect(0f, 0f, _image.Width, _image.Height);
            }
            else if (scaling == ScalingType.Zoom)
            {
                float BoundRatio = boundingBox.Height / boundingBox.Width;
                float scaler = 1;
                if (imageRatio > BoundRatio)
                {
                    // Wider box, will be scaling based on height.
                    scaler = boundingBox.Height / (float)(_image.Height);
                }
                else
                {
                    // Taller Box, will be scaling based on width
                    scaler = boundingBox.Width / (float)(_image.Width);
                }
                float newWidth = _image.Width * scaler;
                float newHeight = _image.Height * scaler;
                float deltaW = boundingBox.Width - newWidth;
                float deltaH = boundingBox.Height - newHeight;
                SKRect actualBound = new SKRect(
                    boundingBox.Left + (deltaW / 2),
                    boundingBox.Top + (deltaH / 2),
                    boundingBox.Left + newWidth + (deltaW / 2),
                    boundingBox.Top + newHeight + (deltaH / 2));
                return actualBound;
            }
            return new SKRect();
        }
        public ScalingType scaling = ScalingType.Zoom;
        public void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint = null)
        {
            if (_image == null) { return; }
            DrawImage(canvas, RatioBox(boundingBox), paint);
            return;
        }
    }
}
