using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class MineCartHud : MonoBehaviour
    {
        public TMP_Text text;

        private Material fontMaterial;

        public float colorIntensity = 4.0f;
        public float spacing = 60;
        
        private void Awake()
        {
            fontMaterial = new Material(text.fontSharedMaterial);
        }

        public void SetTextColor(Color color)
        {
            fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, color * colorIntensity);
            text.fontSharedMaterial = fontMaterial;
        }

        public void SetPoints(float points)
        {
            text.text = $"<mspace=mspace={spacing}>{Mathf.Round(points)}</mspace>";
        }
    }
}