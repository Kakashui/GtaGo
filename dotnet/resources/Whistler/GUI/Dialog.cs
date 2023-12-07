using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.GUI
{
    public class DialogUI : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DialogUI));
        private static readonly Dictionary<ExtPlayer, Dialog> DialogByPlayer = new Dictionary<ExtPlayer, Dialog>();

        public static void Open(ExtPlayer player, string header, List<ButtonSetting> buttonSettings)
        {
            try
            {
                if (DialogByPlayer.ContainsKey(player))
                {
                    DialogByPlayer.Remove(player);
                }

                DialogByPlayer.Add(player, new Dialog(buttonSettings));

                SafeTrigger.ClientEvent(player,"dialogServer:open", header, JsonConvert.SerializeObject(buttonSettings));
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on dialog:buttonClick error: {e}"); }
        }

        [RemoteEvent("dialog:buttonClick")]
        public void HandleDialogButtonClick(ExtPlayer player, int index)
        {
            try
            {
                if (DialogByPlayer.ContainsKey(player))
                {
                    DialogByPlayer[player].ActivateButton(player, index);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on dialog:buttonClick error: {e}"); }
        }

        public class ButtonSetting
        {
            public string Name { get; set; }
            public string Icon { get; set; }

            [JsonIgnore]
            public Action<ExtPlayer> Action { get; set; }
        }

        private class Dialog
        {
            private List<ButtonSetting> _buttons = new List<ButtonSetting>();

            public Dialog(List<ButtonSetting> buttons)
            {
                _buttons = buttons;
            }

            public void ActivateButton(ExtPlayer player, int buttonIndex)
            {
                _buttons[buttonIndex].Action?.Invoke(player);
            }
        }
    }
}
