using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using AForge.Imaging;
using AForge;
using AForge.Math.Geometry;
using System.Collections.Generic;
using AForge.Imaging.Filters;

namespace IMGProcess
{
    class Program
    {

        static void Main(string[] args)
        {



            string path = "temp\\orezany1.png";
           /* int u = 1;
            string[] filePaths = Directory.GetFiles("temp\\", "*.png", SearchOption.TopDirectoryOnly);
            foreach (string obrazek in filePaths)
            {
                //UdelejModrejOkraj(obrazek, u);
                u++;
                if (u > 200)
                {
                    break;
                }

            }

            */

            VytahniHlinu(path, 500);
        }

        static void VytahniHlinu(string path, int u)
        {

            Bitmap image = (Bitmap)Bitmap.FromFile(path);
            Bitmap imagePuvodni = (Bitmap)Bitmap.FromFile(path);


            ColorFiltering filterBlue = new ColorFiltering();
            filterBlue.Red = new IntRange(35, 45);
            filterBlue.Green = new IntRange(32, 42);
            filterBlue.Blue = new IntRange(30, 40);



            filterBlue.ApplyInPlace(image);
            image.Save("hlina.png");


            GaussianBlur filterBlur = new GaussianBlur(4, 11);
            filterBlur.ApplyInPlace(image);
            image.Save("hlina blur.png");

            BlobCounter bc = new BlobCounter();

            bc.FilterBlobs = true;
            bc.MinHeight = 500;
            bc.MinWidth = 500;


            bc.ProcessImage(image);

            Blob[] blobs = bc.GetObjectsInformation();

            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            foreach (var blob in blobs)
            {
                List<IntPoint> edgePoints = bc.GetBlobsEdgePoints(blob);
                List<IntPoint> cornerPoints;

                // use the shape checker to extract the corner points
                if (shapeChecker.IsQuadrilateral(edgePoints, out cornerPoints))
                {
                    // only do things if the corners form a rectangle
                    if (shapeChecker.CheckPolygonSubType(cornerPoints) == PolygonSubType.Rectangle)
                    {
                        // here i use the graphics class to draw an overlay, but you
                        // could also just use the cornerPoints list to calculate your
                        // x, y, width, height values.
                        List<System.Drawing.Point> Points = new List<System.Drawing.Point>();
                        foreach (var point in cornerPoints)
                        {
                            Points.Add(new System.Drawing.Point(point.X, point.Y));
                        }

                        /* uauauauauauaua */
                        System.Drawing.Point[] poleRohu = new System.Drawing.Point[Points.Count];
                        poleRohu = Points.ToArray();

                        /* Tady zpracuju rohove body a vypocitam si vzdalenosti */
                        int novaSirka = (poleRohu[2].X - poleRohu[0].X);
                        int novaVyska = (poleRohu[3].Y - poleRohu[1].Y);
                        int noveX = poleRohu[0].X;
                        int noveY = poleRohu[1].Y;

                        // vyrezu obrazek
                        Rectangle vyrez = new Rectangle(noveX, noveY, novaSirka, novaVyska);
                        // ulozim vyrezany
                        Bitmap vyrezanyObraz = imagePuvodni.Clone(vyrez, imagePuvodni.PixelFormat);

                        vyrezanyObraz.Save("output\\orezany" + u + ".png", ImageFormat.Jpeg);


                        Graphics g = Graphics.FromImage(image);
                        g.DrawPolygon(new Pen(Color.Red, 5.0f), poleRohu);
                        // image.Save("result.png");


                    }
                }
            }


        }





        /*var hodiny = System.Diagnostics.Stopwatch.StartNew();
        int u = 1;
        string[] filePaths = Directory.GetFiles("temp\\", "*.png", SearchOption.TopDirectoryOnly);
        /* int num_threads = 4;
         int pocetObrazkuVeVlakne = filePaths.Length / num_threads;

         Console.WriteLine(pocetObrazkuVeVlakne);

         Thread[] threads = new Thread[num_threads];
         for (int i = 0; i < num_threads; ++i)
         {
             threads[i] = new Thread(Work);
             threads[i].Start(i);
         }


         for (int i = 0; i < num_threads; ++i)
         {
             threads[i].Join();
         }

         void Work(object arg)
         {
             Console.WriteLine("Thread #" + arg + " has begun...");

             // calculate my working range [start, end)
             int id = (int)arg;
             int mystart = id * pocetObrazkuVeVlakne;
             int myend = (id + 1) * pocetObrazkuVeVlakne;

             // start work on my range !!
             for (int i = mystart; i < myend; ++i)
                 orezObrazek(i, filePaths.Length, filePaths[i]);

             // Console.WriteLine("Thread #" + arg + " Element " + filePaths[i]);
         }
        foreach (string obrazek in filePaths)
        {
            orezObrazek(u, filePaths.Length, obrazek);
            //RozrezObrazek(obrazek);
            u++;
            if (u > 200)
            {
                break;
            }
        }
        hodiny.Stop();
        Console.WriteLine("Celej set o " + filePaths.Length + " obrazcich trval: " + hodiny.Elapsed.TotalMinutes);
            */



