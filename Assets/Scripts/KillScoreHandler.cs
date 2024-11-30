using UnityEngine;

public class KillScoreHandler : MonoBehaviour
{
    private ScoreKillTextPool m_textPool;
    private readonly int AppearHashAnim = Animator.StringToHash("Appear");

    private void Awake()
    {
        m_textPool = FindObjectOfType<ScoreKillTextPool>();
    }

    public void CreateScoreTextAfterKill(Vector3 newPos)
    {
        var newInst = m_textPool.TryGetPoolObject();
        newPos.y -= 0.5f;
        newInst.transform.position = newPos;
        RectTransform rectTransform = newInst.GetComponent<RectTransform>();


        newInst.GetComponentInChildren<Animator>().SetTrigger(AppearHashAnim);
    }
}
