using UnityEngine ;
using UnityEngine.UI ;

public class SettingsMenuItem : MonoBehaviour {
   [HideInInspector] public Image img ;
   [HideInInspector] public RectTransform rectTrans ;

   //SettingsMenu reference
   SettingsMenu settingsMenu ;

    void Awake()
    {
        img = GetComponent<Image>();
        rectTrans = GetComponent<RectTransform>();

        settingsMenu = rectTrans.parent.GetComponent<SettingsMenu>();

    }
}
