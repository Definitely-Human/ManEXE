using UnityEngine;

namespace ManExe.UI.Menu
{
    public class ButtonMainMenuExit : MonoBehaviour, IMenuChanger
    {
        [SerializeField] private GameObject DynamicBg;
        [SerializeField] private GameObject StaticBg;
        public void OnMenuExit()
        {
            DynamicBg.SetActive(false);
            StaticBg.SetActive(false);
        }
    }
}
