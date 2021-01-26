using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SynthAnvil.CustomControls
{
    class GradientButton : Button
    {
        Boolean isMouseClick;
        Boolean isMouseOver;
        Boolean isActive;
        Boolean horizontalGradient;
        Image greyScaleImage;

        [Description("Is the button active (blue)"), Category("Behavior")]
        public Boolean Active
        {
            get { return isActive; }
            set { isActive = value; }
        }

        [Description("Is the gradient horizontal instead of 45 degrees"), Category("Behavior")]
        public Boolean HorizontalGradient
        {
            get { return horizontalGradient; }
            set { horizontalGradient = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle rect = new Rectangle(ClientRectangle.X+1, ClientRectangle.Y+1, ClientRectangle.Width-2, ClientRectangle.Height-2);

            if (isMouseClick)
            {
                SolidBrush brush = new SolidBrush(Color.FromArgb(230, 230, 255)); 
                e.Graphics.FillRectangle(brush, rect);
            }
            else if(isActive)
            {
                LinearGradientBrush brush = new LinearGradientBrush(rect,
                                                                        Color.FromArgb(66, 116, 144),
                                                                        Color.FromArgb(167, 200, 255),
                                                                        45F);
                e.Graphics.FillRectangle(brush, rect);
            }
            else if (isMouseOver)
            {
                using (LinearGradientBrush br = new LinearGradientBrush(
                    rect, Color.Blue, Color.White, 45f))
                {
                    // Create a ColorBlend object. Note that you
                    // must initialize it before you save it in the
                    // brush's InterpolationColors property.
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = new Color[]
                    {
                        Color.FromArgb(120, 120, 120),
                        Color.FromArgb(250, 250, 255),
                        Color.FromArgb(120, 120, 120)
                    };
                    colorBlend.Positions = new float[]
                    {
                        0f, 1/2f, 1f
                    };
                    br.InterpolationColors = colorBlend;

                    e.Graphics.FillRectangle(br, rect);
                }
            }
            else if (!Enabled)
            {
                LinearGradientBrush brush = new LinearGradientBrush(rect,
                                                                        Color.FromArgb(30, 30, 30),
                                                                        Color.FromArgb(150, 150, 150),
                                                                        45F);
                e.Graphics.FillRectangle(brush, rect);
            }
            else 
            {
                float angle = 45f;
                if (horizontalGradient)
                {
                    angle = 90f;
                }
                using (LinearGradientBrush br = new LinearGradientBrush(
                    rect, Color.Blue, Color.White, angle))
                {
                    // Create a ColorBlend object. Note that you
                    // must initialize it before you save it in the
                    // brush's InterpolationColors property.
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = new Color[]
                    {
                        Color.FromArgb(50, 50, 50),
                        Color.FromArgb(220, 220, 230),
                        Color.FromArgb(50, 50, 50)
                    };
                    colorBlend.Positions = new float[]
                    {
                        0f, 1/2f, 1f
                    };
                    br.InterpolationColors = colorBlend;

                    e.Graphics.FillRectangle(br, rect);
                }
            }

            // Draw text
            Brush foreBrush = new SolidBrush(ForeColor);
            if (isMouseClick || isMouseOver)
            {
                foreBrush = new SolidBrush(Color.SaddleBrown);
            }
            SizeF size = e.Graphics.MeasureString(Text, Font);
            PointF pt = new PointF((Width - size.Width) / 2 + (isMouseClick ? 1 : 0), (Height - size.Height) / 2 + (isMouseClick ? 1 : 0));
            if (!Enabled)
            {
                foreBrush = new SolidBrush(Color.Gray);
            }
            e.Graphics.DrawString(Text, Font, foreBrush, pt);
            foreBrush.Dispose();

            if (Image != null)
            {
                if (ImageAlign.Equals(ContentAlignment.BottomCenter) || ImageAlign.Equals(ContentAlignment.MiddleCenter) || ImageAlign.Equals(ContentAlignment.TopCenter))
                {
                    if (!Enabled)
                    {
                        e.Graphics.DrawImage(GrayscaleImage(Image), (Width - Image.Width) / 2, 1, Image.Width, Image.Height);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Image, (Width - Image.Width) / 2, 1, Image.Width, Image.Height);
                    }
                }
                else
                {
                    if (!Enabled)
                    {
                        e.Graphics.DrawImage(GrayscaleImage(Image), 3, 1, Image.Width, Image.Height);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Image, 3, 1, Image.Width, Image.Height);
                    }
                }
            }

        }

        private Image GrayscaleImage(Image original)
        {
            if(greyScaleImage!=null)
            {
                return greyScaleImage;
            }

            //create a blank bitmap the same size as original
            greyScaleImage = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(greyScaleImage);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();

            return greyScaleImage;
        }


        protected override void OnMouseEnter(EventArgs e)
                {
                    isMouseOver = true;
                    Invalidate();
                    base.OnMouseEnter(e);
                }

                protected override void OnMouseLeave(EventArgs e)
                {
                    isMouseOver = false;
                    Invalidate();
                    base.OnMouseLeave(e);
                }
        
                protected override void OnMouseUp(MouseEventArgs mevent)
                {
                    isMouseClick = false;
                    Invalidate();
                    base.OnMouseUp(mevent);
                }

                protected override void OnMouseDown(MouseEventArgs mevent)
                {
                    isMouseClick = true;
                    Invalidate();
                    base.OnMouseDown(mevent);
                }

            
    }
}
