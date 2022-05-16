using System;
using System.Collections.Generic;
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

        public BmpHelper()
        {
            Refresh();
        }

        public void Refresh()
        {
            Bmp = new System.Drawing.Bitmap(1536, 864);
        }

        public void ColorBitmap(int x, int y)
        {
            int r = 50;
            int ls = (y - r), ld = x, d = 1;

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
            for (int j = 0; j < d; j++)
            {
                nx = x + j;
                if (y >= 0 && y < Bmp.Height && nx >= 0 && nx < Bmp.Width)
                {
                    System.Drawing.Color color = Bmp.GetPixel(nx, y);
                    if (color.R == 0)
                    {
                        Bmp.SetPixel(nx, y, System.Drawing.Color.FromArgb(255, 255, 0));
                    }
                    else
                    {
                        Bmp.SetPixel(nx, y, System.Drawing.Color.FromArgb(255, (color.G > 9) ? (color.G - 10) : 0, 0));
                    }
                }
            }
        }
    }
}
