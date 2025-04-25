using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Fusion;

namespace Vuplex.WebView.Demos
{
    public class BoardController : MonoBehaviour
    {

        [SerializeField] CanvasWebViewPrefab _WebViewPrefab;

        [SerializeField] private GameObject initialBoard;
        [SerializeField] private GameObject browser;
        [SerializeField] private GameObject keyboard;
        [SerializeField] private GameObject confirmeSuaURL;
        [SerializeField] private string[] historico;
        [SerializeField] private GameObject classRoom;
        int ind;
        public int whatScreenIsOn = 0;
        public int whatScreenIsOnStorage = 10;


        public string url;
        [SerializeField] HardwareRig hardwareRig;

        [SerializeField] private TMP_InputField input;

        void Awake()
        {
            Web.SetCameraAndMicrophoneEnabled(true);
            gameObject.SetActive(false);
            //transform.SetParent(classRoom.transform);
            //transform.SetAsFirstSibling();
        }
        // Start is called before the first frame update
        void Start()
        {
            _WebViewPrefab = browser.GetComponent<CanvasWebViewPrefab>();
            // Send keys from the Keyboard prefab to the textInput.
            keyboard.GetComponent<CanvasKeyboard>().KeyPressed += (sender, eventArgs) =>
            {
                if (eventArgs.Value != "ArrowUp" && eventArgs.Value != "ArrowDown" && eventArgs.Value != "ArrowLeft" && eventArgs.Value != "ArrowRight")
                {
                    if (eventArgs.Value != "Backspace")
                    {
                        Array.Resize(ref historico, historico.Length + 1);
                        historico[ind] = eventArgs.Value;
                        input.text += historico[ind];
                        ind++;
                    }
                    else if (eventArgs.Value == "Backspace")
                    {
                        historico[historico.Length - 1] = null;
                        Array.Resize(ref historico, historico.Length - 1);
                        ind--;
                        input.text = null;
                        for (int i = 0; i < historico.Length; i++)
                        {
                            input.text += historico[i];
                        }
                    }
                }
            };
        }
        private void Update()
        {
            switch (whatScreenIsOn)
            {
                case 1: // Modo Livre
                    TextInputDetection();
                    break;
                case 2: // Confirme sua URL
                    //Backspace
                    if (Input.inputString == "\b")
                    {
                        input.text = null;
                        Array.Resize(ref historico, 0);
                        ind = 0;
                    }
                    break;
                case 3: // Explorador de Arquivos

                    break;
                case 4: // Browser
                    TextInputDetection();
                    break;
                case 0:// closed

                    break;
            }

        }

        async public void ModoLivreButton()
        {
            whatScreenIsOn = 1;
            GetComponent<BoardSyncronizer>().screen = whatScreenIsOn;
            _WebViewPrefab.DragMode = DragMode.DragWithinPage;
            initialBoard.SetActive(false);
            browser.SetActive(true);
            keyboard.SetActive(false);
            await _WebViewPrefab.WaitUntilInitialized();
            _WebViewPrefab.WebView.LoadUrl("https://www.notebookcast.com/en/board/p06ubdg1f413e");
        }
        async public void exploradorDeArquivosButton()
        {
            whatScreenIsOn = 3;
            GetComponent<BoardSyncronizer>().screen = whatScreenIsOn;
            _WebViewPrefab.DragMode = DragMode.DragToScroll;
            initialBoard.SetActive(false);
            browser.SetActive(true);
            keyboard.SetActive(false);
            await _WebViewPrefab.WaitUntilInitialized();
            _WebViewPrefab.WebView.LoadUrl("https://remoto.woodyverso.com.br/video/");
        }
        public void NavegadorButton()
        {
            whatScreenIsOn = 2;
            GetComponent<BoardSyncronizer>().screen = whatScreenIsOn;
            _WebViewPrefab.DragMode = DragMode.DragToScroll;
            initialBoard.SetActive(false);
            confirmeSuaURL.SetActive(true);
            keyboard.SetActive(true);
        }

        public void BackInsiraSuaUrlButton()
        {
            whatScreenIsOn = 0;
            GetComponent<BoardSyncronizer>().screen = whatScreenIsOn;
            initialBoard.SetActive(true);
            browser.SetActive(false);
            confirmeSuaURL.SetActive(false);
            keyboard.SetActive(false);
        }
        public void BackBrowser()
        {
            whatScreenIsOn = 0;
            GetComponent<BoardSyncronizer>().screen = whatScreenIsOn;
            initialBoard.SetActive(true);
            browser.SetActive(false);
            confirmeSuaURL.SetActive(false);
            keyboard.SetActive(false);
        }
        async public void ConfirmarInsiraSuaUrlButton()
        {
            whatScreenIsOn = 4;
            GetComponent<BoardSyncronizer>().screen = whatScreenIsOn;
            browser.SetActive(true);
            if(GetComponent<NetworkObject>().HasStateAuthority)
                url = "https://" + input.text;
            GetComponent<BoardSyncronizer>().url = url;
            confirmeSuaURL.SetActive(false);
            keyboard.SetActive(false);
            await _WebViewPrefab.WaitUntilInitialized();
            url = GetComponent<BoardSyncronizer>().url.ToString();
            _WebViewPrefab.WebView.LoadUrl(url);
            Debug.Log(url);             
        }
        async void TextInputDetection()
        {
            await _WebViewPrefab.WaitUntilInitialized();
            _WebViewPrefab.WebView.FocusedInputFieldChanged += (sender, eventArgs) =>
            {
                if (eventArgs.Type == FocusedInputFieldType.Text)
                {
                    keyboard.SetActive(true);
                }
                else
                    keyboard.SetActive(false);
            };
        }
    }
}
