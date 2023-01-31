using System.Collections;
using ManExe.Core;
using UnityEngine;

namespace ManExe.UI.Menu
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField]
        private MenuType entryMenu;
        [SerializeField]
        private MenuController[] menus;

        private Hashtable MenuHashes;
        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            MenuHashes = new Hashtable();
            RegisterAllMenus();

            if (entryMenu != MenuType.None)
            {
                TurnMenuOn(entryMenu);
            }

            
        }

        #endregion

        #region Public Methods

        public void TurnMenuOn(MenuType menuType)
        {
            if (menuType == MenuType.None) return;
            if (!MenuExits(menuType))
            {
                Debug.LogWarning($"Trying to turn on the page of type [{menuType}] that has not been registered.");
                return;
            }

            MenuController menu = GetMenu(menuType);
            menu.gameObject.SetActive(true);
            menu.Animate(true);
        }
        
        public void TurnMenuOff(MenuType off, MenuType on=MenuType.None, bool waitForExit=false)
        {
            if (off == MenuType.None) return;
            if (!MenuExits(off))
            {
                Debug.LogWarning($"Trying to turn off the page of type [{off}] that has not been registered.");
                return;
            }

            MenuController offMenu = GetMenu(off);
            if (offMenu.gameObject.activeSelf)
            {
                offMenu.Animate(false);
            }

            if (on == MenuType.None)
            {
                return;
            }
            MenuController onMenu = GetMenu(on);
            if (waitForExit)
            {
                StartCoroutine(WaitForMenuExit(onMenu,offMenu));
            }
            else
            {
                TurnMenuOn(on);
            }
        }
        #endregion

        #region Private Methods

        private IEnumerator WaitForMenuExit(MenuController on, MenuController off)
        {
            while (off.TargetState != MenuController.FlagNone)
            {
                yield return null;
            }
            TurnMenuOn(on.MType);
        }

        private void RegisterAllMenus()
        {
            foreach (var menu in menus)
            { 
                RegisterMenu(menu);
            }
        }
        
        private void RegisterMenu(MenuController menu)
        {
            if (MenuExits(menu.MType))
            {
                Debug.LogWarning($"Trying to register a page on of type [{menu.MType}]" +
                                 $" that has already been registered: " + menu.gameObject.name);
                return;
            }
            MenuHashes.Add(menu.MType, menu);
        }
        
        private MenuController GetMenu(MenuType menu)
        {
            if (!MenuExits(menu))
            {
                Debug.LogWarning($"Trying to get the page on of type [{menu}] that has not been registered.");
                return null;
            }

            return (MenuController)MenuHashes[menu];
        }

        private bool MenuExits(MenuType menu)
        {
            return MenuHashes.ContainsKey(menu);
        }
        
        #endregion
    }
}
