using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SunucuYonetim : MonoBehaviourPunCallbacks
{

    GameObject serverbilgi;
    GameObject Ad_kaydet;
    GameObject Random_giris;
    GameObject Oda_kur_ve_gir;
    public bool butonlami;
    void Start()
    {
        Debug.Log("CALISIYOR");
        serverbilgi = GameObject.FindWithTag("Server_bilgi");
        Ad_kaydet = GameObject.FindWithTag("Ad_kaydet_buton");
        Random_giris = GameObject.FindWithTag("Random_giris_yap");
        Oda_kur_ve_gir = GameObject.FindWithTag("Oda_kur_ve_gir");


        PhotonNetwork.ConnectUsingSettings();

        DontDestroyOnLoad(gameObject); // Bu satırdaki kod oyundaki sahneler değişiyor olsa bile sunucu yönetimi scriptini ve beraberindeki yönetimi taşıyor.  

    }


    public override void OnConnectedToMaster() // Server'e bağlanmaya yarayan gerekli kod .
    {
        serverbilgi.GetComponent<Text>().text = "Connected to Server";

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        serverbilgi.GetComponent<Text>().text = "Connected to Lobby";


        if (!PlayerPrefs.HasKey("Kullanıcıadi"))
        {
            Ad_kaydet.GetComponent<Button>().interactable = true;
        }
        else
        {
            Random_giris.GetComponent<Button>().interactable = true;
            Oda_kur_ve_gir.GetComponent<Button>().interactable = true;
        }


    }

    public void RandomGirisYap()// Bu bölüm bizim random odaya girmemize yarıyor. Copy Paste işlem. 
    {

        PhotonNetwork.LoadLevel(1); // Bu kısım butonlara basıldığında sahnenin yüklenmesine yarıyor. 
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("No opened room was found.");

    }
    public void OdaOlusturvegir() // Bu bölüm Photon bize oda oluşturuyor. Copy Paste işlem. Sunucuda oda kurmaya yarayan gerekli kodlar.
    {
        PhotonNetwork.LoadLevel(1); // Bu kısım butonlara basıldığında sahnenin yüklenmesine yarıyor. 
        string odaadi = Random.Range(0, 9964124).ToString();
        PhotonNetwork.JoinOrCreateRoom(odaadi, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom() // Burada  Objeyi oluşturdu. Klasörümüzdeki Oyuncu ismindeki objeyi oluşturduk ve pozisyonunu atadık. Sonrasında kullanıcını nick name değerini. PlayPrefs üzerinden yani sistem üzerinden kullanıcı adını alıyoruz.
    {

        InvokeRepeating("BilgileriKontrolEt", 0, 1f); // Burası ise bilgileri 1 saniyede bir kontrol ediyor. Çünkü her an oyuncu girebilir.
        GameObject objem = PhotonNetwork.Instantiate("Oyuncu", Vector3.zero, Quaternion.identity, 0, null); // Sonrasında sunucu üzerinde bir obje oluşturacaksın ve adı "Oyuncu" olacak ve pozisyonu yanda olanlar olacak.
        objem.GetComponent<PhotonView>().Owner.NickName = PlayerPrefs.GetString("Kullanıcıadi");

        if (PhotonNetwork.PlayerList.Length == 2) // Asıl olay burada. Eğer oyuncu sayısı 2 ye eşit ise gelen oyuncunun tagını  "Oyuncu_2" yapacaksın diyoruz. Bu durum tag kontrollerini sunucu yönetimine veriyor.
        {
            objem.gameObject.tag = "Oyuncu_2";
            GameObject.FindWithTag("GameKontrol").gameObject.GetComponent<PhotonView>().RPC("Basla", RpcTarget.All);
        }

    }

    public override void OnLeftRoom()

    {
        if (butonlami)
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();
            //  Debug.Log("Sen Çıktın");
            PlayerPrefs.SetInt("Toplam_mac", PlayerPrefs.GetInt("Toplam_mac") + 1);
            PlayerPrefs.SetInt("Maglubiyet", PlayerPrefs.GetInt("Maglubiyet") + 1);
        }
    }

    public override void OnLeftLobby()

    {
        // lobiden

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // bir oyuncu girdiyse

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)

    {
        if (butonlami)
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();
            PlayerPrefs.SetInt("Toplam_mac", PlayerPrefs.GetInt("Toplam_mac") + 1);
            PlayerPrefs.SetInt("Galibiyet", PlayerPrefs.GetInt("Galibiyet") + 1);
            PlayerPrefs.SetInt("Toplam_puan", PlayerPrefs.GetInt("Toplam_puan") + 150);
        }



        // Debug.Log("Rakip Çıktı");
        // bir oyuncu çıktıysa

        InvokeRepeating("BilgileriKontrolEt", 0, 1f);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        serverbilgi.GetComponent<Text>().text = "Could not enter the room";

    }
    public override void OnJoinRandomFailed(short returnCode, string message)

    {
        serverbilgi.GetComponent<Text>().text = "Could not enter a random room.";




    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        serverbilgi.GetComponent<Text>().text = "Failed to Create Room";

    }


    void BilgileriKontrolEt()
    {

        if (PhotonNetwork.PlayerList.Length == 2) // Toplam oyuncu sayısı 2 ye eşitse 
        {
            GameObject.FindWithTag("OyuncuBekleniyor").SetActive(false); // paneli kapatıyoruz
            GameObject.FindWithTag("Oyuncu_1_isim").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName; // Oyuncu_1_isim tagındaki text'te yazanı yazdır.
            GameObject.FindWithTag("Oyuncu_2_isim").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName; // Oyuncu_2_isim tagındaki text'te yazanı yazdır.
            CancelInvoke("BilgileriKontrolEt");     // Yuarıdaki şartlar sağlanınca oda giriş metodundaki bilgileri tekrar tekrar kontrol işlemi iptal ediliyor.      
        }
        else
        {

            GameObject.FindWithTag("OyuncuBekleniyor").SetActive(true); //toplam oyuncu sayısı 2 ye eşit değilse 1 kişi varsa paneli tekrar aç. (Çıkıp giren oyuncuara karşı)
            GameObject.FindWithTag("Oyuncu_1_isim").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName; // Gelen kişiyi 0. oyuncu olarak al 
            GameObject.FindWithTag("Oyuncu_2_isim").GetComponent<TextMeshProUGUI>().text = ".......";                           //  ikinci oyuncu için ilgili tagın texti için ise "......." yap.
        }



    }


}
