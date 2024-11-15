using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��Ϸ�ܹ���������������еĹ�����
public class GameManager : MonoBehaviour
{
    public Texture2D texture2D;
    public GameController gameController;
    public AudioSourceManager audioSourceManager;
    public FactoryManager factoryManager;
    public PlayerManager playerManager;
    public StringManager stringManager;
    public UIManager uiManager;
    public SceneStateManager sceneStateManager;
    private static GameManager _instance;
    public Stage currentStage;

    public bool isInitPlayerManager = false;//�Ƿ�������Ϸ
    public static GameManager Instance { get => _instance; }

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
        _instance = this;
        playerManager = new PlayerManager();
        //�����ʼ��Ϣ`
        //playerManager.SaveData();
        playerManager.ReadData();
        factoryManager = new FactoryManager();
        audioSourceManager = new AudioSourceManager();
        uiManager = new UIManager();

        sceneStateManager = new SceneStateManager();
        uiManager.mUIFacade.mSceneStateManager = sceneStateManager;

        uiManager.mUIFacade.currentSceneState = sceneStateManager.startLoadSenceState;
        uiManager.mUIFacade.currentSceneState.EnterSence();

    }

    public GameObject CreateItem(GameObject itemGo)
    {
        GameObject go = Instantiate(itemGo);
        return go;
    }

    //��ȡͼƬ
    public Sprite GetSprite(string resourcePath)
    {
        return factoryManager.spriteFactory.GetSingleResources(resourcePath);
    }

    //��ȡͼƬ
    public AudioClip GetAudioClip(string resourcePath)
    {
        return factoryManager.audioClipFactory.GetSingleResources(resourcePath);
    }
    //��ȡ����������
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return factoryManager.runtimeAnimatorControllerFactory.GetSingleResources(resourcePath);
    }
    //��ȡ��Ϸ����
    public GameObject GetGameObject(FactoryType factoryType, string name)
    {
        GameObject w = factoryManager.factoryDist[factoryType].GetItem(name);
        return w;
    }

    public void PushGameObjectToFactory(FactoryType factoryType, string name, GameObject gameObject)
    {
        factoryManager.factoryDist[factoryType].PushItem(name, gameObject);
    }
}
