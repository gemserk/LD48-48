using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class MineCartHud : MonoBehaviour
    {
        // public Text

        public TMP_Text text;

        private Material fontMaterial;
        
        private void Awake()
        {
            fontMaterial = new Material(text.fontSharedMaterial);
        }

        public void SetTextColor(Color color)
        {
            fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, color * 4.0f);
            text.fontSharedMaterial = fontMaterial;
        }
    }
}