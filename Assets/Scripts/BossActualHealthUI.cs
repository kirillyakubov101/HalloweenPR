using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossActualHealthUI : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image m_ActualHpBar;


    private Queue<IEnumerator> m_ActualHealthListOfTasks = new Queue<IEnumerator>();

    private bool m_isProcessing = false;
    private Coroutine m_actualHealthTaskThread;

    private void OnEnable()
    {
        BossHealth.OnHit += UpdateHealthView;
    }

    private void OnDisable()
    {
        BossHealth.OnHit -= UpdateHealthView;
    }

    private void UpdateHealthView(float dmg, float maxHealth)
    {
        AddNewTask(UpdateActualHealthUI(dmg, maxHealth));
    }

    private IEnumerator UpdateActualHealthUI(float dmg,float maxHealth)
    {
        float percentToReduce = dmg / maxHealth;
        float newFillAmountGoal = Mathf.Max( 0f,m_ActualHpBar.fillAmount - percentToReduce);
        
        // Make sure it exactly matches the target amount at the end
        m_ActualHpBar.fillAmount = newFillAmountGoal;
        yield return null;
    }

    private void AddNewTask(IEnumerator Task)
    {
        if (Task == null) { return; }

        m_ActualHealthListOfTasks.Enqueue(Task);

        //if the task execution did not start, start it. If it is already running it will find the newly added task
        if (!m_isProcessing)
        {
            m_actualHealthTaskThread = StartCoroutine(ProcessingTasks());
        }

     
       
      
    }

    private IEnumerator ProcessingTasks()
    {
        while (m_ActualHealthListOfTasks.Count > 0)
        {
            if (m_ActualHealthListOfTasks.TryDequeue(out IEnumerator task))
            {
                m_isProcessing = true; //task was found in the queue, begin process
                yield return task;

            }
        }

        m_isProcessing = false;

    }
}
