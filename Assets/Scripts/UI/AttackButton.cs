using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class AttackButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Image filledImage;
                
        private void OnValidate()
        {
            button ??= GetComponent<Button>();
        }

        public void SubscribeOnClick(UnityAction clicked) => button.onClick.AddListener(clicked);

        public void UpdateInteractable(bool value)
        {
            button.interactable = value;
            UpdateColor(icon, value ? 1 : 0.5f);
        }

        private void UpdateColor(Graphic graphic, float value)
        {
            var color = graphic.color;
            color.a = value;
            graphic.color = color;
        }

        public void UpdateCooldown(float value)
        {
            value = Mathf.Clamp01(value);
            filledImage.fillAmount = value;
        }
    }
}