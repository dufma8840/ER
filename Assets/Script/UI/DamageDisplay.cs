using UnityEngine;
using TMPro;

public class DamageDisplay : MonoBehaviour
{
    public TMP_Text damageText;
    public float moveSpeed;
    public float minFont;
    public float fontSpeed;
    public float time;
 
    private void Awake()
    {
        time = 0;
        minFont = 0.2f;
        moveSpeed = 1f;
        fontSpeed = 1.2f;
    }
    private void Update()
    {
        time += Time.deltaTime;
        transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        if (time <= 0.2f)
        {
            damageText.fontSize += Time.deltaTime * fontSpeed;
        }
        if (time >= 0.8)
        {
            Reset();
        }
    }

    void Reset()
    {
        time = 0;
        damageText.fontSize = minFont;
        gameObject.SetActive(false);
    }
}