        static void UdelejModrejOkraj(string path, int u)
        {
            Bitmap image = (Bitmap)Bitmap.FromFile(path);
            Bitmap imagePuvodni = (Bitmap)Bitmap.FromFile(path);

            ColorFiltering filterBlue = new ColorFiltering();
            filterBlue.Red = new IntRange(18, 35);
            filterBlue.Green = new IntRange(23, 40);
            filterBlue.Blue = new IntRange(40, 85);



            filterBlue.ApplyInPlace(image);
            //image.Save("modra orezana.png");

            GaussianBlur filterBlur = new GaussianBlur(4, 11);
            filterBlur.ApplyInPlace(image);
            //image.Save("modra Blur.png");






            //blobCounter.ProcessImage(image);
            Bitmap imageGray = PreProcess(image);
            // imageGray.Save("gray3.png");

            BlobCounter bc = new BlobCounter();

            bc.FilterBlobs = true;
            bc.MinHeight = 500;
            bc.MinWidth = 500;


            bc.ProcessImage(image);

            Blob[] blobs = bc.GetObjectsInformation();

            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            foreach (var blob in blobs)
            {
                List<IntPoint> edgePoints = bc.GetBlobsEdgePoints(blob);
                List<IntPoint> cornerPoints;

                // use the shape checker to extract the corner points
                if (shapeChecker.IsQuadrilateral(edgePoints, out cornerPoints))
                {
                    // only do things if the corners form a rectangle
                    if (shapeChecker.CheckPolygonSubType(cornerPoints) == PolygonSubType.Rectangle)
                    {
                        // here i use the graphics class to draw an overlay, but you
                        // could also just use the cornerPoints list to calculate your
                        // x, y, width, height values.
                        List<System.Drawing.Point> Points = new List<System.Drawing.Point>();
                        foreach (var point in cornerPoints)
                        {
                            Points.Add(new System.Drawing.Point(point.X, point.Y));
                        }

                        /* uauauauauauaua */
                        System.Drawing.Point[] poleRohu = new System.Drawing.Point[Points.Count];
                        poleRohu = Points.ToArray();

                        /* Tady zpracuju rohove body a vypocitam si vzdalenosti */
                        int novaSirka = (poleRohu[2].X - poleRohu[0].X);
                        int novaVyska = (poleRohu[3].Y - poleRohu[1].Y);
                        int noveX = poleRohu[0].X;
                        int noveY = poleRohu[1].Y;

                        // vyrezu obrazek
                        Rectangle vyrez = new Rectangle(noveX, noveY, novaSirka, novaVyska);
                        // ulozim vyrezany
                        Bitmap vyrezanyObraz = imagePuvodni.Clone(vyrez, imagePuvodni.PixelFormat);

                        vyrezanyObraz.Save("output\\orezany" + u + ".png", ImageFormat.Jpeg);


                        Graphics g = Graphics.FromImage(image);
                        g.DrawPolygon(new Pen(Color.Red, 5.0f), poleRohu);
                        // image.Save("result.png");


                    }
                }
            }


        }
        static void RozrezObrazek(string inputPath)
        {
            using (var image = new Bitmap(System.Drawing.Image.FromFile(inputPath)))
            {
                int sizeWidthSmall = image.Width / 11;
                int sizeHeightSmall = image.Height / 10;

                Rectangle vyrez = new Rectangle(0, 0, sizeWidthSmall, sizeHeightSmall);
                // ulozim vyrezany
                Bitmap vyrezanyObraz = image.Clone(vyrez, image.PixelFormat);
                vyrezanyObraz.Save(("output\\" + inputPath), ImageFormat.Jpeg);
            }

        }





