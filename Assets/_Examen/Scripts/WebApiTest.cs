// ============================================================
//  P5 - SERVICIO WEB (version SIN Firebase, lista para usar)
//  Consume una API REST publica (Yu-Gi-Oh! YGOPRODeck) por HTTP GET,
//  deserializa el JSON con JsonUtility y muestra los datos.
//  Todo es nativo de Unity (UnityWebRequest) -> NO necesita ningun SDK.
//  Pega otro endpoint en 'url' si el examen pide otra API.
// ============================================================
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class WebApiTest : MonoBehaviour
{
    [Header("API REST de prueba")]
    public string cardName = "Dark Magician";
    [Tooltip("Texto opcional para mostrar el resultado en pantalla")]
    public TMP_Text resultText;

    [System.Serializable]
    public class CardDataResponse { public CardData[] data; }

    [System.Serializable]
    public class CardData
    {
        public string name;
        public string type;
        public string desc;
        public int atk;
        public int def;
        public int level;
    }

    void Start()
    {
        Buscar(); // prueba automatica al iniciar
    }

    // Conecta este metodo al OnClick de un boton si quieres
    public void Buscar()
    {
        StartCoroutine(GetCardData(cardName));
    }

    IEnumerator GetCardData(string name)
    {
        string url = $"https://db.ygoprodeck.com/api/v7/cardinfo.php?name={UnityWebRequest.EscapeURL(name)}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var resp = JsonUtility.FromJson<CardDataResponse>(request.downloadHandler.text);
                if (resp != null && resp.data != null && resp.data.Length > 0)
                {
                    var c = resp.data[0];
                    string info = $"{c.name}\nTipo: {c.type}\nATK: {c.atk}  DEF: {c.def}  Nivel: {c.level}";
                    Debug.Log(info);
                    if (resultText != null) resultText.text = info;
                }
                else Debug.LogError("Carta no encontrada o respuesta invalida.");
            }
            else Debug.LogError("Error obteniendo datos: " + request.error);
        }
    }
}
