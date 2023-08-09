using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tool.Scripts.UI.ButtonManager
{
    /// <summary>
    /// ���ư�ť����ʾ����ʧ
    /// </summary>
    public class ButtonsShowControl
    {
        private List<Button> showButtons;

        public ButtonsShowControl(List<Button> btns)
        {
            showButtons = btns;
            foreach (var b in showButtons)
            {
                b.onClick.AddListener(() => { ButtonsShowOnClick(b); });
            }
        }

        /// <summary>
        /// ����һ����ť����
        /// </summary>
        public void ButtonsShowOnClick(Button btn)
        {
            int index = showButtons.IndexOf(btn);
            for (int i = 0; i < showButtons.Count; i++)
            {
                if (i == index)
                {
                    showButtons[i].gameObject.SetActive(false);
                }
                else
                {
                    showButtons[i].gameObject.SetActive(true);
                }
            }
        }

    }
}
