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
        private Image[,] visualTiles;

        private Dictionary<BitmapSource, System.Drawing.Bitmap> cachedSourceToBitmaps = new Dictionary<BitmapSource, System.Drawing.Bitmap>();

        private BitmapSource tileTexture;
        private System.Drawing.Bitmap[] cachedTileFrames;

        public Point TileDimensions;

        unsafe public MainWindow()
        {
            InitializeComponent();

            GrabData();

            LoadTileTexture();

            GenerateMap();
        }

        private unsafe void LoadTileTexture()
        {
            Uri path = new Uri(Environment.CurrentDirectory + "/Tileset32.bmp", UriKind.RelativeOrAbsolute);
            tileTexture = new BitmapImage(path);

            int numTiles = (int)Math.Round(tileTexture.Width / 32);
            cachedTileFrames = new System.Drawing.Bitmap[numTiles];
            for (int i = 0; i < numTiles; i++)
            {
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)TileDimensions.X, (int)TileDimensions.Y);
                System.Drawing.Bitmap originalBitmap = _bitmapFromSource(tileTexture);
                BitmapData originalPixels = originalBitmap.LockBits(new System.Drawing.Rectangle((int)TileDimensions.X * i, 0, (int)TileDimensions.X, (int)TileDimensions.Y), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                BitmapData bitmapPixels = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, (int)TileDimensions.X, (int)TileDimensions.Y), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                for (int row = 0; row < TileDimensions.Y; row++)
                {
                    int* originalScanline = (int*)originalPixels.Scan0 + (row * (int)TileDimensions.Y);
                    int* bitmapScanline = (int*)bitmapPixels.Scan0 + (row * (int)TileDimensions.Y);

                    for (int col = 0; col < TileDimensions.X; col++)
                    {
                        bitmapScanline[col] = originalScanline[col];
                    }
                }

                originalBitmap.UnlockBits(originalPixels);
                bitmap.UnlockBits(bitmapPixels);

                cachedTileFrames[i] = bitmap;
            }
        }

        private void GrabData()
        {
            XmlTextReader reader = new XmlTextReader(Environment.CurrentDirectory + "\\MapDefinitions.xml");

            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;

                if (nodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("map"))
                    {
                        Uri path = new Uri(Environment.CurrentDirectory + reader.GetAttribute("path"), UriKind.Absolute);

                        ImageSource loadedImage = null;

                        try
                        {
                            loadedImage = new BitmapImage(path);
                        }
                        catch (Exception e)
                        {
                            System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(25, 15);
                            loadedImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(newImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        }
                        finally
                        {
                            drawnMap.Source = loadedImage;
                        }

                        drawnMap.Width = drawnMap.Source.Width;
                        drawnMap.Height = drawnMap.Source.Height;

                        TileDimensions = new Point(Int32.Parse(reader.GetAttribute("tileWidth")), Int32.Parse(reader.GetAttribute("tileHeight")));
                    }/*
                    else if (reader.Name.Equals("tile"))
                    {
                        Uri path = new Uri(Environment.CurrentDirectory + reader.GetAttribute("path"), UriKind.Absolute);
                        BitmapImage newImg = new BitmapImage(path);

                        Image imageVisual = new Image();
                        imageVisual.Source = newImg;
                        imageVisual.Width = newImg.Width;
                        imageVisual.Height = newImg.Height;

                        tileList.Children.Add(imageVisual);
                    }*/
                }
            }
        }

        private System.Drawing.Bitmap _bitmapFromSource(BitmapSource bitmapsource)
        {
            if (cachedSourceToBitmaps.ContainsKey(bitmapsource))
            {
                return cachedSourceToBitmaps[bitmapsource];
            }

            System.Drawing.Bitmap bitmap;

            using (MemoryStream outStream = new MemoryStream())
            {
                // from System.Media.BitmapImage to System.Drawing.Bitmap
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }

            cachedSourceToBitmaps.Add(bitmapsource, bitmap);

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
            int imageWidth = (int)Math.Round(drawnMap.Source.Width);
            int imageHeight = (int)Math.Round(drawnMap.Source.Height);

            tiles = new int[imageWidth, imageHeight];
            visualTiles = new Image[imageWidth, imageHeight];
            //visualBitmaps = new System.Drawing.Bitmap[imageWidth, imageHeight];
            //tileImages = new BitmapSource[imageWidth, imageHeight];

            System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, imageWidth, imageHeight);

            System.Drawing.Bitmap bitmapSource = _bitmapFromSource((BitmapSource)drawnMap.Source);

            BitmapData bitmapPixels = bitmapSource.LockBits(imageRectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            for (int row = 0; row < imageHeight; row++)
            {
                int* scanline = (int*)bitmapPixels.Scan0 + (row * imageWidth);

                for (int column = 0; column < imageWidth; column++)
                {
                    System.Drawing.Color savedColor = System.Drawing.Color.FromArgb(scanline[column]);

                    tiles[column, row] = savedColor.R;
                    //tileImages[column, row] = tileTextures[tiles[column, row]];

                    Image newImage = new Image();
                    newImage.Source = _bitmapToSource(cachedTileFrames[tiles[column, row]]);
                    newImage.Width = cachedTileFrames[tiles[column, row]].Width;
                    newImage.Height = cachedTileFrames[tiles[column, row]].Height;
                    visualTiles[column, row] = newImage;

                    //visualBitmaps[column, row] = _bitmapFromSource(tileTextures[tiles[column, row]]);

                    Canvas.SetLeft(newImage, column * TileDimensions.X);
                    Canvas.SetTop(newImage, row * TileDimensions.Y);
                    map.Children.Add(visualTiles[column, row]);
                }
            }

            bitmapSource.UnlockBits(bitmapPixels);
        }

        private void Map_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (null == tiles || 0 == tiles.Length)
            {
                // Tiles haven't been loaded.
                return;
            }

            Point mousePos = e.GetPosition(map);
            int column = (int)(mousePos.X / TileDimensions.X);
            int row = (int)(mousePos.Y / TileDimensions.Y);

            if (column < 0 || column > tiles.GetUpperBound(0) || row < 0 || row > tiles.GetUpperBound(1))
            {
                // Out of bounds.
                return;
            }

            tiles[column, row] += (e.LeftButton == MouseButtonState.Pressed ? 1 : -1);

            if (tiles[column, row] == cachedTileFrames.Length)
            {
                tiles[column, row] = 0;
            }
            else if (tiles[column, row] < 0)
            {
                tiles[column, row] = cachedTileFrames.Length - 1;
            }

            visualTiles[column, row].Source = _bitmapToSource(cachedTileFrames[tiles[column, row]]);
        }

        private int ColorToInt(Color color)
        {
            return (int)((color.A << 24) | (color.R << 16) |
                          (color.G << 8) | (color.B << 0));
        }

        /// <summary>
        /// Event handler called when the save map button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event handler.</param>
        private unsafe void Save_Map_Click(object sender, RoutedEventArgs e)
        {
            if (null == tiles || 0 == tiles.Length)
            {
                // There are no tiles shown. We can't save a map.
                return;
            }

            // Open the save file dialog to request for a file name and location.
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "SafeAndFreeMap";
            dialog.DefaultExt = ".png";
            dialog.Filter = "Portable Network Graphics|*.png";
            bool? save = dialog.ShowDialog();

            if (save.Equals(true))
            {
                // The user decided to save - so let's do it.

                // Get image dimensions.
                int imageWidth = (int)Math.Round((tiles.GetUpperBound(0) + 1) * TileDimensions.X);
                int imageHeight = (int)Math.Round((tiles.GetUpperBound(1) + 1) * TileDimensions.Y);

                System.Drawing.Bitmap newMap = new System.Drawing.Bitmap(imageWidth, imageHeight);

                System.Drawing.Rectangle newMapRectangle = new System.Drawing.Rectangle(0, 0, imageWidth, imageHeight);

                BitmapData newMapData = newMap.LockBits(newMapRectangle, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                for (int i = 0; i < imageHeight; i++)
                {
                    int* newMapScanline = (int*)newMapData.Scan0 + (i * imageWidth);

                    for (int j = 0; j < imageWidth; j++)
                    {
                        int col = (int)Math.Floor(j / TileDimensions.X);
                        int row = (int)Math.Floor(i / TileDimensions.Y);

                        int relativeCol = (int)(j - col * TileDimensions.X);
                        int relativeRow = (int)(i - row * TileDimensions.Y);

                        newMapScanline[j] = cachedTileFrames[tiles[col, row]].GetPixel(relativeCol, relativeRow).ToArgb();
                    }
                }

                newMap.UnlockBits(newMapData);

                newMap.Save(dialog.FileName);
            }
        }
    }
}