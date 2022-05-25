using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace JEMusicAndPylon.UI
{
    internal class PylonMenu : UIState
    {
        private PylonMenuPanel _pylonMenuPanel;

        public override void OnInitialize()
        {
            int pylonCount = JEMusicAndPylonWorld.Instance.PylonCoordinates.Count;
            _pylonMenuPanel = new PylonMenuPanel();
            _pylonMenuPanel.SetPadding(0f);
            _pylonMenuPanel.Left.Set(600f, 0f);
            _pylonMenuPanel.Top.Set(60f, 0f);
            _pylonMenuPanel.Width.Set(455f, 0f);
            _pylonMenuPanel.Height.Set(15f + 45f * pylonCount, 0f);
            _pylonMenuPanel.BackgroundColor = new Color(73, 94, 171);
            _pylonMenuPanel.AddPylons();
            Append(_pylonMenuPanel);
        }
    }
}
