using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;

public class SettingsMenu2 : MonoBehaviour {
   [Header ("space between menu items")]
   [SerializeField] Vector2 spacing ;

   [Space]
   [Header ("Main button rotation")]
   [SerializeField] float rotationDuration ;
   [SerializeField] Ease rotationEase ;

   [Space]
   [Header ("Animation")]
   [SerializeField] float expandDuration ;
   [SerializeField] float collapseDuration ;
   [SerializeField] Ease expandEase ;
   [SerializeField] Ease collapseEase ;

   [Space]
   [Header ("Fading")]
   [SerializeField] float expandFadeDuration ;
   [SerializeField] float collapseFadeDuration ;

   [SerializeField] Button mainButton ;
   [SerializeField] SettingsMenuItem2[] menuItems;

    //is menu opened or not
    public bool isExpanded = false ;

   Vector2 mainButtonPosition ;
   int itemsCount;
    Color noAlpha = new Color(1, 1, 1, 1);
    Color alpha = new Color(1, 1, 1, 1);

    void Start () {

        noAlpha.a = 0; // set alpha from buttons
        alpha.a = 1;

        //add all the items to the menuItems array
        itemsCount = transform.childCount;
      menuItems = new SettingsMenuItem2[itemsCount] ;
      for (int i = 0; i < itemsCount; i++) {
         menuItems [ i ] = transform.GetChild (i).GetComponent <SettingsMenuItem2> ();
      }

      mainButton = transform.parent.GetChild(1).GetChild(4).GetComponent <Button> ();
      mainButton.onClick.AddListener (ToggleMenu);

      mainButtonPosition = mainButton.GetComponent <RectTransform> ().anchoredPosition ;
      mainButtonPosition = new Vector3(mainButton.transform.position.x, mainButton.transform.position.y, mainButton.transform.position.z - 10);
        //set all menu items position to mainButtonPosition
        ResetPositions () ;
        
    }

  public void ResetPositions () {
      for (int i = 0; i < itemsCount; i++) {
         menuItems [ i ].rectTrans.anchoredPosition = mainButtonPosition;
         menuItems[i].GetComponent<Button>().enabled = false;
         menuItems[i].GetComponent<Image>().color = noAlpha;
         menuItems[i].transform.GetChild(0).GetComponent<Image>().color = noAlpha;
        }
   }

   public void ToggleMenu () {
      isExpanded = !isExpanded ;

      if (isExpanded) {
            //menu opened
            for (int i = 0; i < itemsCount; i++) {
                menuItems[i].GetComponent<Image>().color = alpha;
                menuItems[i].transform.GetChild(0).GetComponent<Image>().color = alpha;
                menuItems[i].GetComponent<Button>().enabled = true;
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                //Fade to alpha=1 starting from alpha=0 immediately
                menuItems [ i ].img.DOFade (1f, expandFadeDuration).From (0f) ;
                menuItems[i].transform.GetChild(0).GetComponent<Image>().DOFade(1f, expandFadeDuration).From(0f);
            }
      } else {
         //menu closed
         for (int i = 0; i < itemsCount; i++) {
            menuItems [ i ].rectTrans.DOAnchorPos (mainButtonPosition, collapseDuration).SetEase (collapseEase) ;
            //Fade to alpha=0
            menuItems [ i ].img.DOFade (0f, collapseFadeDuration) ;
            menuItems[i].transform.GetChild(0).GetComponent<Image>().DOFade(0f, collapseFadeDuration);
            }
      }

      //rotate main button arround Z axis by 180 degree starting from 0
      /* mainButton.transform
			.DORotate (Vector3.forward * 180f, rotationDuration)
			.From (Vector3.zero)
			.SetEase (rotationEase) ;*/
   }

   public void OnItemClick (int index) {
      //here you can add you logic 
      switch (index) {
         case 0: 
				//first button
            Debug.Log ("Music") ;
            break ;
         case 1: 
				//second button
            Debug.Log ("Sounds") ;
            break ;
         case 2: 
				//third button
            Debug.Log ("Vibration") ;
            break ;
      }
   }

   void OnDestroy () {
      //remove click listener to avoid memory leaks
      mainButton.onClick.RemoveListener (ToggleMenu) ;
   }
}
