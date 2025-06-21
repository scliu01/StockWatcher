﻿using CSDeskBand;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockWatcher
{
    [ComVisible(true)]
    [Guid("EFD26D5D-1609-4B8A-B51C-843BB9DFD2BE")]
    [CSDeskBand.CSDeskBandRegistration(Name = "任务栏工具", ShowDeskBand = true)]
    public class Deskband : CSDeskBand.CSDeskBandWin
    {
        private static Control _control;

        public Deskband()
        {
            _control = new StockPreviewControl(this);
			Options.MinHorizontalSize.Width = _control.Width;
			Options.MinHorizontalSize.Height = _control.Height;
			//Options.MinHorizontalSize = new Size(_control.Width, _control.Height);
        }

        protected override Control Control => _control;

        protected override void DeskbandOnClosed()
        {
            _control.Dispose();
            _control = null;
            base.DeskbandOnClosed();
        }
    }
}
