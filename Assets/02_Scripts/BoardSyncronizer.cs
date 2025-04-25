using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView.Demos;
using Fusion;

public class BoardSyncronizer : NetworkBehaviour
{

    [SerializeField] BoardController board;

    [Networked(OnChanged = nameof(UrlSyncer))]
    public NetworkString<_128> url { get; set; }
    public static string urlStatic;


    [Networked(OnChanged = nameof(BoardSyncer))]
    public int screen { get; set; }
    public int screenStorage = 10;


    public static int screenStatic;
    public static int screenStorageStatic;

    private void Awake()
    {
        board = GetComponent<BoardController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        
        screenStatic = screen;
        screenStorageStatic = screenStorage;
        board.whatScreenIsOn = screen;
        board.whatScreenIsOnStorage = screenStorage;
        urlStatic = url.ToString();
        board.url = url.ToString();
        BoardSync();
    }

    public void BoardSync()
    {
        if (screen != screenStorage)
        {
            switch (screen)
            {
                case 1: // Modo Livre
                    board.ModoLivreButton();
                    break;
                case 2: // Confirme sua URL
                    board.NavegadorButton();
                    break;
                case 3: // Explorador de Arquivos
                    board.exploradorDeArquivosButton();
                    break;
                case 4: // Browser
                    board.ConfirmarInsiraSuaUrlButton();
                    break;
                case 0:// closed
                    board.BackBrowser();
                    break;
            }
            screenStorage = screen;
        }

    }

    static void BoardSyncer(Changed<BoardSyncronizer> changed)
    {
        changed.Behaviour.screen = screenStatic;
        changed.Behaviour.screenStorage = screenStorageStatic;
    }
    static void UrlSyncer(Changed<BoardSyncronizer> changed)
    {
        changed.Behaviour.url = urlStatic;
    }
}
