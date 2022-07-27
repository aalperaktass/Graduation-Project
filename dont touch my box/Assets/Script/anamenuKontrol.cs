using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class anamenuKontrol : MonoBehaviour
{
    public GameObject ilkpanel;
    public GameObject ikincipanel;
    public InputField kullaniciadi;
    public Text Varolankullaniciadi;
    public TextMeshProUGUI[] istatistik;
    public Text serverbilgi;
    GameObject Random_giris;
    GameObject Oda_kur_ve_gir;

    void Start()
    {
         

        if (!PlayerPrefs.HasKey("Kullanıcıadi")) // Böyle bir kullanıcı adı var mı yok mu diye kontrol ediyoruz. Yoksa ilk panel çıkıyor ve inputfield'te  yazılan ad oluşturuluyor ve aşağıdaki değerler sıfıra eşitleniyor.
        {
            PlayerPrefs.SetInt("Toplam_mac", 0);
            PlayerPrefs.SetInt("Maglubiyet", 0);
            PlayerPrefs.SetInt("Galibiyet", 0);
            PlayerPrefs.SetInt("Toplam_puan", 0);

            ilkpanel.SetActive(true);
            DegerleriYaz();

        }
        else // varsa sistemde kayıtlı olan Kullanıcı bilgilerini ve adını yazdıracak 
        {
            ikincipanel.SetActive(true);
            Varolankullaniciadi.text = PlayerPrefs.GetString("Kullanıcıadi"); 
            DegerleriYaz();
        }
        
    }

    public void KullaniciAdiKaydet()
    {
       
        PlayerPrefs.SetString("Kullanıcıadi", kullaniciadi.text);

        ilkpanel.SetActive(false);
        ikincipanel.SetActive(true);
        Varolankullaniciadi.text = kullaniciadi.text;
        Random_giris = GameObject.FindWithTag("Random_giris_yap");
        Oda_kur_ve_gir = GameObject.FindWithTag("Oda_kur_ve_gir");
        Random_giris.GetComponent<Button>().interactable = true;
        Oda_kur_ve_gir.GetComponent<Button>().interactable = true;

    }

    void DegerleriYaz()
    {
        istatistik[0].text = PlayerPrefs.GetInt("Toplam_mac").ToString();
        istatistik[1].text = PlayerPrefs.GetInt("Maglubiyet").ToString();
        istatistik[2].text = PlayerPrefs.GetInt("Galibiyet").ToString();
        istatistik[3].text = PlayerPrefs.GetInt("Toplam_puan").ToString();
    }
}
