using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    public TMP_Text statText;
    public Stat stat;
    void Start()
    {
        statText = GetComponent<TMP_Text>(); 
    }

    void Update()
    {
        statText.text = "LV  :  " + stat.lv + "\n"+ "ATK : " +stat.atk + "\n"+ "HP : " + stat.hp + "\n"+ "MP : " + stat.mp + "\n";
    }
}
