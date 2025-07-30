using MelonLoader;
using UnityEngine;
using System.Collections;

namespace ConnieCustom
{
    public class Main : MelonMod
    {
        public static MelonPreferences_Category config;
        public static MelonPreferences_Entry<bool> prefDisableNewGameButton;
        public static MelonPreferences_Entry<bool> prefDisableHubClouds;
        public static MelonPreferences_Entry<bool> prefDisableResetToDefaultButton;
        public static MelonPreferences_Entry<bool> prefOverrideBeliversPark;

        public override void OnInitializeMelon()
        {
            config = MelonPreferences.CreateCategory("conniemods", "connie modpack");
            prefDisableNewGameButton = config.CreateEntry("prefDisableNewGameButton", true, "NoMoreNewFile", "requires restart");
            prefDisableResetToDefaultButton = config.CreateEntry("prefDisableResetToDefaultButton", true, "Disable Reset to Default Button", "disables the 'reset to defaults' button in game options");
            prefOverrideBeliversPark = config.CreateEntry("prefOverrideBelieversPark", true, "Override Believers Park Button", "makes the believers park button in the hub open global instead of going to believers park\nrequires restart");
            prefDisableHubClouds = config.CreateEntry("prefDisableHubClouds", true, "Disable Hub Clouds");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"Scene loaded: {sceneName}");

            MelonCoroutines.Start(DisableNewGameButtonCoroutine());
            MelonCoroutines.Start(DisableHubCloudsCoroutine());
            MelonCoroutines.Start(OverrideBelieversParkButton());
            MelonCoroutines.Start(DisableResetToDefaultsCoroutine());
        }

        private IEnumerator DisableNewGameButtonCoroutine()
        {
            yield return new WaitForSeconds(1f);

            if (prefDisableNewGameButton.Value)
            {
                var buttonObj = GameObject.Find("/Main Menu/Canvas/Main Menu/Panel/Title Panel/Title Buttons/New Game Button");

                if (buttonObj != null)
                {
                    buttonObj.SetActive(false);
                    MelonLogger.Msg("new game button nuked");
                }
                else
                {
                    MelonLogger.Warning("new game button couldnt be found");
                }
            }
            else
            {
                MelonLogger.Msg("new game button chosen not to be nuked (mlpref disabled)");
            }
        }
        private IEnumerator DisableResetToDefaultsCoroutine()
        {
            yield return new WaitForSeconds(1f);

            if (prefDisableResetToDefaultButton.Value)
            {
                var resetButtonObj = GameObject.Find("/Main Menu/Canvas/Ingame Menu/Menu Holder/Options Panel/OptionsGeneralPanel/OptionsGeneral/Reset to Defaults Button/");

                if (resetButtonObj != null)
                {
                    resetButtonObj.SetActive(false);
                    MelonLogger.Msg("reset to defaults button nuked");
                }
                else
                {
                    MelonLogger.Warning("reset to defaults button couldnt be found");
                }
            }
            else
            {
                MelonLogger.Msg("reset to defaults button chosen not to be nuked (mlpref disabled)");
            }
        }

        private IEnumerator DisableHubCloudsCoroutine()
        {
            yield return new WaitForSeconds(0f);

            if (prefDisableHubClouds.Value)
            {
                var cloudObj = GameObject.Find("/Central Heaven Map/cloudPlane (72)");
                var cloud2Obj = GameObject.Find("/Central Heaven Map/cloudPlane (73)");
                var fgCloudObj = GameObject.Find("/Central Heaven Map/FG Clouds");
                var rocksObj = GameObject.Find("/Central Heaven Map/BG Rocks");

                if (cloudObj != null & cloud2Obj != null & fgCloudObj != null & rocksObj != null)
                {
                    cloudObj.SetActive(false);
                    cloud2Obj.SetActive(false);
                    fgCloudObj.SetActive(false);
                    rocksObj.SetActive(false);
                    MelonLogger.Msg("hub clouds nuked");
                }
                else
                {
                    MelonLogger.Warning("a hub cloud object couldnt be found");
                }
            }
            else
            {
                MelonLogger.Msg("hub clouds chosen to not be nuked (mlpref disabled)");
            }
        
        }
        private IEnumerator OverrideBelieversParkButton()
        {

            yield return new WaitForSeconds(1f);

            if (prefOverrideBeliversPark.Value)
            {

                var believersButtonObj = GameObject.Find("/Main Menu/Canvas/Main Menu/Panel/Map Panel/Panel/Map Button City Square/Button/");
                if (believersButtonObj == null)
                {
                    MelonLogger.Warning("park button not found");
                    yield break;
                }

                var button = believersButtonObj.GetComponent<UnityEngine.UI.Button>();
                if (button == null)
                {
                    MelonLogger.Warning("park object doesnt have button component");
                    yield break;
                }

                var believersTextObj = GameObject.Find("/Main Menu/Canvas/Main Menu/Panel/Map Panel/Panel/Map Button City Square/Button/Text/");
                if (believersTextObj == null)
                {
                    MelonLogger.Warning("park text object not found");
                    yield break;
                }

                var textComponent = believersTextObj.GetComponent<TMPro.TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = "Global Leaderboard";
                    MelonLogger.Msg("park button text updated!!");
                }
                else
                {
                    MelonLogger.Warning("text mesh component not found on park text object");
                }

                var InteriorPanel = GameObject.Find("/Main Menu/Canvas/Main Menu/Panel/Map Panel/Panel/Map Button City Square/Button/Interior Panel");
                if (InteriorPanel == null)
                {
                    MelonLogger.Warning("interior panel object could not be found");
                    yield break;
                }
                else
                {
                    InteriorPanel.SetActive(false);
                    MelonLogger.Msg("interior panel disalbed");
                }


                button.onClick.RemoveAllListeners();


                button.onClick.AddListener(() =>
                {
                    var mainMenu = GameObject.FindObjectOfType<MainMenu>();
                    if (mainMenu != null)
                    {
                        mainMenu.SetState(MainMenu.State.GlobalNeonScore);
                        mainMenu.SetBackButtonState(MainMenu.State.Map);
                        MelonLogger.Msg("opened global");
                    }
                    else
                    {
                        MelonLogger.Warning("mainmenu not found somehow ?");
                    }
                });

                MelonLogger.Msg("park button fully overridden :fire:");
            }
            else
            {
                MelonLogger.Msg("park button not overridden (mlpref disabled)");
            }

        }

    }
}