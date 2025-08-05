using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class MainCharacter : MonoBehaviour
{
    [Header("ĳ���� �ʿ� �ڿ�")]
    [SerializeField] GameObject _prafabProjectile;
    [Header("ĳ���� ����")]
    [SerializeField] float _rotAngleY = 120;
    [SerializeField] float _rotAngleX = 60;

    Camera _myCam;
    VirtualStick _vStick;

    //�˻�� ����==
    string _value;
    string _value2;
    //==


    //�ӽ� ==
    private void Start()
    {
        InitCharacter();
    }
    //==
    private void Update()
    {
        float rotY = 0;
        float rotX = 0;
        float delta = Time.deltaTime;

#if UNITY_ANDROID

        {

            //Vector3 eulerAngle = transform.rotation.eulerAngles;

            ////�¿�� ȸ��, ���Ʒ��� ȸ��
            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    eulerAngle.x -= delta * _rotAngleX;
            //}
            //if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    eulerAngle.x += delta * _rotAngleX;
            //}

            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    eulerAngle.y -= delta * _rotAngleY;
            //}
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    eulerAngle.y += delta * _rotAngleY;
            //}
        }

        rotX = _vStick._verticalRawValue;
        rotY = _vStick._horizontalRawValue;

        if (Input.touchCount > 0)
        {

            for (int i = 0; i < Input.touchCount; i++)
            {
                _value = Input.GetTouch(i).fingerId.ToString();
                _value2 = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId).ToString();
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        Ray ray = _myCam.ScreenPointToRay(Input.GetTouch(i).position);
                           Launch(ray);
                        
                    }
                }
            }
        }
#else
        rotY = Input.GetAxis("Horizontal");
        rotX = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = _myCam.ScreenPointToRay(Input.mousePosition);
                Launch(ray);
            }
        }
#endif
        transform.Rotate(_rotAngleY * delta * rotY * Vector3.up);
        _myCam.transform.Rotate(_rotAngleX * delta * rotX * Vector3.left);

    }


    public void InitCharacter()
    {
        _myCam = Camera.main;
#if UNITY_ANDROID
        _vStick = GameObject.FindGameObjectWithTag("VirtualStick").GetComponent<VirtualStick>();
#endif
    }

    void Launch(Ray ray)
    {

        MagicProjectile fireBall = Instantiate(_prafabProjectile, ray.origin, Quaternion.identity)
                                                 .GetComponent<MagicProjectile>();

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            fireBall.InitProjectile(this, ray.direction, hit.transform);
        }
        else
            fireBall.InitProjectile(this, ray.direction);
    }


    void OnGUI()
    {
        GUIStyle st = new GUIStyle();
        st.fontSize = 100;
        GUI.Label(new Rect(10, 100, 200, 40), _value, st);
        GUI.Label(new Rect(10, 200, 200, 40), _value2, st);
    }

}
