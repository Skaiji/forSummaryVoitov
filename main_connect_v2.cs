using System;
using System.Collections;
using System.Collections.Generic;
using BarcodeScanner.Webcam;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
///  main_connect_v2
///  основной класс для взаимодействия с
///  API
/// </summary>
public class main_connect_v2 : MonoBehaviour
{
    
    private string AuthKey; // Ключ авторизации
    public string NiceJson; // Строка хранения, изменения JSON
    public GameObject allert_obj; // Ссылка на объект уведомлений приложения
    public GameObject telegram; // Ссылка на объект отправки сообщений в телеграмм бота
    public GameObject preloader; // Ссылка на объект прелоудера приложения
    private string Login; // Логин пользователя
    
    /// <summary>
    /// IEnumerator request является
    /// входной точкой работы веб запросами приложения
    /// </summary>
    /// <param name="Method">Выбор метода запроса</param>
    /// <param name="api">Путь метода API</param>
    /// <param name="callback">Название фукнции которое примет значение ответа</param>
    /// <param name="json">Передача JSON для некоторых запросов API</param>
    /// <param name="parm">Параметры для некоторых запросов API</param>
    /// <param name="form">Форма для некоторых запросов API</param>
    public IEnumerator request(string Method, string api, [CanBeNull] System.Action<string> callback, string json = null, string parm = null, WWWForm form = null)
    { 
        // Установка токена, установка URL
        AuthKey = PlayerPrefs.GetString("beawer_token");
      string URL = PlayerPrefs.GetString("url") + api;
      preloader.GetComponent<Preload_script>().preload_object.SetActive(true);
    
      // Выполнение запроса GET
        if (Method == "GET")
        {
            UnityWebRequest req = UnityWebRequest.Get(URL + parm);
            req.SetRequestHeader("Authorization", "Bearer " + AuthKey);
            req.certificateHandler = new BypassCertificate();

            yield return req.SendWebRequest();
          
            // Обработка ошибки
            if (req.isNetworkError || req.responseCode != 200)
            {
                if (req.error == "Cannot resolve destination host")
                {
                     StartCoroutine(Timeout(Method, api, callback, json, parm, form));
                    yield break;
                }
                Debug.Log("Error While Sending: " + req.error);
                error(req.downloadHandler.text, req.error);
                callback(req.downloadHandler.text);
            }
            // Успешное выполнение запроса GET
            else
            {
                Debug.Log("Received: " + req.downloadHandler.text);
                if (req.downloadHandler.text.StartsWith("["))
                {
                    NiceJson = "{\"result\":" + req.downloadHandler.text + "}";
                }
                else
                {
                    NiceJson = "{\"result\":[" + req.downloadHandler.text + "]}";
                }
                
                 preloader.GetComponent<Preload_script>().preload_object.SetActive(false);
                 preloader.GetComponent<Preload_script>().preload_text.text = "";
                callback(NiceJson);
            }
        }
        // Выполнение запроса POST
        if (Method == "POST")
        {
            UnityWebRequest req = UnityWebRequest.Post(URL + parm, "POST");
            req.SetRequestHeader("Authorization", "Bearer " + AuthKey);
            req.certificateHandler = new BypassCertificate();
            if (json != null)
            {
                req.SetRequestHeader("Content-Type", "application/json");
                var formData = System.Text.Encoding.UTF8.GetBytes(json);
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }
            if (form != null)
            {
                req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                byte[] rawFormData = form.data;
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawFormData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }
            
            yield return req.SendWebRequest();
            
            // Обработка ошибки
            if (req.isNetworkError || req.responseCode != 200)
            {
               
                if (req.error == "Cannot resolve destination host")
                {
                     StartCoroutine(Timeout(Method, api, callback, json, parm, form));
                    yield break;
                }
                Debug.Log("Error While Sending: " + req.error);
                error(req.downloadHandler.text, req.error);
                callback(req.downloadHandler.text);
            }
            // Успешное выполнение запроса POST
            else
            {
                Debug.Log("Received: " + req.downloadHandler.text);
                if (req.downloadHandler.text.StartsWith("["))
                {
                    NiceJson = "{\"result\":" + req.downloadHandler.text + "}";
                }
                else
                {
                    NiceJson = "{\"result\":[" + req.downloadHandler.text + "]}";
                }
                preloader.GetComponent<Preload_script>().preload_object.SetActive(false);
                 preloader.GetComponent<Preload_script>().preload_text.text = "";
                callback(NiceJson);
            }
   
        }
        // Выполнение запроса PUT
        if (Method == "PUT")
        {
            UnityWebRequest req = UnityWebRequest.Put(URL + parm, "test");
            req.SetRequestHeader("Authorization", "Bearer " + AuthKey);
            req.SetRequestHeader("Content-Type", "application/json");
            req.certificateHandler = new BypassCertificate();
            if (json != null)
            {
                req.SetRequestHeader("Content-Type", "application/json");
                var formData = System.Text.Encoding.UTF8.GetBytes(json);
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }
            if (form != null)
            {
                req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                byte[] rawFormData = form.data;
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawFormData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }
            
            yield return req.SendWebRequest();

            // Обработка ошибки
            if (req.isNetworkError || req.responseCode != 200)
            {
                                if (req.error == "Cannot resolve destination host")
                {
                     StartCoroutine(Timeout(Method, api, callback, json, parm, form));
                    yield break;
                }
                Debug.Log("Error While Sending: " + req.error);
                error(req.downloadHandler.text, req.error);
                callback(req.downloadHandler.text);
            }
            // Успешное выполнение запроса PUT
            else
            {
                Debug.Log("Received: " + req.downloadHandler.text);
                if (req.downloadHandler.text.StartsWith("["))
                {
                    NiceJson = "{\"result\":" + req.downloadHandler.text + "}";
                }
                else
                {
                    NiceJson = "{\"result\":[" + req.downloadHandler.text + "]}";
                }
                Debug.Log(NiceJson);
                 preloader.GetComponent<Preload_script>().preload_object.SetActive(false);
                 preloader.GetComponent<Preload_script>().preload_text.text = "";
                callback(NiceJson);
            }

        }
        // Выполнение запроса DEL
        if (Method == "DEL")
        {
            UnityWebRequest req = UnityWebRequest.Delete(URL + parm);
            req.certificateHandler = new BypassCertificate();
            req.SetRequestHeader("Authorization", "Bearer " + AuthKey);
            req.SetRequestHeader("Content-Type", "application/json");
            if (json != null)
            {
                req.SetRequestHeader("Content-Type", "application/json");
                var formData = System.Text.Encoding.UTF8.GetBytes(json);
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }
            if (form != null)
            {
                req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                byte[] rawFormData = form.data;
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawFormData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }


            yield return req.SendWebRequest();

            // Обработка ошибки
            if (req.isNetworkError || req.responseCode != 200)
            {
                                if (req.error == "Cannot resolve destination host")
                {
                     StartCoroutine(Timeout(Method, api, callback, json, parm, form));
                    yield break;
                }
                Debug.Log("Error While Sending: " + req.error);
                error(req.downloadHandler.text, req.error);
                callback(req.downloadHandler.text);
            }
            // Успешное выполнение запроса DEL
            else
            {
                preloader.GetComponent<Preload_script>().preload_object.SetActive(false);
                preloader.GetComponent<Preload_script>().preload_text.text = "";
                callback("ok");
            }
        }
        // Выполнение запроса PATCH
        if (Method == "PATCH")
        {
            UnityWebRequest req = UnityWebRequest.Put(URL + parm, "test");
            req.certificateHandler = new BypassCertificate();
            req.method = "PATCH";
            req.SetRequestHeader("Authorization", "Bearer " + AuthKey);
            req.SetRequestHeader("Content-Type", "application/json");
            if (json != null)
            {
                req.SetRequestHeader("Content-Type", "application/json");
                var formData = System.Text.Encoding.UTF8.GetBytes(json);
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }
            if (form != null)
            {
                req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                byte[] rawFormData = form.data;
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawFormData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }
            
            yield return req.SendWebRequest();

            // Обработка ошибки
            if (req.isNetworkError || req.responseCode != 200)
            {
                                if (req.error == "Cannot resolve destination host")
                {
                     StartCoroutine(Timeout(Method, api, callback, json, parm, form));
                    yield break;
                }
                Debug.Log("Error While Sending: " + req.error);
                error(req.downloadHandler.text, req.error);
                callback(req.downloadHandler.text);
            }
            // Успешное выполнение запроса PATCH
            else
            {
                Debug.Log("Received: " + req.downloadHandler.text);
                if (req.downloadHandler.text.StartsWith("["))
                {
                    NiceJson = "{\"result\":" + req.downloadHandler.text + "}";
                }
                else
                {
                    NiceJson = "{\"result\":[" + req.downloadHandler.text + "]}";
                }
                Debug.Log(NiceJson);
                 preloader.GetComponent<Preload_script>().preload_object.SetActive(false);
                 preloader.GetComponent<Preload_script>().preload_text.text = "";
                callback(NiceJson);
            }

        }

        preloader.GetComponent<Preload_script>().preload_object.SetActive(false);
    }
  

