using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossForegroundHealthUI : MonoBehaviour
{
    [SerializeField] private float m_foregroundDelay = 1f;
    [SerializeField] private Image m_ForegroundHpBar;
    [SerializeField] private float m_ForegroundHpBarFadeTime = 1f;


    private Queue<IEnumerator> m_ForegroundHealthListOfTasks = new Queue<IEnumerator>();

    private bool m_isProcessing = false;
    private Coroutine m_ForegroundlHealthTaskThread;


    private void OnEnable()
    {
        BossHealth.OnHit += UpdateHealthView;
    }

    private void OnDisable()
    {
        BossHealth.OnHit -= UpdateHealthView;
    }

    float timer = 0f;
    [SerializeField] float maxCd = 5f;
    bool firstTime = true;
    private void Update()
    {
        if(!firstTime)
        {
            timer += Time.deltaTime;
            if(timer > maxCd)
            {
                timer = 0f;
                firstTime= true;
            }
        }
    }

    private void UpdateHealthView(float dmg, float maxHealth)
    {
        AddNewTask(UpdateForegroundHealthUI(dmg, maxHealth));
    }


    private void AddNewTask(IEnumerator Task)
    {
        if (Task == null) { return; }

        m_ForegroundHealthListOfTasks.Enqueue(Task);
        timer = 0f;

        //if the task execution did not start, start it. If it is already running it will find the newly added task
        if (!m_isProcessing)
        {
            m_ForegroundlHealthTaskThread = StartCoroutine(ProcessingTasks());
        }
    }

    private IEnumerator ProcessingTasks()
    {
        while (m_ForegroundHealthListOfTasks.Count > 0)
        {
            if (m_ForegroundHealthListOfTasks.TryDequeue(out IEnumerator task))
            {
                m_isProcessing = true; //task was found in the queue, begin process
                yield return task;

            }
        }

        m_isProcessing = false;

    }

    private IEnumerator UpdateForegroundHealthUI(float dmg, float maxHealth)
    {
        if(firstTime)
        {
            firstTime = false;
            yield return new WaitForSeconds(m_foregroundDelay);
        }
        
        float timer = 0f;
        float percentToReduce;
        float newFillAmountGoal;
        float fillAmountReductionPerSecond;



        percentToReduce = dmg / maxHealth;
        newFillAmountGoal = Mathf.Max(0f, m_ForegroundHpBar.fillAmount - percentToReduce);
        fillAmountReductionPerSecond = percentToReduce / m_ForegroundHpBarFadeTime;



        //actual hp bar fade timer
        while (timer < m_ForegroundHpBarFadeTime)
        {
            m_ForegroundHpBar.fillAmount -= fillAmountReductionPerSecond * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // Make sure it exactly matches the target amount at the end
        m_ForegroundHpBar.fillAmount = newFillAmountGoal;
    }
}
