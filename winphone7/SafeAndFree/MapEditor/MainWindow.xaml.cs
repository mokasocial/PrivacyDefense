using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Drawing.Imaging;
using System.IO;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] tiles;
        private BitmapSource[,] tileImages;

        private List<BitmapImage> tileTextures = new List<BitmapImage>();

        public MainWindow()
        {
            InitializeComponent();

            GrabData();

            GenerateMap();
        }

        private void GrabData()
        {
            XmlTextReader reader = new XmlTextReader(Environment.CurrentDirectory + "\\TileDefinitions.xml");

            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;

                if (nodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("map"))
                    {
                        Uri path = new Uri(Environment.CurrentDirectory + reader.GetAttribute("path"), UriKind.Absolute);
                        BitmapImage newImg = new BitmapImage(path);

                        drawnMap.Source = newImg;
                        drawnMap.Width = newImg.Width;
                        drawnMap.Height = newImg.Height;
                    }
                    else if (reader.Name.Equals("tile"))
                    {
                        Uri path = new Uri(Environment.CurrentDirectory + reader.GetAttribute("path"), UriKind.Absolute);
                        BitmapImage newImg = new BitmapImage(path);

                        tileTextures.Add(newImg);

                        Image imageVisual = new Image();
                        imageVisual.Source = newImg;
                        imageVisual.Width = newImg.Width;
                        imageVisual.Height = newImg.Height;

                        tileList.Children.Add(imageVisual);
                    }
                }
            }
        }

        private System.Drawing.Bitmap _bitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                // from System.Media.BitmapImage to System.Drawing.Bitmap
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        private BitmapSource _bitmapToSource(System.Drawing.Bitmap bitmap)
        {
            BitmapSource destination;
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromEmptyOptions();
            destination = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, sizeOptions);
            destination.Freeze();
            return destination;
        }


        private unsafe void GenerateMap()
        {
            int imageWidth = (int)drawnMap.Source.Width;
            int imageHeight = (int)drawnMap.Source.Height;

            tiles = new int[imageWidth, imageHeight];
            tileImages = new BitmapSource[imageWidth, imageHeight];

            System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, imageWidth, imageHeight);

            System.Drawing.Bitmap bitmapSource = _bitmapFromSource((BitmapSource)drawnMap.Source);

            BitmapData bitmapPixels = bitmapSource.LockBits(imageRectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int row = 0; row < imageHeight; row++)
            {
                int* scanline = (int*)bitmapPixels.Scan0 + (row * imageWidth);

                for (int col = 0; col < imageWidth; col++)
                {
                    System.Drawing.Color savedColor = System.Drawing.Color.FromArgb(scanline[col]);

                    tiles[col, row] = savedColor.R;
                    tileImages[col, row] = tileTextures[tiles[col, row]];

                    Image newImage = new Image();
                    newImage.Source = tileImages[col, row];
                    newImage.Width = tileImages[col, row].Width;
                    newImage.Height = tileImages[col, row].Height;
                    Canvas.SetLeft(newImage, col * 16);
                    Canvas.SetTop(newImage, row * 16);
                    map.Children.Add(newImage);
                }
            }

            bitmapSource.UnlockBits(bitmapPixels);
        }
    }
}