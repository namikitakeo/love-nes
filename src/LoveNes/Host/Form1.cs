using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LoveNes.Host;
using LoveNes.IO;

namespace LoveNes
{
    public partial class Form1 : Form
    {
        private readonly NesSystem _nesSystem;
        private uint[] _frontBuffer;
        private uint[] _backBuffer;

        public Form1()
        {
            _frontBuffer = new uint[256 * 240];
            _backBuffer = new uint[256 * 240];
            _nesSystem = new NesSystem(new HostGraphics(this));
            InitializeComponent();
        }

        private class HostGraphics : IHostGraphics
        {
            private readonly Form1 _mainWindow;

            public HostGraphics(Form1 mainWindow)
            {
                _mainWindow = mainWindow;
            }

            void IHostGraphics.DrawPixel(byte x, byte y, uint rgb)
            {
                _mainWindow._backBuffer[y * 256 + x] = rgb;
            }

            unsafe void IHostGraphics.Flip()
            {
                var buffer = _mainWindow._backBuffer;
                _mainWindow._backBuffer = _mainWindow._frontBuffer;
                _mainWindow._frontBuffer = _mainWindow._backBuffer;
                fixed (uint* p = _mainWindow._frontBuffer)
                {
                    var bitmap = new Bitmap(256, 240, 256 * sizeof(uint), System.Drawing.Imaging.PixelFormat.Format32bppRgb, (IntPtr)p);
                    _mainWindow.BackgroundImage = bitmap;
                }
            }
        }
    }
}
