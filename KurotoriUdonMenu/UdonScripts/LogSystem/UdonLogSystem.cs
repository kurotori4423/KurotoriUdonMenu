
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UdonLogSystem : UdonSharpBehaviour
{

    [SerializeField]
    int MAX_LOG_SIZE = 100;

    [SerializeField]
    GameObject logPrefab = null;


    int nextAddIndex = 0;

    GameObject[] logs;

    void Start()
    {
        logs = new GameObject[MAX_LOG_SIZE];

        for(int i = 0; i < MAX_LOG_SIZE; ++i)
        {
            logs[i] = null;
        }
    }

    public void AddLog(string logText)
    {
        var log = VRCInstantiate(logPrefab);
        
        log.transform.localRotation = logPrefab.transform.localRotation;
        log.transform.localScale = logPrefab.transform.localScale;
        log.transform.SetParent(logPrefab.transform.parent, false);

        var textComponent = log.GetComponent<Text>();
        textComponent.text = " " + logText;

        // テキストに合うサイズにRectTransformをリサイズする。
        textComponent.rectTransform.sizeDelta = new Vector2(textComponent.preferredWidth, textComponent.preferredHeight);

        log.SetActive(true);

        UpdateLogBuffer(log);
    }

    void UpdateLogBuffer(GameObject log)
    {
        if(logs[nextAddIndex] != null)
        {
            Destroy(logs[nextAddIndex].gameObject);
        }

        logs[nextAddIndex] = log;

        nextAddIndex = (nextAddIndex + 1) % MAX_LOG_SIZE;
    }

    public override void Interact()
    {
        string testlog ="TestLog:\n " + Time.time.ToString();
        AddLog(testlog);
    }

}
