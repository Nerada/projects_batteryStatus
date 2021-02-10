// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.tests.IconHandlerT.cs
// Created on: 20201207
// -----------------------------------------------

using System.Drawing.Imaging;

using BatteryStatus.IconHandling;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BatteryStatus.tests.IconHandling
{
    [TestClass]
    public class IconHandlerT
    {
        private readonly IconHandler _iconHandler = new IconHandler();

        [TestMethod]
        public void GenerateIcon()
        {
            //_iconHandler.IsCharging = true;
            //_iconHandler.StayAwake = true;
            //_iconHandler.Percentage = 66;

            //var image = _iconHandler.Draw();

            //image.Save("iconImage.png", ImageFormat.Png);
        }
    }
}