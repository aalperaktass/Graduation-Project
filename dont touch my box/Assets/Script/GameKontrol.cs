using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class GameKontrol : MonoBehaviour
{


    [Header("OYUNCU SAĞLIK AYARLARI")]
    public Image Oyuncu_1_saglik_Bar;
    float Oyuncu_1_saglik = 100;
    public Image Oyuncu_2_saglik_Bar;
    float Oyuncu_2_saglik = 100;
    PhotonView pw;

    bool basladikmi;
    int limit;
    float beklemesuresi;
    int olusturmaSayisi;
    public GameObject[] noktalar;
    GameObject oyuncu1;
    GameObject oyuncu2;

    bool oyunbittimi=false;

    private void Start()
    {
        pw = GetComponent<PhotonView>();
        basladikmi = false;
        limit = 4;
        beklemesuresi = 5f;

    }


    IEnumerator OlusturmayaBasla()
    {
        olusturmaSayisi = 0;

        while (true && basladikmi)
        {
            if (limit == olusturmaSayisi)
                basladikmi = false;

            yield return new WaitForSeconds(15f);
            int olusandeger = Random.Range(0, 7);
            PhotonNetwork.Instantiate("Odul", noktalar[olusandeger].transform.position, noktalar[olusandeger].transform.rotation, 0, null);
            olusturmaSayisi++;
        }

    }
    [PunRPC]
    public void Basla() 
    {
        if (PhotonNetwork.IsMasterClient)
                basladikmi = true;
                StartCoroutine(OlusturmayaBasla());
       
    }

    [PunRPC]
    public void Darbe_vur(int kriter,float darbegucu)
    {

        switch (kriter) 
        {

            case 1:
                
                    Oyuncu_1_saglik -= darbegucu;

                    Oyuncu_1_saglik_Bar.fillAmount = Oyuncu_1_saglik / 100;

                    if (Oyuncu_1_saglik <= 0)
                    {


                    foreach (GameObject objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (objem.gameObject.CompareTag("OyunSonuPanel"))
                        {
                            objem.gameObject.SetActive(true);
                            GameObject.FindWithTag("OyunSonuBilgi").GetComponent<TextMeshProUGUI>().text= "Player 1 Won :)";
                            

                        }


                    }

                    kazanan(2);                 

                     }
               
                break;
            case 2:                

                    Oyuncu_2_saglik -= darbegucu;

                    Oyuncu_2_saglik_Bar.fillAmount = Oyuncu_2_saglik / 100;

                    if (Oyuncu_2_saglik <= 0)
                    {

                    foreach (GameObject objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (objem.gameObject.CompareTag("OyunSonuPanel"))
                        {
                            objem.gameObject.SetActive(true);
                            GameObject.FindWithTag("OyunSonuBilgi").GetComponent<TextMeshProUGUI>().text = "Player 1 Won :)";


                        }


                    }

                    kazanan(1);
                 
                    
                     }
                break;

        }

    }
    public void anamenu()
    {
        GameObject.FindWithTag("SunucuYonetimi").GetComponent<SunucuYonetim>().butonlami = true;
        PhotonNetwork.LoadLevel(0);
       
    }
    public void NormalCikis()
    {
       
        PhotonNetwork.LoadLevel(0);

    }


    void kazanan(int deger)
    {

        if (!oyunbittimi)
        {
            GameObject.FindWithTag("Oyuncu_1").GetComponent<Oyuncu>().sonuc(deger);
            GameObject.FindWithTag("Oyuncu_2").GetComponent<Oyuncu>().sonuc(deger);
            oyunbittimi = true;
        }

        

    }
    [PunRPC]
    public void SaglikDoldur(int hangioyuncu)
    {
        switch (hangioyuncu)
        {

            case 1:
                Oyuncu_1_saglik += 30;

                if (Oyuncu_1_saglik > 100)
                {
                    Oyuncu_1_saglik = 100;
                    Oyuncu_1_saglik_Bar.fillAmount = Oyuncu_1_saglik / 100;

                }
                else
                {
                    Oyuncu_1_saglik_Bar.fillAmount = Oyuncu_1_saglik / 100;
                }

               

                

                break;
            case 2:
                Oyuncu_2_saglik += 30;

                if (Oyuncu_2_saglik > 100)
                {
                    Oyuncu_2_saglik = 100;
                    Oyuncu_2_saglik_Bar.fillAmount = Oyuncu_2_saglik / 100;

                }
                else
                {
                    Oyuncu_2_saglik_Bar.fillAmount = Oyuncu_2_saglik / 100;
                }
                break;

        }

    }
}
