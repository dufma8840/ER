
using UnityEngine;


public class Teleport : MonoBehaviour
{
    public AudioSource tpSound;
    public PlayerInput player;


    private void Start()
    {
        tpSound = GetComponent<AudioSource>();
        
    }

    public void PlaySound()
    {
        tpSound.Play();
    }
    public void OnButtonClick()
    {
        PlaySound();
        player.stat.coin -= 100;
        Vector3 potal = new Vector3(26.9f, 11.3f, -54.7f);
        PlayerInput.GetInstance().agent.Warp(potal);
        player.mousePos = player.transform.position;
        player.teleportmap.SetActive(false);
    }
    public void OnButtonClick2()
    {
        PlaySound();
        player.stat.coin -= 100;
        Vector3 potal = new Vector3(-4.6f,11.31f, -16.15f);
        PlayerInput.GetInstance().agent.Warp(potal);
        player.mousePos = player.transform.position;
        player.teleportmap.SetActive(false);
    }
    public void OnButtonClick3()
    {
        PlaySound();
        player.stat.coin -= 100;
        Vector3 potal = new Vector3(-18.5f, 11.3f, -79.1f);
        PlayerInput.GetInstance().agent.Warp(potal);
        player.mousePos = player.transform.position;
        player.teleportmap.SetActive(false);
    }
    public void OnButtonClick4()
    {
        PlaySound();
        player.stat.coin -= 100;
        Vector3 potal = new Vector3(63f, 9.7f, 13f);
        PlayerInput.GetInstance().agent.Warp(potal);
        player.mousePos = player.transform.position;
        player.teleportmap.SetActive(false);
    }
}
