using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;



namespace MeadowClockGraphics
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        St7735 st7735;
        GraphicsLibrary graphics;

        public MeadowApp()
        {
            var config = new SpiClockConfiguration(6000,
                SpiClockConfiguration.Mode.Mode3);
            st7735 = new St7735
            (
                device: Device,
                spiBus: Device.CreateSpiBus(
                    Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 128, height: 160
            );

            graphics = new GraphicsLibrary(st7735);
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            DrawShapes();
        }

        void DrawShapes()
        {
            Random rand = new Random();
            int displayWidth = 128;
            int displayHeight = 160;

            graphics.Clear(true);

            int radius = 10;
            int originX = displayWidth / 2;
            int originY = displayHeight / 2;
            for (int i = 1; i < 5; i++)
            {
                graphics.DrawCircle
                (
                    centerX: originX,
                    centerY: originY,
                    radius: radius,
                    color: Color.FromRgb(
                        rand.Next(255), rand.Next(255), rand.Next(255))
                );
                graphics.Show();
                radius += 30;
            }

            int sideLength = 30;
            for (int i = 1; i < 5; i++)
            {
                graphics.DrawRectangle
                (
                    x: (displayWidth - sideLength) / 2,
                    y: (displayHeight - sideLength) / 2,
                    width: sideLength,
                    height: sideLength,
                    color: Color.FromRgb(
                        rand.Next(255), rand.Next(255), rand.Next(255))
                );
                graphics.Show();
                sideLength += 60;
            }

            graphics.DrawLine(0, displayHeight / 2, displayWidth, displayHeight / 2,
                Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255)));
            graphics.DrawLine(displayWidth / 2, 0, displayWidth / 2, displayHeight,
                Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255)));
            graphics.DrawLine(0, 0, displayWidth, displayHeight,
                Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255)));
            graphics.DrawLine(0, displayHeight, displayWidth, 0,
                Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255)));
            graphics.Show();

            Thread.Sleep(5000);
        }
    }
}
