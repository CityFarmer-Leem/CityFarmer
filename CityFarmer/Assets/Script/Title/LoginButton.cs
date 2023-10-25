using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => StartLogin());
    }

    private void StartLogin()
    {
        // TODO : ���� �α��� ��� ���� �� �߰�
        Debug.Log("This is Login Button");
    }
}

