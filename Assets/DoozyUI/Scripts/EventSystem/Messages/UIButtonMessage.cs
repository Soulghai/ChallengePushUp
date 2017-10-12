// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEngine;

namespace DoozyUI
{
    public class UIButtonMessage : Message
    {
        public bool addToNavigationHistory;
        public bool backButton;
        public string buttonName;
        public List<string> gameEvents;

        public GameObject gameObject;
        public List<string> hideElements;

        public List<string> showElements;
    }
}