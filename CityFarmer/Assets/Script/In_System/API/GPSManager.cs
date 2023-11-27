using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance;
   
    private float _maxWaitTime = 10.0f;
    private float _resendTime = 1.0f;
    public WeatherAPI Weather;
    //���� �浵 ����
    private float _latitude = 0;
    private float _longitude = 0;
    private float _waitTime = 0;

    private bool _receiveGPS = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [System.Obsolete]
    void Start()
    {
        StartCoroutine(GPS_On());
    }

    //GPSó�� �Լ�
    [System.Obsolete]
    public IEnumerator GPS_On()
    {
       
        //����,GPS��� �㰡�� ���� ���ߴٸ�, ���� �㰡 �˾��� ���
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        //���� GPS ��ġ�� ���� ���� ������ ��ġ ������ ������ �� ���ٰ� ǥ��
#if UNITY_EDITOR
        //Wait until Unity connects to the Unity Remote, while not connected, yield return null
        while (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            yield return null;
        }
#endif
      
        if (!Input.location.isEnabledByUser)
        {
         
            yield break;
        }

        //��ġ �����͸� ��û -> ���� ���
        Input.location.Start();

        //GPS ���� ���°� �ʱ� ���¿��� ���� �ð� ���� �����
        while (Input.location.status == LocationServiceStatus.Initializing && _waitTime < _maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            _waitTime++;
        }

        //���� ���� �� ������ ���еƴٴ� ���� ���
        if (Input.location.status == LocationServiceStatus.Failed)
        {
        
        }

        //���� ��� �ð��� �Ѿ���� ������ �����ٸ� �ð� �ʰ������� ���
        if (_waitTime >= _maxWaitTime)
        {
          
        }

        //���ŵ� GPS �����͸� ȭ�鿡 ���/

        LocationInfo li = Input.location.lastData;
        /*latitude = li.latitude;
       longitude = li.longitude;
       latitude_text.text = "���� : " + latitude.ToString();
       longitude_text.text = "�浵 : " + longitude.ToString();
       */
        //��ġ ���� ���� ���� üũ
        _receiveGPS = true;
        Weather.CheckCityWeather(_latitude, _longitude);
        //��ġ ������ ���� ���� ���� resendTime ������� ��ġ ������ �����ϰ� ���
        while (_receiveGPS)
        {
            li = Input.location.lastData;
            _latitude = li.latitude;
            _longitude = li.longitude;

            yield return new WaitForSeconds(_resendTime);
        }
      
        
    }
}
