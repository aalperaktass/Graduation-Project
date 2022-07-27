using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ortadaki_kutu : MonoBehaviour
{
    float saglik = 100;
    public GameObject SaglikCanvası;
    public Image healtBar;    

    GameObject gameKontrol;
    PhotonView pw;
    AudioSource KutuYokOlmaSesi;
     
    private void Start()
    {
        gameKontrol = GameObject.FindWithTag("GameKontrol");
        pw = GetComponent<PhotonView>();
        KutuYokOlmaSesi = GetComponent<AudioSource>();
    }
    [PunRPC]
    public void darbeal(float dargegucu)
    {

        if (pw.IsMine)
        {

            saglik -= dargegucu;

            healtBar.fillAmount = saglik / 100; // 0.9

            if (saglik <= 0)
            {

               // gameKontrol.GetComponent<GameKontrol>().Ses_ve_Efekt_Olustur(2, gameObject);

                PhotonNetwork.Instantiate("Kutu_kirilma_efekt", transform.position, transform.rotation, 0, null);
                KutuYokOlmaSesi.Play();
                PhotonNetwork.Destroy(gameObject);

            }
            else
            {
                StartCoroutine(CanvasCikar());

            }

        }
           

        
    }


    IEnumerator CanvasCikar()
    {
        if (!SaglikCanvası.activeInHierarchy)
        {
            SaglikCanvası.SetActive(true);
            yield return new WaitForSeconds(2);
            SaglikCanvası.SetActive(false);
        }

    }
 
}
