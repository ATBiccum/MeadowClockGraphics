/*MeadowClockGraphics project
 * 
 * Tony Biccum
 * October 2nd, 2021
 * Program written for ECET 230 course at Camosun College
 * Based on https://github.com/WildernessLabs/Meadow_Project_Samples/tree/main/source/Hackster/MeadowClockGraphics 
 * 
 * Progression notes:
 * 
 * ST7735 screen would not display circles in appropririate centered resolution from function DrawShapes().
 * Solution: Added the code example http://developer.wildernesslabs.co/docs/api/Meadow.Foundation/Meadow.Foundation.Displays.TftSpi.St7735.html
 * under new function DrawShapesTest. Also, added updated screen information commented below.
 * 
 * 
 * 
 */





using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading;

namespace MeadowClockGraphics
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        readonly Color WatchBackgroundColor = Color.White;

        St7735 st7735;
        GraphicsLibrary graphics;
        int displayWidth, displayHeight;
        int hour, minute, second, tick;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            var config = new SpiClockConfiguration(6000, SpiClockConfiguration.Mode.Mode3);
            st7735 = new St7735
            (
                device: Device,
                //spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.COPI, Device.Pins.CIPO, config),   //Changed MOSI and MISO To COPI and CIPO, new pin names
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 128, height: 160, St7735.DisplayType.ST7735R                                         //Added "St7735.DisplayType.ST7735R" from meadow website
            );
            displayWidth = Convert.ToInt32(st7735.Width);
            displayHeight = Convert.ToInt32(st7735.Height);

            graphics = new GraphicsLibrary(st7735); 
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            led.SetColor(RgbLed.Colors.Green);
            //DrawShapesTest();
            DrawShapes();            
            //DrawTexts();
            //DrawClock();
        }
        void DrawShapesTest()
        {
            graphics.Clear();

            graphics.DrawCircle(60, 60, 20, Color.Purple);
            graphics.DrawRectangle(10, 10, 30, 60, Color.Red);
            graphics.DrawTriangle(20, 20, 10, 70, 60, 60, Color.Green);

            graphics.DrawCircle(90, 60, 20, Color.Cyan, true);
            graphics.DrawRectangle(100, 100, 30, 10, Color.Yellow, true);
            graphics.DrawTriangle(120, 20, 110, 70, 160, 60, Color.Pink, true);

            graphics.DrawLine(10, 120, 110, 130, Color.SlateGray);

            graphics.Show();
        }
        void DrawShapes()
        {
            Random rand = new Random();

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
                    color: Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255))
                );
                graphics.Show();
                radius += 10;       //Changed from 30 to 10 to adjust for smaller screen
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
                    color: Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255))
                );
                graphics.Show();
                sideLength += 10;   //Changed from 60 to 10 to adjust for smaller screen
            }

            graphics.DrawLine(0, displayHeight / 2, displayWidth, displayHeight / 2,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.DrawLine(displayWidth / 2, 0, displayWidth / 2, displayHeight,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.DrawLine(0, 0, displayWidth, displayHeight,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.DrawLine(0, displayHeight, displayWidth, 0,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.Show();

            Thread.Sleep(5000);
        }

        void DrawTexts()
        {
            graphics.Clear(true);

            int indent = 20;
            int spacing = 20;
            int y = 5;

            graphics.CurrentFont = new Font12x16();
            graphics.DrawText(indent, y, "Meadow F7 SPI ST7735!!");
            graphics.DrawText(indent, y += spacing, "Red", Color.Red);
            graphics.DrawText(indent, y += spacing, "Purple", Color.Purple);
            graphics.DrawText(indent, y += spacing, "BlueViolet", Color.BlueViolet);
            graphics.DrawText(indent, y += spacing, "Blue", Color.Blue);
            graphics.DrawText(indent, y += spacing, "Cyan", Color.Cyan);
            graphics.DrawText(indent, y += spacing, "LawnGreen", Color.LawnGreen);
            graphics.DrawText(indent, y += spacing, "GreenYellow", Color.GreenYellow);
            graphics.DrawText(indent, y += spacing, "Yellow", Color.Yellow);
            graphics.DrawText(indent, y += spacing, "Orange", Color.Orange);
            graphics.DrawText(indent, y += spacing, "Brown", Color.Brown);
            graphics.Show();

            Thread.Sleep(5000);
        }

        void DrawClock()
        {
            graphics.Clear(true);

            hour = 8;
            minute = 54;
            DrawWatchFace();
            while (true)
            {
                tick++;
                Thread.Sleep(1000);
                UpdateClock(second: tick % 60);
            }
        }
        void DrawWatchFace()
        {
            graphics.Clear();
            int hour = 12;
            int xCenter = displayWidth / 2;
            int yCenter = displayHeight / 2;
            int x, y;

            graphics.DrawRectangle(0, 0, displayWidth, displayHeight, Color.White);
            graphics.DrawRectangle(5, 5, displayWidth - 10, displayHeight - 10, Color.White);

            graphics.CurrentFont = new Font12x20();
            graphics.DrawCircle(xCenter, yCenter, 100, WatchBackgroundColor, true);
            for (int i = 0; i < 60; i++)
            {
                x = (int)(xCenter + 80 * Math.Sin(i * Math.PI / 30));
                y = (int)(yCenter - 80 * Math.Cos(i * Math.PI / 30));

                if (i % 5 == 0)
                {
                    graphics.DrawText(hour > 9 ? x - 10 : x - 5, y - 5, hour.ToString(), Color.Black);
                    if (hour == 12) hour = 1; else hour++;
                }
            }

            graphics.Show();
        }
        void UpdateClock(int second = 0)
        {
            int xCenter = displayWidth / 2;
            int yCenter = displayHeight / 2;
            int x, y, xT, yT;

            if (second == 0)
            {
                minute++;
                if (minute == 60)
                {
                    minute = 0;
                    hour++;
                    if (hour == 12)
                    {
                        hour = 0;
                    }
                }
            }

            graphics.Stroke = 3;

            //remove previous hour
            int previousHour = (hour - 1) < -1 ? 11 : (hour - 1);
            x = (int)(xCenter + 43 * Math.Sin(previousHour * Math.PI / 6));
            y = (int)(yCenter - 43 * Math.Cos(previousHour * Math.PI / 6));
            xT = (int)(xCenter + 3 * Math.Sin((previousHour - 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousHour - 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            xT = (int)(xCenter + 3 * Math.Sin((previousHour + 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousHour + 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            //current hour
            x = (int)(xCenter + 43 * Math.Sin(hour * Math.PI / 6));
            y = (int)(yCenter - 43 * Math.Cos(hour * Math.PI / 6));
            xT = (int)(xCenter + 3 * Math.Sin((hour - 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((hour - 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            xT = (int)(xCenter + 3 * Math.Sin((hour + 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((hour + 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            //remove previous minute
            int previousMinute = minute - 1 < -1 ? 59 : (minute - 1);
            x = (int)(xCenter + 55 * Math.Sin(previousMinute * Math.PI / 30));
            y = (int)(yCenter - 55 * Math.Cos(previousMinute * Math.PI / 30));
            xT = (int)(xCenter + 3 * Math.Sin((previousMinute - 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousMinute - 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            xT = (int)(xCenter + 3 * Math.Sin((previousMinute + 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousMinute + 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            //current minute
            x = (int)(xCenter + 55 * Math.Sin(minute * Math.PI / 30));
            y = (int)(yCenter - 55 * Math.Cos(minute * Math.PI / 30));
            xT = (int)(xCenter + 3 * Math.Sin((minute - 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((minute - 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            xT = (int)(xCenter + 3 * Math.Sin((minute + 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((minute + 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            //remove previous second
            int previousSecond = second - 1 < -1 ? 59 : (second - 1);
            x = (int)(xCenter + 70 * Math.Sin(previousSecond * Math.PI / 30));
            y = (int)(yCenter - 70 * Math.Cos(previousSecond * Math.PI / 30));
            graphics.DrawLine(xCenter, yCenter, x, y, WatchBackgroundColor);
            //current second
            x = (int)(xCenter + 70 * Math.Sin(second * Math.PI / 30));
            y = (int)(yCenter - 70 * Math.Cos(second * Math.PI / 30));
            graphics.DrawLine(xCenter, yCenter, x, y, Color.Red);
            graphics.Show();
        }
    }
}
