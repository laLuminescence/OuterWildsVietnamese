using HarmonyLib;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static OuterWildsVietnamese.SliderPatch;

namespace OuterWildsVietnamese
{
    [HarmonyPatch]
    public static class FontPatch
    {
        

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(TitleScreenManager), nameof(TitleScreenManager.OnTitleMenuAnimationComplete))]
        //public static bool OnTitleMenuAnimationComplete(TitleScreenManager __instance)
        //{
        //    if (OuterWildsVietnamese.menuFont != null)
        //        for (int i = 0; i < __instance._mainMenuTextFields.Length; i++)
        //        {
        //            __instance._mainMenuTextFields[i].font = OuterWildsVietnamese.menuFont;
        //        }
        //    return true;
        //}

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(Menu), nameof(Menu.InitializeMenu))]
        //public static void InitializeMenuPostfix(Menu __instance)
        //{
        //    MenuOption[] list = __instance.GetMenuOptions();
        //    if (OuterWildsVietnamese.menuFont != null)
        //        foreach (MenuOption option in list)
        //        {
        //            option.GetLabelField().font = OuterWildsVietnamese.menuFont;
        //            //Luminescence090923: Can't change the font of SecondaryTextField and TooltipDisplay

        //            //if (option.GetSecondaryTextField().font != null)
        //            //    option.GetSecondaryTextField().font = OuterWildsVietnamese.menuFont;
        //            //option._menuTooltipDisplay._textDisplay.font = OuterWildsVietnamese.menuFont;
        //        }
        //}

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(Menu), nameof(Menu.Activate))]
        //public static void ActivatePostfix(Menu __instance)
        //{
        //    MenuOption[] list = __instance.GetMenuOptions();
        //    if (OuterWildsVietnamese.menuFont != null)
        //        foreach (MenuOption option in list)
        //        {
        //            option.GetLabelField().font = OuterWildsVietnamese.menuFont;
        //            //Luminescence090923: Can't change the font of SecondaryTextField and TooltipDisplay

        //            //if (option.GetSecondaryTextField().font != null)
        //            //    option.GetSecondaryTextField().font = OuterWildsVietnamese.menuFont;
        //            //option._menuTooltipDisplay._textDisplay.font = OuterWildsVietnamese.menuFont;
        //        }
        //}

        ////GameOver Font patch
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(GameOverController), nameof(GameOverController.SetupGameOverScreen))]
        //public static bool SetupGameOverScreenPrefix(GameOverController __instance)
        //{
        //    if (OuterWildsVietnamese.menuFont != null)
        //        __instance._deathText.font = OuterWildsVietnamese.menuFont;
        //    else
        //        OuterWildsVietnamese.Log("Main Menu Font (GameOver) null");
        //    return true;
        //}

        //Conversation Font patch
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DialogueBoxVer2), "InitializeFont")]
        public static bool InitializeFontPrefix(DialogueBoxVer2 __instance)
        {
            if (OuterWildsVietnamese.conversationFont != null)
            {
                //__instance._fontInUse = OuterWildsVietnamese.bundle.LoadAsset<Font>("Assets/MerriweatherSans-SemiBold.ttf");
                __instance._fontInUse = OuterWildsVietnamese.conversationFont;
                __instance._dynamicFontInUse = OuterWildsVietnamese.conversationFont;
            }
            else
            {
                OuterWildsVietnamese.Log("Heathian Font null");
                return true;
            }
            int customFontSize = 25;
            float customLineSpacing = 1.4f;

            //__instance._dynamicFontInUse = __instance._defaultDialogueFontDynamic;
            //Luminescence291123: Somehow _defaultFontSpacing got yeeted in the new update, so I won't be using it, but the old line is kept just in case
            //__instance._fontSpacingInUse = __instance._defaultFontSpacing;


            __instance._mainTextField.font = __instance._fontInUse;
            __instance._mainTextField.lineSpacing = __instance._fontSpacingInUse * customLineSpacing;
            __instance._mainTextField.fontSize = customFontSize;
            __instance._nameTextField.font = __instance._fontInUse;
            __instance._nameTextField.lineSpacing = __instance._fontSpacingInUse * customLineSpacing;
            __instance._nameTextField.fontSize = customFontSize;

            DialogueOptionUI requiredComponent =
                __instance._optionBox.GetRequiredComponent<DialogueOptionUI>();
            requiredComponent.textElement.font = __instance._fontInUse;
            requiredComponent.textElement.lineSpacing = __instance._fontSpacingInUse * customLineSpacing;
            requiredComponent.textElement.fontSize = customFontSize;
            return false;
        }

        //Input Prompt Font patch
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PromptManager), nameof(PromptManager.Awake))]
        public static void PromptManagerPostfix(PromptManager __instance)
        {
            if (OuterWildsVietnamese.promptFont != null)
                __instance._currentFont = OuterWildsVietnamese.promptFont;
            else
                OuterWildsVietnamese.Log("Prompt Font null");
        }


        //Menu, Object and UI Font patch
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FontAndLanguageController), nameof(FontAndLanguageController.InitializeFont))]
        public static void InitFontPostfix(FontAndLanguageController __instance)
        {
            for (int i = 0; i < __instance._textContainerList.Count; i++)
            {
                __instance._textContainerList[i].textElement.font = OuterWildsVietnamese.menuFont;
            }
        }

        //TODO: Make ShipDisplay text bigger (ShipNotificationDisplay)

    }
}
