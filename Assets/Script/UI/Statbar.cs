using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statbar : MonoBehaviour
{

    [SerializeField] RectTransform m_rectBar;
    [SerializeField] RectTransform m_rectBackGround;

    public void SetBar(float cur, float max)
    {
        Vector2 vRectSize = m_rectBar.sizeDelta;
        vRectSize.x = m_rectBackGround.sizeDelta.x * (cur / max);
        m_rectBar.sizeDelta = vRectSize;
    }

}
