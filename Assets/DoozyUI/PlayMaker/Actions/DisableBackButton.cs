// Copyright (c) 2015 - 2017 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("Disable the back button by adding another disable level to the additive bool")]
    public class DisableBackButton : FsmStateAction
    {
        public FsmBool debugThis;

        public override void Reset()
        {
            debugThis = new FsmBool { UseVariable = false, Value = false };
        }

        public override void OnEnter()
        {
            UIManager.DisableBackButton();

            if (debugThis.Value)
                Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Disable Back Button");

            Finish();
        }
    }
}
#endif