        static void orezObrazek(int i, int totalCount, string inputPath)
        {
            //zacnu merit time 
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //tady jedu kod
            using (var image = new Bitmap(System.Drawing.Image.FromFile(inputPath)))
            {
                int halfWidth = Convert.ToInt32(image.Width / 2);
                int halfHeight = Convert.ToInt32(image.Height / 2);

                int hladina = 48;
                int hladina2 = 25;

                int[,] modreBody = new int[4, 2];

                /* V teto casti najdu krajni modre body */

                // jdu z leva a jdu po sirce
                for (int x = halfWidth; x > 0; x--)
                {
                    // Console.WriteLine("Pixel: " + x +  " barvy "+ image.GetPixel(x, halfHeight).B + " modra, " + image.GetPixel(x, halfHeight).R + " cervena, " +image.GetPixel(x, halfHeight).G + "zelena.");
                    if (image.GetPixel(x, halfHeight).R < 30 && image.GetPixel(x, halfHeight).G < 30 && image.GetPixel(x, halfHeight).B > image.GetPixel(x, halfHeight).G)
                    {
                        modreBody[0, 0] = x;
                        modreBody[0, 1] = halfHeight;
                        break;
                    }
                }
                // jdu z prava a jdu po sirce
                for (int x = halfWidth; x < (image.Width - 1); x++)
                {
                    if (image.GetPixel(x, halfHeight).R < 30 && image.GetPixel(x, halfHeight).G < 30 && image.GetPixel(x, halfHeight).B > image.GetPixel(x, halfHeight).G)
                    {
                        modreBody[1, 0] = x;
                        modreBody[1, 1] = halfHeight;
                        break;
                    }
                }
                // jdu z vrchu a jdu dolu
                for (int y = 0; y < halfHeight; y++)
                {
                    if (image.GetPixel(halfWidth, y).B > image.GetPixel(halfWidth, y).G)
                    {
                        modreBody[2, 0] = halfWidth;
                        modreBody[2, 1] = y;
                        break;
                    }
                }
                // jdu ze spodu a jdu nahoru
                for (int y = (image.Height - 1); y > halfHeight; y--)
                {
                    if (image.GetPixel(halfWidth, y).B > image.GetPixel(halfWidth, y).G)
                    {
                        modreBody[3, 0] = halfWidth;
                        modreBody[3, 1] = y;
                        break;
                    }
                }
                /* KONEC V teto casti najdu krajni modre body */

                /* Tady zpracuju rohove body a vypocitam si vzdalenosti */
                int novaSirka = (modreBody[1, 0] - modreBody[0, 0]);
                int novaVyska = (modreBody[3, 1] - modreBody[2, 1]);
                int noveX = modreBody[0, 0];
                int noveY = modreBody[2, 1];

                // vyrezu obrazek
                Rectangle vyrez = new Rectangle(noveX, noveY, novaSirka, novaVyska);
                // ulozim vyrezany
                Bitmap vyrezanyObraz = image.Clone(vyrez, image.PixelFormat);
                vyrezanyObraz.Save(("output\\01.jpg"), ImageFormat.Jpeg);
            }
            watch.Stop();
            Console.WriteLine("Jeden obrazek trval: " + watch.Elapsed.TotalSeconds + " je to obrazek " + i + "/" + totalCount + ".");
            //var elapsedMs = watch.ElapsedMilliseconds;
        }
    }
}

/* POznamky 
    Modra musí byt menší než 230
     //LH - levy horni
                //PH - pravy horni
                //LD - levy dolni
                //PD - pravy dolni
                Rectangle cropLH = new Rectangle(0, 0, 25, 25);
                Rectangle cropLD = new Rectangle(0, halfHeight, halfWidth, halfHeight);
                Rectangle cropPH = new Rectangle(halfWidth, 0, halfWidth, halfHeight);
                Rectangle cropPD = new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight);

                //rozrezano na ctvrtiny
                Bitmap croppedLH = image.Clone(cropLH, image.PixelFormat);
                int x;
                // blok pro nalezeni hran nejdriv jdu po sirce od nuly a zastavim se na prvni modre a pak to stejny na vysce
                for ( x = 0 ; x < croppedLH.Width; x++)
                {
                    if (croppedLH.GetPixel(x, croppedLH.Height-1).B > 230)
                    {   
                        Console.WriteLine("modara");
                        break;
                    }
                     Console.WriteLine("jedu");
                }
                Console.WriteLine(x);

                Bitmap croppedLD = image.Clone(cropLD, image.PixelFormat);
                Bitmap croppedPH = image.Clone(cropPH, image.PixelFormat);
                Bitmap croppedPD = image.Clone(cropPD, image.PixelFormat);

            
                croppedLH.Save("01.jpg", ImageFormat.Jpeg);
                croppedLD.Save("02.jpg", ImageFormat.Jpeg);
                croppedPH.Save("03.jpg", ImageFormat.Jpeg);
                croppedPD.Save("04.jpg", ImageFormat.Jpeg);


                
                
                
            
                int[,] modreRohy = new int[4, 2];
                //Levy horni
                modreRohy[0, 0] = modreBody[0, 0];
                modreRohy[0, 1] = modreBody[2, 1];
                //Pravy horni
                modreRohy[1, 0] = modreBody[1, 0];
                modreRohy[1, 1] = modreBody[2, 1];
                //Levy dolni
                modreRohy[2, 0] = modreBody[0, 0];
                modreRohy[2, 1] = modreBody[3, 1];
                //pravy dolni
                modreRohy[3, 0] = modreBody[1, 0];
                modreRohy[3, 1] = modreBody[3, 1];

                Console.WriteLine("X " + modreRohy[0, 0] + " Y " + modreRohy[0, 1]);

                Console.WriteLine("X " + modreRohy[1, 0] + " Y " + modreRohy[1, 1]);

                Console.WriteLine("X " + modreRohy[2, 0] + " Y " + modreRohy[2, 1]);

                Console.WriteLine("X " + modreRohy[3, 0] + " Y " + modreRohy[3, 1]);

 */
