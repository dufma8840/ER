
using UnityEngine;

public class Setting : MonoBehaviour
{
    public GameObject setting;
   
    public void Click()
    {
        GameManager.GetInstance().EventTitle();
    }

    public void Closesetting()
    {
        GameManager.GetInstance().EventContinue();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
