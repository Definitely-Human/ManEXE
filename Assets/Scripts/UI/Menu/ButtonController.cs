using UnityEngine;
using UnityEngine.UI;

namespace ManExe.UI.Menu
{
    [RequireComponent(typeof(Button))]
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private MenuType desiredMenuType;
        [SerializeField] private MenuType closedMenuType;
        [SerializeField] private Component menuChanger;

        private MenuManager menuManager;
        private Button menuButton;
        void Start()
        {
            menuButton = GetComponent<Button>();
            menuButton.onClick.AddListener(OnButtonClicked);
            menuManager = MenuManager.GetInstance();
        }

        private void OnButtonClicked()
        {
            if (menuChanger != null)
            {
                var changer = menuChanger as IMenuChanger;
                if (changer != null)
                    changer.OnMenuExit();
            }

            
            menuManager.TurnMenuOff(closedMenuType,desiredMenuType,false);
        }
    }
}
