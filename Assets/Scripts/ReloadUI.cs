using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoBehaviour
{
    [SerializeField] private Image m_ReloadImage_1;
    [SerializeField] private Image m_ReloadImage_2;
    [SerializeField] private GameObject m_UIContainer;

    private Coroutine m_ReloadCoroutine;
    private bool m_isReloading = false;

    public void ReloadUI_Process(float reloadTime)
    {
        m_UIContainer.SetActive(true);
        m_ReloadCoroutine = StartCoroutine(ReloadProcess(reloadTime));
    }
    private IEnumerator ReloadProcess(float reloadTime)
    {
        Cursor.visible = false;
        float timer = 0f;
        m_isReloading = true;
        m_ReloadImage_1.fillAmount = 0f;
        m_ReloadImage_2.fillAmount = 0f;

        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            m_ReloadImage_1.fillAmount = Mathf.Clamp01(timer / reloadTime); // Update fillAmount based on time elapsed
            m_ReloadImage_2.fillAmount = Mathf.Clamp01(timer / reloadTime*2); // Update fillAmount based on time elapsed
            yield return null;
        }

        m_isReloading = false;
        Cursor.visible = true;
       // yield return new WaitForSeconds(1f);

        TryToHideUi();
    }

    private void TryToHideUi()
    {
        if(!m_isReloading)
        {
            m_UIContainer.SetActive(false);
        }
        
    }

    private void LateUpdate()
    {
        m_UIContainer.transform.position = Input.mousePosition;
    }
}
