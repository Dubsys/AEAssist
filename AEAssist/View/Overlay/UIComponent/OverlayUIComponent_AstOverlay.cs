﻿using AEAssist.Helper;
using Buddy.Overlay;
using Buddy.Overlay.Controls;
using ff14bot.Enums;

namespace AEAssist.View.Overlay.UIComponent
{
    [Job(ClassJobType.Scholar)]
    internal class OverlayUIComponent_SchOverlay : OverlayUIComponent
    {
        private OverlayControl _control;

        public OverlayUIComponent_SchOverlay() : base(true)
        {
        }

        public override OverlayControl Control
        {
            get
            {
                if (_control != null)
                    return _control;

                var overlayUc = new SchOverlayWindow();

                _control = new OverlayControl
                {
                    Name = overlayUc.GetType().Name,
                    Content = overlayUc,
                    Width = overlayUc.Width + 5,
                    Height = overlayUc.Height,
                    X = SettingMgr.GetSetting<GeneralSettings>().OverlayPos_X,
                    Y = SettingMgr.GetSetting<GeneralSettings>().OverlayPos_Y,
                    AllowMoving = true,
                    AllowResizing = false
                };
                LogHelper.Info("CreateOverlay " + _control.Width + "  " + _control.Height);
                _control.MouseLeave += (sender, args) =>
                {
                    SettingMgr.GetSetting<GeneralSettings>().OverlayPos_X = _control.X;
                    SettingMgr.GetSetting<GeneralSettings>().OverlayPos_Y = _control.Y;
                };
                _control.MouseLeftButtonDown += (sender, args) =>
                {
                    _control.DragMove();
                };

                return _control;
            }
        }
    }
}
