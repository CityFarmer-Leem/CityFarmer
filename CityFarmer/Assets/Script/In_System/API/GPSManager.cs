using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPSManager : MonoBehaviour
{
   
   
    private float _maxWaitTime = 10.0f;
    private float _resendTime = 1.0f;
    public WeatherAPI Weather;
    //���� �浵 ����
    private float _latitude = 0;
    private float _longitude = 0;
    private float _waitTime = 0;

    private bool _receiveGPS = false;

    private static GPSManager _instance;
    public static GPSManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GPSManager)) as GPSManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        StartCoroutine(GPS_On());
    }

    //GPSó�� �Լ�

    public IEnumerator GPS_On()
    {
#if UNITY_EDITOR
        //����Ƽ ����Ʈ ���
        while (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            yield return null;
        }
#endif

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
            Debug.Log("���� ����");
        }

        //���� ��� �ð��� �Ѿ���� ������ �����ٸ� �ð� �ʰ������� ���
        if (_waitTime >= _maxWaitTime)
        {
            Debug.Log("�ð� �ʰ�");
        }
        Input.location.Start();
       
      
        LocationInfo li = Input.location.lastData;

        _receiveGPS = true;

      
        while (_receiveGPS)
        {
            li = Input.location.lastData;
            _latitude = li.latitude;
            _longitude = li.longitude;
            if(_latitude != 0)
            {
                break;
            }
            yield return new WaitForSeconds(_resendTime);
        }

        Weather.CheckCityWeather(_latitude, _longitude);
    }
}
