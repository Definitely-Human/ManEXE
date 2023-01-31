using System;
using System.Collections;
using UnityEngine;

namespace ManExe.UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        public static readonly string FlagOn = "On";
        public static readonly string FlagOff = "Off";
        public static readonly string FlagNone = "None";

        [field: SerializeField] public MenuType MType { get; private set; }
        [SerializeField] private bool useAnimation;
        private Animator menuAnimator;
        
        
        public string TargetState { get; private set; }

        #region Unity Functions

        private void OnEnable()
        {
            
        }

        #endregion

        #region Public Functions

        public void Animate(bool on)
        {
            if (useAnimation)
            {
                menuAnimator.SetBool("on",on);
                StopCoroutine("AwaitAnimation");
                StartCoroutine("AwaitAnimation", on);
            }
            else
            {
                if (!on)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        #endregion

        #region Private Functions

        private IEnumerator AwaitAnimation(bool on)
        {
            TargetState = on ? FlagOn : FlagOff;
            
            //Wait for animator to reach the target state 
            while (!menuAnimator.GetCurrentAnimatorStateInfo(0).IsName(TargetState))
            {
                yield return null;
            }
            
            //Wait for animator to finish animating
            while (menuAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

            TargetState = FlagNone;

            if (!on)
            {
                gameObject.SetActive(false);
            }
        }

        private void CheckAnimatorIntegrity()
        {
            if (useAnimation)
            {
                menuAnimator = GetComponent<Animator>();
                if (menuAnimator == null)
                {
                    Debug.LogWarning($"Trying to animate a page [{MType}] but no animator exists on the object");
                }
            }
        }

        #endregion
    }
    
}
