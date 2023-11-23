using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public class WeatherAPI : MonoBehaviour
{
    //API �ּ�
    //===============================================
    public const string API_ADDRESS = @"api.openweathermap.org/data/2.5/weather?q=Seoul&appid=e";
    //===============================================

    //���� �����Ͱ� �ٿ�ε�Ǹ� CallBack���� �ʿ��� �Լ��� ���ư���
    public delegate void WeatherDataCallback(WeatherData weatherData);

    //�ٿ�ε�� ���� ������. �ߺ� �ٿ�ε带 �������Ͽ� �����صд�
    private WeatherData _weatherData;

    /// <summary>
    /// API�κ��� ���� �����͸� �޾ƿ´�
    /// </summary>
    public void GetWeather(WeatherDataCallback callback)
    {
        //������ ���� �����Ͱ� ���ٸ� API�κ��� �޾ƿ´�
        if (_weatherData == null)
        {
            StartCoroutine(CoGetWeather(callback));
        }
        else
        {
            //������ ���� �����Ͱ� �����Ѵٸ� �� ���������͸� �״�� ����Ѵ�
            callback(_weatherData);
        }
    }

    /// <summary>
    /// ���� API�κ��� ������ �޾ƿ´�
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    [System.Obsolete]
    private IEnumerator CoGetWeather(WeatherDataCallback callback)
    {
        Debug.Log("���� ������ �ٿ�ε��մϴ�");

        var webRequest = UnityWebRequest.Get(API_ADDRESS);
        yield return webRequest.SendWebRequest();

        //���� ������ ���� ���
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
            yield break;
        }

        //�ٿ�ε� �Ϸ�
        var downloadedTxt = webRequest.downloadHandler.text;

        Debug.Log("���� ������ �ٿ�ε� �Ǿ����ϴ�! : " + downloadedTxt);

        //����Ƽ ���� ��ġ�Ƿ� base�� ����� �� ���⶧���� Replace�� �ʿ��ϴ�
        string weatherStr = downloadedTxt.Replace("base", "station");

        _weatherData = JsonUtility.FromJson<WeatherData>(weatherStr);
        callback(_weatherData);
    }
}
