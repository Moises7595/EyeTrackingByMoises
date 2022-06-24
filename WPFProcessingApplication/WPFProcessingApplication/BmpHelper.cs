using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WPFProcessingApplication
{
    public class BmpHelper
    {
        private static BmpHelper instance;
        public static BmpHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BmpHelper();
                }
                return instance;
            }
        }

        public System.Drawing.Bitmap Bmp { get; set; }
        System.Drawing.Bitmap circle;

        public BmpHelper()
        {
            Refresh();
        }

        public void Refresh()
        {
            Bmp = new System.Drawing.Bitmap(1536, 864);
            circle = GetCircle(100, 20);
        }

        #region Diamond
        public void ColorBitmap(int x, int y)
        {
            int r = 50;
            int ls = (y - 25), ld = (x - 25), d = 1;

            for (int i = 0; i <= r; i++)
            {
                ColorPixel(ld, ls, d);
                ls++;
                ld--;
                d += 2;
            }
            ld += 2;
            d -= 4;
            for (int i = 0; i < r; i++)
            {
                ColorPixel(ld, ls, d);
                ls++;
                ld++;
                d -= 2;
            }
        }

        void ColorPixel(int x, int y, int d)
        {
            int nx = 0;
            int indiceAumento = 10;
            int red = 0, green = 0;
            for (int j = 0; j < d; j++)
            {
                nx = x + j;
                if (y >= 0 && y < Bmp.Height && nx >= 0 && nx < Bmp.Width)
                {
                    System.Drawing.Color color = Bmp.GetPixel(nx, y);
                    if ((color.R + color.G + color.B) == 0)
                    {
                        green = 255;
                    }
                    else
                    {
                        red = color.R;
                        green = color.G;
                        if (green == 255)
                        {
                            if ((red + indiceAumento) <= 255)
                            {
                                red += indiceAumento;
                            }
                            else
                            {
                                green -= (indiceAumento - (255 - red));
                                red = 255;
                            }
                        }
                        else
                        {
                            if ((green - indiceAumento) >= 0)
                            {
                                green -= indiceAumento;
                            }
                            else
                            {
                                green = 0;
                            }
                        }
                        Bmp.SetPixel(nx, y, System.Drawing.Color.FromArgb(255, (color.G > 9) ? (color.G - 10) : 0, 0));
                    }
                    Bmp.SetPixel(nx, y, System.Drawing.Color.FromArgb(red, green, 0));
                }
            }
        }
        #endregion

        #region CircleFig
        public void ColorCircle(int x, int y)
        {
            int nx = x - (circle.Width / 2), ny = y - (circle.Width / 2);
            int red = 0, green = 0;
            for (int i = 0; i < circle.Width; i++)
            {
                for (int j = 0; j < circle.Width; j++)
                {
                    if (nx + i >= 0 && nx + i < Bmp.Width && ny + j >= 0 && ny + j < Bmp.Height)
                    {
                        var pixel = circle.GetPixel(i, j);
                        if (pixel.R != 0)
                        {
                            var color = Bmp.GetPixel(nx + i, ny + j);
                            red = color.R;
                            green = color.G;
                            if ((color.R + color.G + color.B) == 0)
                            {
                                green = 255;
                            }
                            else
                            {
                                if (green == 255)
                                {
                                    if ((red + pixel.R) <= 255)
                                    {
                                        red += pixel.R;
                                    }
                                    else
                                    {
                                        green -= (pixel.R - (255 - red));
                                        red = 255;
                                    }
                                }
                                else
                                {
                                    if ((green - pixel.R) >= 0)
                                    {
                                        green -= pixel.R;
                                    }
                                    else
                                    {
                                        green = 0;
                                    }
                                }
                            }
                            Bmp.SetPixel(nx + i, ny + j, System.Drawing.Color.FromArgb(red, green, 0));
                        }
                    }
                }
            }
        }

        Bitmap GetCircle(int diameter, int intensity)
        {
            var color = System.Drawing.Color.FromArgb(intensity, 0, 0);
            int aumento = 1;
            Bitmap bmp = new Bitmap(diameter + aumento, diameter + aumento);
            Graphics graphics = Graphics.FromImage(bmp);
            var pen = new System.Drawing.Pen(color);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            SolidBrush solidBrush = new SolidBrush(color);
            graphics.FillEllipse(solidBrush, 0, 0, diameter, diameter);
            graphics.DrawEllipse(pen, 0, 0, diameter, diameter);
            return bmp;
        }
        #endregion
    }
}