    /// <summary>
    /// IEnumerator Timeout
    /// выполняется в случае сбоя в интернет соединении / долгого ответа от сервера
    /// во время выполнения запроса
    /// </summary>
    /// <param name="Method">Выбор метода запроса</param>
    /// <param name="api">Путь метода API</param>
    /// <param name="callback">Название фукнции которое примет значение ответа</param>
    /// <param name="json">Передача JSON для некоторых запросов API</param>
    /// <param name="parm">Параметры для некоторых запросов API</param>
    /// <param name="form">Форма для некоторых запросов API</param>
    IEnumerator Timeout(string Method, string api, [CanBeNull] System.Action<string> callback, string json = null, string parm = null, WWWForm form = null)
    {
      yield return new WaitForSeconds(7f);
      if (preloader.GetComponent<Preload_script>().preload_object.activeSelf)
      {
          preloader.GetComponent<Preload_script>().preload_text.text = "Нет соединения с интернетом, повторная попытка...";
          //Повторная попытка соединения
         StartCoroutine(request(Method,api,callback,json,parm,form));
      }
    }

    /// <summary>
    /// error
    /// Выполняется в случае ошибки при запросе
    /// </summary>
    private void error(string response = null, string code = null)
    {
        if (response != "")
        {
        if (response.StartsWith("{"))
        {
        response = "{\"result\":" + response + "}";
        jsonError jsnError = JsonUtility.FromJson<jsonError>(response);
        allert_obj.GetComponent<UpdateAnim>().anim_allert(jsnError.result.detail);
        }
        else
        {
            allert_obj.GetComponent<UpdateAnim>().anim_allert(code);
        }
        }
        else
        {
           allert_obj.GetComponent<UpdateAnim>().anim_allert("Ошибка соединения"); 
        }
        if (Login == null)
        {    
          Login = PlayerPrefs.GetString("Login");  
        }
        //Сохранение скриншота экрана приложения
        ScreenCapture.CaptureScreenshot("SomeLevel"); 
        Debug.Log( "Платформа: "+Application.platform + " билд: " + Application.version + "\n"+ "Код: "+ code + "\n"+ "Ответ от сервера: " + System.Text.RegularExpressions.Regex.Unescape(response) + "\n" + "\n" +"Время: " + System.DateTime.Now.ToString("HH:mm"));
        //Отправка скриншота экрана приложения и другой необходимой информации в телеграм бота.
        telegram.GetComponent<Telegram>().SendMessage( "Платформа: "+Application.platform + " билд: " + Application.version + "\n"+ "Код: "+ code + "\n"+ "Логин: "+ Login + "\n"+ "Ответ от сервера: " + System.Text.RegularExpressions.Regex.Unescape(response) + "\n" + "\n" +"Время: " + System.DateTime.Now.ToString("HH:mm"));
    }
}
