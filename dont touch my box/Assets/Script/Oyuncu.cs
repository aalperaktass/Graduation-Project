

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oyuncu : MonoBehaviour
{
    public GameObject top;
    public GameObject TopCikisnoktasi;
    public ParticleSystem TopAtisEfekt;
    public AudioSource TopAtmaSesi;
    float AtisYonu;
     

   [Header("GÜÇ BARI AYARLARI")]
    Image PowerBar; 
    float powerSayi;    
    bool sonageldimi=false;
    Coroutine powerDongu;

    PhotonView pw;
    bool AtesAktifmi=false;
    void Start()
    {      

        pw = GetComponent<PhotonView>(); // PhotonViewi çağırıypruz. Companentlerine ihtiyacımız var.

        if (pw.IsMine) // Bu oyuncu ben isem 
        {
            PowerBar = GameObject.FindWithTag("PowerBar").GetComponent<Image>(); // PowerBar tagını bul ve onun içindeki imageyi al 
            if (PhotonNetwork.IsMasterClient)                                    // Bu odanın kurucusu ben isem 
            {
                gameObject.tag = "Oyuncu_1";                                                        // Ben ilk girdiysem benim tagımı Oyuncu_1 yap.
                transform.position = GameObject.FindWithTag("OlusacakNokta_1").transform.position;  // Ve bu tagların pozisyonu ve rotationunu  OlusacakNokta_1 yap 
                transform.rotation = GameObject.FindWithTag("OlusacakNokta_1").transform.rotation;
                AtisYonu = 2f;              // Atış yönümü 2f yap   
            }
            else
            {
                gameObject.tag = "Oyuncu_2"; // Ben ikinci girdiysem benim tagımı Oyuncu_2 yap.
                transform.position = GameObject.FindWithTag("OlusacakNokta_2").transform.position;  // Ben ilk girdiysem benim tagımı Oyuncu_2 yap.
                transform.rotation = GameObject.FindWithTag("OlusacakNokta_2").transform.rotation;  // Ve bu tagların pozisyonu ve rotationunu  OlusacakNokta_2 yap 
                AtisYonu = -2f;             // Atış yönümü -2f yap  dedik çünkü tersinden gitmeli.

            }

        }
        InvokeRepeating("Oyunbasladimi", 0, .5f); //  Yukarıdaki Şartlar sağlandıktan sonra 

    }
    public void Oyunbasladimi() // Bu metodu eklememizin sebebi ilk etaptaki tetikilemeyi yapmak istememiz.
    {
        if (PhotonNetwork.PlayerList.Length == 2) // Oyuncu sayısı 2 ye eşit ise 
        {
            if (pw.IsMine) // Bu ben isem 
            {
                powerDongu = StartCoroutine(PowerBarCalistir()); // Powerbarı çalıştır
                CancelInvoke("Oyunbasladimi"); // Kontrol etmeyi bırak

            }
           

        }else
        {
            StopAllCoroutines();
        }
    }
    IEnumerator PowerBarCalistir()
    {
        PowerBar.fillAmount = 0;
        sonageldimi = false;
        AtesAktifmi = true;

        while (true)
        {
            if (PowerBar.fillAmount < 1 && !sonageldimi)
            {
                powerSayi = 0.01f;
                PowerBar.fillAmount += powerSayi;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

            }else
            {
                sonageldimi = true;
                powerSayi = 0.01f;
                PowerBar.fillAmount -= powerSayi;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                if (PowerBar.fillAmount==0)
                {
                    sonageldimi = false;

                }

            }


        }

    }

    
    
    void Update()
    {
              

        if (pw.IsMine) // Bu kişi ben isem 
        {
            if (Input.touchCount > 0 && AtesAktifmi) // dokunma eklenecek
            {
               
                PhotonNetwork.Instantiate("Patlama_efekt", TopCikisnoktasi.transform.position, TopCikisnoktasi.transform.rotation, 0, null); // Sunucu üzerinde patlama efektini oluşturduk. Pozisyon ve transformla birlikte
                TopAtmaSesi.Play();
                GameObject topobjem = PhotonNetwork.Instantiate("Top", TopCikisnoktasi.transform.position, TopCikisnoktasi.transform.rotation, 0, null);


                topobjem.GetComponent<PhotonView>().RPC("TagAktar",RpcTarget.All, gameObject.tag);

                Rigidbody2D rg = topobjem.GetComponent<Rigidbody2D>();
                rg.AddForce(new Vector2(AtisYonu, 0f) * PowerBar.fillAmount * 12f, ForceMode2D.Impulse);
                AtesAktifmi = false;
                StopCoroutine(powerDongu);
               


            }

        }

       

        
    }


    public void PowerOynasin()
    {
        powerDongu = StartCoroutine(PowerBarCalistir());
    }
   
    public void sonuc(int deger)
    {
       
        if (pw.IsMine)
        {

            if (PhotonNetwork.IsMasterClient)
                {

                if (deger==1)
                {
                    PlayerPrefs.SetInt("Toplam_mac", PlayerPrefs.GetInt("Toplam_mac") + 1);
                    PlayerPrefs.SetInt("Galibiyet", PlayerPrefs.GetInt("Galibiyet") + 1);
                    PlayerPrefs.SetInt("Toplam_puan", PlayerPrefs.GetInt("Toplam_puan") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("Toplam_mac", PlayerPrefs.GetInt("Toplam_mac") + 1);
                    PlayerPrefs.SetInt("Maglubiyet", PlayerPrefs.GetInt("Maglubiyet") + 1);

                }

            }
            else
            {


                if (deger == 2)
                {
                    PlayerPrefs.SetInt("Toplam_mac", PlayerPrefs.GetInt("Toplam_mac") + 1);
                    PlayerPrefs.SetInt("Galibiyet", PlayerPrefs.GetInt("Galibiyet") + 1);
                    PlayerPrefs.SetInt("Toplam_puan", PlayerPrefs.GetInt("Toplam_puan") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("Toplam_mac", PlayerPrefs.GetInt("Toplam_mac") + 1);
                    PlayerPrefs.SetInt("Maglubiyet", PlayerPrefs.GetInt("Maglubiyet") + 1);

                }


            }





        }

        Time.timeScale = 0;


    }
}
