using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Author: Saran Krishnaraja
//https://github.com/xenfinity-software/click-objects-unity-tutorial/releases/tag/1

public class Warden : NetworkBehaviour
{
    [SerializeField]
    private ArduinoInputs HID;
    [SerializeField]
    int HIDInput;
    [SerializeField]
    int CrankCounter;

    public GameObject playerCamera;

    public GameObject pauseMenuUI;

    public GameObject Area1Lights;
    public GameObject Area2Lights;
    public GameObject Area3Lights;
    public GameObject Area4Lights;

    public GameObject Area1Doors;
    public GameObject Area2Doors;
    public GameObject Area3Doors;
    public GameObject Area4Doors;

    public Guard guard1;
    public Guard guard2;
    public Guard guard3;
    public Guard guard4;
    public Guard guard5;
    public Guard guard6;
    public Guard guard7;
    public Guard guard8;
    public Guard guard9;
    public Guard guard10;
    public Guard guard11;
    public Guard guard12;

    public Light guard1Light;
    public Light guard2Light;
    public Light guard3Light;
    public Light guard4Light;
    public Light guard5Light;
    public Light guard6Light;
    public Light guard7Light;
    public Light guard8Light;
    public Light guard9Light;
    public Light guard10Light;
    public Light guard11Light;
    public Light guard12Light;

    // The objects on the warden board
    public GameObject plugOn1;
    public GameObject plugOn2;
    public GameObject plugOn3;
    public GameObject plugOn4;
    public GameObject plugOn5;
    public GameObject plugOn6;
    public GameObject plugOff1;
    public GameObject plugOff2;
    public GameObject plugOff3;
    public GameObject plugOff4;
    public GameObject plugOff5;
    public GameObject plugOff6;

    public GameObject switchOn1;
    public GameObject switchOn2;
    public GameObject switchOn3;
    public GameObject switchOn4;
    public GameObject switchOff1;
    public GameObject switchOff2;
    public GameObject switchOff3;
    public GameObject switchOff4;

    public GameObject droneRight;
    public GameObject droneLeft;

    public GameObject crankObject;

    public bool power = true;

    bool tempPower = true;

    // Switches
    [SerializeField]
    bool switch1 = false;
    [SerializeField]
    bool switch2 = false;
    [SerializeField]
    bool switch3 = false;
    [SerializeField]
    bool switch4 = false;
    [SerializeField]
    int switchCount = 0;
    // Save switches bools when power turns off
    [SerializeField]
    bool tempSwitch1 = true;
    [SerializeField]
    bool tempSwitch2 = true;
    [SerializeField]
    bool tempSwitch3 = true;
    [SerializeField]
    bool tempSwitch4 = true;

    // Plugs
    [SerializeField]
    bool plug1 = false;
    [SerializeField]
    bool plug2 = false;
    [SerializeField]
    bool plug3 = false;
    [SerializeField]
    bool plug4 = false;
    [SerializeField]
    bool plug5 = false;
    [SerializeField]
    bool plug6 = false;
    [SerializeField]
    int plugCount = 0;
    // Save plug bools when power turns off
    [SerializeField]
    bool tempPlug1 = true;
    [SerializeField]
    bool tempPlug2 = true;
    [SerializeField]
    bool tempPlug3 = true;
    [SerializeField]
    bool tempPlug4 = true;
    [SerializeField]
    bool tempPlug5 = true;
    [SerializeField]
    bool tempPlug6 = true;

    private float nextActionTime = 0.0f;
    public float period = 1.0f;

    public Behaviour StaticCC;

    int DroneCamera = 1;
    [SerializeField]
    GameObject DroneCamera1;
    [SerializeField]
    GameObject DroneCamera2;
    [SerializeField]
    GameObject DroneCamera3;

    // Game UI
    // Power
    int powerCount = 0;

    public GameObject powerUI0;
    public GameObject powerUI1;
    public GameObject powerUI2;
    public GameObject powerUI3;
    public GameObject powerUI4;
    public GameObject powerUI5;
    public GameObject powerUI6;
    public GameObject powerUI7;



    // Key
    [SyncVar] bool key = false;
    public GameObject yesKey;
    public GameObject noKey;

    // Timer 
    public Text uiText;
    public float mainTimer;
    [SyncVar] float timer;
    [SyncVar]bool canCount = true;
    [SyncVar] bool doOnce = false;
    public GameObject green;
    public GameObject orange;
    public GameObject red;


    Vector3 emptyVec3 = new Vector3(0, 0, 0);
    Quaternion emptyQuat = new Quaternion(0, 0, 0, 0);

    private NetworkManager networkManager;

    void Start()
    {
        if (!hasAuthority)
            return;

        // Set the Settings
        //power = true;
        //tempPower = true;
        //switch1 = false;
        //switch2 = false;
        //switch3 = false;
        //switch4 = false;
        //switchCount = 0;
        //tempSwitch1 = true;
        //tempSwitch2 = true;
        //tempSwitch3 = true;
        //tempSwitch4 = true;
        //plug1 = false;
        //plug2 = false;
        //plug3 = false;
        //plug4 = false;
        //plug5 = false;
        //plug6 = false;
        //plugCount = 0;
        //tempPlug1 = true;
        //tempPlug2 = true;
        //tempPlug3 = true;
        //tempPlug4 = true;
        //tempPlug5 = true;
        //tempPlug6 = true;

        // Call the functions to update the map at the start
        CmdCheckPower();
        CmdCheckLights();
        CmdCheckGuards();

        CmdUpdateLights(1);
        CmdUpdateLights(2);
        CmdUpdateLights(3);
        CmdUpdateLights(4);

        CmdUpdateGuards(1);
        CmdUpdateGuards(2);
        CmdUpdateGuards(3);
        CmdUpdateGuards(4);
        CmdUpdateGuards(5);
        CmdUpdateGuards(6);

        CmdCheckDrones();

        timer = mainTimer;

        networkManager = NetworkManager.singleton;
    }

    void MakeSureUpdate()
    {
        if (Time.time > nextActionTime)
        {
            CmdTimeUpdate();

            CmdUpdateLights(1);
            CmdUpdateLights(2);
            CmdUpdateLights(3);
            CmdUpdateLights(4);

            CmdUpdateGuards(1);
            CmdUpdateGuards(2);
            CmdUpdateGuards(3);
            CmdUpdateGuards(4);
            CmdUpdateGuards(5);
            CmdUpdateGuards(6);

            CmdCheckDrones();

            //CmdLockPos();
        }
    }

    [Command]
    void CmdTimeUpdate()
    {
        RpcTimeUpdate();
    }
    [ClientRpc]
    void RpcTimeUpdate()
    {
        nextActionTime = Time.time + period;
    }

    // Update is called once per frame
    void Update()
    {
        //CmdUpdateGenerator();
        if (!hasAuthority)
        {
            playerCamera.SetActive(false);
            return;
        }

        // Update the HID Input
        CmdUpdateHIDInput();
        CmdUpdateHID();

        // Check the inputs and power
        InputHandler();
        CmdCheckPower();

        // If theres power check the lights and guards
        if (power)
        {
            CmdCheckLights();
            CmdCheckGuards();
            CmdCheckDrones();
        }

        // Makes sure everyone is updated and good
        MakeSureUpdate();

        // UI
        UpdatePowerUI();    // Update Power UI
        CmdUpdateTimer();   // Updates timer
        UpdateKey();        // Update Key UI
    }

    [Command]
    void CmdLockPos()
    {
        RpcLockPos();
    }
    [ClientRpc]
    void RpcLockPos()
    {
        // Lock Position
        //transform.position = emptyVec3;
        //transform.rotation = emptyQuat;
    }

    // Called every update to handle inputs
    void InputHandler()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // If the mouse is clicking on a object
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            // If you hit something
            if (hit.transform != null)
            {
                //Debug.Log(hit.collider.transform.parent.gameObject);
                if(power)
                {
                    LightInputs(hit);
                    GuardInputs(hit);
                    DroneInputs(hit);
                }
                else
                {
                    CrankInputs(hit);
                }
            }
        }

        // Turn off power
        if (Input.GetKeyDown(KeyCode.T))
        {
            CmdUpdatePower(false);
        }

        // Pause 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
            PauseMenu.IsOn = pauseMenuUI.activeSelf;
        }
    }

    void LightInputs(RaycastHit hit)
    {
        if (hit.collider.transform.parent.gameObject == switchOff1)
        {
            Renderer rend = switchOff1.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch1)
                {
                    Debug.Log("Can't Switch Light: " + switch1);
                    return;
                }

                CmdChangeSwitch1();
                CmdChangeSwitchCount(switch1);
            }
        }
        else if (hit.collider.transform.parent.gameObject == switchOn1)
        {
            Renderer rend = switchOn1.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch1)
                {
                    Debug.Log("Can't Switch Light: " + switch1);
                    return;
                }

                CmdChangeSwitch1();
                CmdChangeSwitchCount(switch1);
            }
        }
        else if (hit.collider.transform.parent.gameObject == switchOff2)
        {
            Renderer rend = switchOff2.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch2)
                {
                    Debug.Log("Can't Switch Light: " + switch2);
                    return;
                }

                CmdChangeSwitch2();
                CmdChangeSwitchCount(switch2);
            }
          
        }
        else if (hit.collider.transform.parent.gameObject == switchOn2)
        {
            Renderer rend = switchOn2.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch2)
                {
                    Debug.Log("Can't Switch Light: " + switch2);
                    return;
                }

                CmdChangeSwitch2();
                CmdChangeSwitchCount(switch2);
            }
            
        }
        else if(hit.collider.transform.parent.gameObject == switchOff3)
        {
            Renderer rend = switchOff3.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch3)
                {
                    Debug.Log("Can't Switch Light: " + switch3);
                    return;
                }

                CmdChangeSwitch3();
                CmdChangeSwitchCount(switch3);
            }
            
        }
        else if (hit.collider.transform.parent.gameObject == switchOn3)
        {
            Renderer rend = switchOn3.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch3)
                {
                    Debug.Log("Can't Switch Light: " + switch3);
                    return;
                }

                CmdChangeSwitch3();
                CmdChangeSwitchCount(switch3);
            }
           
        }
        else if(hit.collider.transform.parent.gameObject == switchOff4)
        {
            Renderer rend = switchOff4.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch4)
                {
                    Debug.Log("Can't Switch Light: " + switch4);
                    return;
                }

                CmdChangeSwitch4();
                CmdChangeSwitchCount(switch4);
            }
            
        }
        else if (hit.collider.transform.parent.gameObject == switchOn4)
        {
            Renderer rend = switchOn4.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchLights() && !switch4)
                {
                    Debug.Log("Can't Switch Light: " + switch4);
                    return;
                }

                CmdChangeSwitch4();
                CmdChangeSwitchCount(switch4);
            }
            
        }
    }

    void GuardInputs(RaycastHit hit)
    {
        if (hit.collider.transform.parent.gameObject == plugOff1)
        {
            Renderer rend = plugOff1.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug1)
                {
                    Debug.Log("Can't Switch Plug: " + plug1);
                    return;
                }

                CmdChangePlug1();
                CmdChangePlugCount(plug1);
            }
        }
        else if (hit.collider.transform.parent.gameObject == plugOn1)
        {
            Renderer rend = plugOn1.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug1)
                {
                    Debug.Log("Can't Switch Plug: " + plug1);
                    return;
                }

                CmdChangePlug1();
                CmdChangePlugCount(plug1);
            }
        }
        else if(hit.collider.transform.parent.gameObject == plugOff2)
        {
            Renderer rend = plugOff2.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug2)
                {
                    Debug.Log("Can't Switch Plug: " + plug2);
                    return;
                }

                CmdChangePlug2();
                CmdChangePlugCount(plug2);
            }
        }
        else if (hit.collider.transform.parent.gameObject == plugOn2)
        {
            Renderer rend = plugOn2.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug2)
                {
                    Debug.Log("Can't Switch Plug: " + plug2);
                    return;
                }

                CmdChangePlug2();
                CmdChangePlugCount(plug2);
            }
        }
        else if(hit.collider.transform.parent.gameObject == plugOff3)
        {
            Renderer rend = plugOff3.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug3)
                {
                    Debug.Log("Can't Switch Plug: " + plug3);
                    return;
                }

                CmdChangePlug3();
                CmdChangePlugCount(plug3);
            }
        }
        else if (hit.collider.transform.parent.gameObject == plugOn3)
        {
            Renderer rend = plugOn3.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug3)
                {
                    Debug.Log("Can't Switch Plug: " + plug3);
                    return;
                }

                CmdChangePlug3();
                CmdChangePlugCount(plug3);
            }
        }
        else if(hit.collider.transform.parent.gameObject == plugOff4)
        {
            Renderer rend = plugOff4.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug4)
                {
                    Debug.Log("Can't Switch Plug: " + plug4);
                    return;
                }

                CmdChangePlug4();
                CmdChangePlugCount(plug4);
            }
        }
        else if (hit.collider.transform.parent.gameObject == plugOn4)
        {
            Renderer rend = plugOn4.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug4)
                {
                    Debug.Log("Can't Switch Plug: " + plug4);
                    return;
                }

                CmdChangePlug4();
                CmdChangePlugCount(plug4);
            }
        }
        else if(hit.collider.transform.parent.gameObject == plugOff5)
        {
            Renderer rend = plugOff5.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug5)
                {
                    Debug.Log("Can't Switch Plug: " + plug5);
                    return;
                }

                CmdChangePlug5();
                CmdChangePlugCount(plug5);
            }
        }
        else if (hit.collider.transform.parent.gameObject == plugOn5)
        {
            Renderer rend = plugOn5.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug5)
                {
                    Debug.Log("Can't Switch Plug: " + plug5);
                    return;
                }

                CmdChangePlug5();
                CmdChangePlugCount(plug5);
            }
        }
        else if(hit.collider.transform.parent.gameObject == plugOff6)
        {
            Renderer rend = plugOff6.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug6)
                {
                    Debug.Log("Can't Switch Plug: " + plug6);
                    return;
                }

                CmdChangePlug6();
                CmdChangePlugCount(plug6);
            }
        }
        else if (hit.collider.transform.parent.gameObject == plugOn6)
        {
            Renderer rend = plugOn6.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!CanSwitchPlugs() && !plug6)
                {
                    Debug.Log("Can't Switch Plug: " + plug6);
                    return;
                }

                CmdChangePlug6();
                CmdChangePlugCount(plug6);
            }
        }
    }

    void DroneInputs(RaycastHit hit)
    {
        if (hit.collider.transform.parent.gameObject == droneLeft)
        {
            Renderer rend = droneLeft.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                CmdBackDrone();

            }
        }
        else if (hit.collider.transform.parent.gameObject == droneRight)
        {
            Renderer rend = droneRight.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                CmdForwardDrone();

            }
        }
    }

    void CrankInputs(RaycastHit hit)
    {
        if (hit.collider.transform.parent.gameObject == crankObject)
        {
            Renderer rend = crankObject.GetComponentInChildren<Renderer>();
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.red);
            }
            if (Input.GetMouseButtonDown(0))
            {
                CmdCrankCounter();

            }
        }
    }

    // Check if you can switch a light
    bool CanSwitchLights()
    {
        if (switchCount == 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Check if you can switch a plug
    bool CanSwitchPlugs()
    {
        if (plugCount == 4)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    #region HID
    // Update the input from the HID
    [Command]
    void CmdUpdateHIDInput()
    {
        RpcUpdateHIDInput();
    }
    [ClientRpc]
    void RpcUpdateHIDInput()
    {
        HIDInput = HID.GetHIDInput();
    }

    // Updates the warden board using the HID inputs
    [Command]
    void CmdUpdateHID()
    {
        RpcUpdateHID();
    }
    [ClientRpc]
    void RpcUpdateHID()
    {
        switch (HIDInput)
        {
            case 21:
                if (power)
                {
                    if (!plug1)
                    {
                        if (!CanSwitchPlugs() && !plug1)
                        {
                            Debug.Log("Can't Switch Plug: " + plug1);
                            break;
                        }
                        else
                        {
                            CmdChangePlug1();
                            CmdChangePlugCount(plug1);
                        }
                    }
                }
                break;
            case 1:
                if(plug1)
                {
                    CmdChangePlug1();
                    CmdChangePlugCount(plug1);
                }
                break;
            case 2:
                if (power)
                {
                    if (!plug2)
                    {
                        if (!CanSwitchPlugs() && !plug2)
                        {
                            Debug.Log("Can't Switch Plug: " + plug2);
                            break;
                        }
                        else
                        {
                            CmdChangePlug2();
                            CmdChangePlugCount(plug2);
                        }
                    }
                }
                break;
            case 3:
                if(plug2)
                {
                    CmdChangePlug2();
                    CmdChangePlugCount(plug2);
                }
                break;
            case 4:
                if (power)
                {
                    if (!plug3)
                    {
                        if (!CanSwitchPlugs() && !plug3)
                        {
                            Debug.Log("Can't Switch Plug: " + plug3);
                            break;
                        }
                        else
                        {
                            CmdChangePlug3();
                            CmdChangePlugCount(plug3);
                        }
                    }
                }
                break;
            case 5:
                if(plug3)
                {
                    CmdChangePlug3();
                    CmdChangePlugCount(plug3);
                }
                break;
            case 6:
                if (power)
                {
                    if (!plug4)
                    {
                        if (!CanSwitchPlugs() && !plug4)
                        {
                            Debug.Log("Can't Switch Plug: " + plug4);
                            break;
                        }
                        else
                        {
                            CmdChangePlug4();
                            CmdChangePlugCount(plug4);
                        }
                    }
                }
                break;
            case 7:
                if(plug4)
                {
                    CmdChangePlug4();
                    CmdChangePlugCount(plug4);
                }
                break;
            case 8:
                if (power)
                {
                    if (!plug5)
                    {
                        if (!CanSwitchPlugs() && !plug5)
                        {
                            Debug.Log("Can't Switch Plug: " + plug5);
                            break;
                        }
                        else
                        {
                            CmdChangePlug5();
                            CmdChangePlugCount(plug5);
                        }
                    }
                }
                break;
            case 9:
                if(plug5)
                {
                    CmdChangePlug5();
                    CmdChangePlugCount(plug5);
                }
                break;
            case 10:
                if (power)
                {
                    if (!plug6)
                    {
                        if (!CanSwitchPlugs() && !plug6)
                        {
                            Debug.Log("Can't Switch Plug: " + plug6);
                            break;
                        }
                        else
                        {
                            CmdChangePlug6();
                            CmdChangePlugCount(plug6);
                        }
                    }
                }
                break;
            case 11:
                if(plug6)
                {
                    CmdChangePlug6();
                    CmdChangePlugCount(plug6);
                }
                break;
            case 12:
                if (power)
                {
                    if (!switch1)
                    {
                        if (!CanSwitchLights() && !switch1)
                        {
                            Debug.Log("Can't Switch Light: " + switch1);
                            return;
                        }
                        else
                        {
                            CmdChangeSwitch1();
                            CmdChangeSwitchCount(switch1);
                            //switch1 = true;
                            //switchCount++;
                        }
                    }
                }
                break;
            case 13:
                if (switch1)
                {
                    CmdChangeSwitch1();
                    CmdChangeSwitchCount(switch1);
                    //switch1 = false;
                    //switchCount--;
                }
                break;
            case 14:
                if (power)
                {
                    if (!switch2)
                    {
                        if (!CanSwitchLights() && !switch2)
                        {
                            Debug.Log("Can't Switch Light: " + switch2);
                            return;
                        }
                        else
                        {
                            CmdChangeSwitch2();
                            CmdChangeSwitchCount(switch2);
                        }
                    }
                }
                break;
            case 15:
                if (switch2)
                {
                    CmdChangeSwitch2();
                    CmdChangeSwitchCount(switch2);
                }
                break;
            case 16:
                if (power)
                {
                    if (!switch3)
                    {
                        if (!CanSwitchLights() && !switch3)
                        {
                            Debug.Log("Can't Switch Light: " + switch3);
                            return;
                        }
                        else
                        {
                            CmdChangeSwitch3();
                            CmdChangeSwitchCount(switch3);
                        }
                    }
                }
                break;
            case 17:
                if (switch3)
                {
                    CmdChangeSwitch3();
                    CmdChangeSwitchCount(switch3);
                }
                break;
            case 18:
                if (power)
                {
                    if (!switch4)
                    {
                        if (!CanSwitchLights() && !switch4)
                        {
                            Debug.Log("Can't Switch Light: " + switch4);
                            return;
                        }
                        else
                        {
                            CmdChangeSwitch4();
                            CmdChangeSwitchCount(switch4);
                        }
                    }
                }
                break;
            case 19:
                if (switch4)
                {
                    CmdChangeSwitch4();
                    CmdChangeSwitchCount(switch4);
                }
                break;
            case 20:
                if (!power)
                {
                    CmdCrankCounter();
                }
                break;
        }
        
    }

    // The function to update the crank
    [Command]
    void CmdCrankCounter()
    {
        RpcCrankCounter();
    }
    [ClientRpc]
    void RpcCrankCounter()
    {
        CrankCounter++;
        if (CrankCounter > 10)
        {
            CmdUpdatePower(true);
            CrankCounter = 0;
        }
    }

    #endregion

    #region Power
    // Update the power
    // Used for when a electrical box is destoryed
    //public void UpdatePower(bool temp)
    //{
    //    CmdUpdatePower(temp);
    //}
    [Command]
    void CmdUpdatePower(bool temp)
    {
        RpcUpdatePower(temp);
    }
    [ClientRpc]
    void RpcUpdatePower(bool temp)
    {
        power = temp;
        Debug.Log("CMD Update Power: " + power);
    }

    // Checks to see if the power has changed
    [Command]
    void CmdCheckPower()
    {
        RpcCheckPower();
    }
    [ClientRpc]
    void RpcCheckPower()
    {
        if (tempPower != power)
        {
            if(!power)
            {
                switch1 = false;
                switch2 = false;
                switch3 = false;
                switch4 = false;
                switchCount = 0;
                tempSwitch1 = true;
                tempSwitch2 = true;
                tempSwitch3 = true;
                tempSwitch4 = true;
                plug1 = false;
                plug2 = false;
                plug3 = false;
                plug4 = false;
                plug5 = false;
                plug6 = false;
                plugCount = 0;
                tempPlug1 = true;
                tempPlug2 = true;
                tempPlug3 = true;
                tempPlug4 = true;
                tempPlug5 = true;
                tempPlug6 = true;

                StaticCC.enabled = true;

                DroneCamera1.SetActive(false);
                DroneCamera1.SetActive(false);
                DroneCamera1.SetActive(false);
            }
            else
            {
                switch1 = tempSwitch1;
                switch2 = tempSwitch2;
                switch3 = tempSwitch3;
                switch4 = tempSwitch4;
                plug1 = tempPlug1;
                plug2 = tempPlug2;
                plug3 = tempPlug3;
                plug4 = tempPlug4;
                plug5 = tempPlug5;
                plug6 = tempPlug6;

                StaticCC.enabled = false;
            }

        }

        tempPower = power;
    }

    #endregion

    #region Lights Functions

    // Checks to see if the lights has changed
    [Command]
    void CmdCheckLights()
    {
        RpcCheckLights();
    }
    [ClientRpc]
    void RpcCheckLights()
    {
        if (switch1 != tempSwitch1)
        {
            //Debug.Log("Area 1 Lights switched");
            CmdUpdateLights(1);
            if(switch1)
            {
                switchOff1.SetActive(false);
                switchOn1.SetActive(true);
            }
            else
            {
                switchOff1.SetActive(true);
                switchOn1.SetActive(false);
            }
        }
        if (switch2 != tempSwitch2)
        {
            //Debug.Log("Area 2 Lights switched");
            CmdUpdateLights(2);
            if (switch2)
            {
                switchOff2.SetActive(false);
                switchOn2.SetActive(true);
            }
            else
            {
                switchOff2.SetActive(true);
                switchOn2.SetActive(false);
            }
        }
        if (switch3 != tempSwitch3)
        {
            //Debug.Log("Area 3 Lights switched");
            CmdUpdateLights(3);
            if (switch3)
            {
                switchOff3.SetActive(false);
                switchOn3.SetActive(true);
            }
            else
            {
                switchOff3.SetActive(true);
                switchOn3.SetActive(false);
            }
        }
        if (switch4 != tempSwitch4)
        {
            //Debug.Log("Area 4 Lights switched");
            CmdUpdateLights(4);
            if (switch4)
            {
                switchOff4.SetActive(false);
                switchOn4.SetActive(true);
            }
            else
            {
                switchOff4.SetActive(true);
                switchOn4.SetActive(false);
            }
        }

        tempSwitch1 = switch1;
        tempSwitch2 = switch2;
        tempSwitch3 = switch3;
        tempSwitch4 = switch4;
    }

    // Update the lights
    [Command]
    void CmdUpdateLights(int switchNum)
    {
        RpcUpdateLights(switchNum);   
    }
    [ClientRpc]
    void RpcUpdateLights(int switchNum)
    {
        DoorOpen[] doorOpen;

        // Lights
        switch (switchNum)
        {
            case 1:
                //Debug.Log("Light 1 Updated");
                Area1Lights.SetActive(switch1);
                doorOpen = Area1Doors.GetComponentsInChildren<DoorOpen>(true);
                foreach (DoorOpen door in doorOpen)
                {
                    door.isDoorLocked = switch1;
                }
                break;
            case 2:
                Area2Lights.SetActive(switch2);
                doorOpen = Area2Doors.GetComponentsInChildren<DoorOpen>(true);
                foreach (DoorOpen door in doorOpen)
                {
                    door.isDoorLocked = switch2;
                }
                break;
            case 3:
                Area3Lights.SetActive(switch3);
                doorOpen = Area3Doors.GetComponentsInChildren<DoorOpen>(true);
                foreach (DoorOpen door in doorOpen)
                {
                    door.isDoorLocked = switch3;
                }
                break;
            case 4:
                Area4Lights.SetActive(switch4);
                doorOpen = Area4Doors.GetComponentsInChildren<DoorOpen>(true);
                foreach (DoorOpen door in doorOpen)
                {
                    door.isDoorLocked = switch4;
                }
                break;
        }

        //Debug.Log("Updating Lights");
    }

    // Changes the switches
    [Command]
    void CmdChangeSwitch1()
    {
        RpcChangeSwitch1();
    }
    [ClientRpc]
    void RpcChangeSwitch1()
    {
        switch1 = !switch1;
    }

    [Command]
    void CmdChangeSwitch2()
    {
        RpcChangeSwitch2();
    }
    [ClientRpc]
    void RpcChangeSwitch2()
    {
        switch2 = !switch2;
    }

    [Command]
    void CmdChangeSwitch3()
    {
        RpcChangeSwitch3();
    }
    [ClientRpc]
    void RpcChangeSwitch3()
    {
        switch3 = !switch3;
    }

    [Command]
    void CmdChangeSwitch4()
    {
        RpcChangeSwitch4();
    }
    [ClientRpc]
    void RpcChangeSwitch4()
    {
        switch4 = !switch4;
    }

    // Change the switch count
    [Command]
    void CmdChangeSwitchCount(bool temp)
    {
        RpcChangeSwitchCount(temp);
    }
    [ClientRpc]
    void RpcChangeSwitchCount(bool temp)
    {
        if (!temp)
        {
            switchCount++;
        }
        else
        {
            switchCount--;
        }
    }

    #endregion

    #region Guard Functions

    // Checks to see if the guards has changed
    [Command]
    void CmdCheckGuards()
    {
        RpcCheckGuards();
    }
    [ClientRpc]
    void RpcCheckGuards()
    {
        if (plug1 != tempPlug1)
        {
            CmdUpdateGuards(1);
            if (plug1)
            {
                plugOff1.SetActive(false);
                plugOn1.SetActive(true);
            }
            else
            {
                plugOff1.SetActive(true);
                plugOn1.SetActive(false);
            }
        }
        if (plug2 != tempPlug2)
        {
            CmdUpdateGuards(2);
            if (plug2)
            {
                plugOff2.SetActive(false);
                plugOn2.SetActive(true);
            }
            else
            {
                plugOff2.SetActive(true);
                plugOn2.SetActive(false);
            }
        }
        if (plug3 != tempPlug3)
        {
            CmdUpdateGuards(3);
            if (plug3)
            {
                plugOff3.SetActive(false);
                plugOn3.SetActive(true);
            }
            else
            {
                plugOff3.SetActive(true);
                plugOn3.SetActive(false);
            }
        }
        if (plug4 != tempPlug4)
        {
            CmdUpdateGuards(4);
            if (plug4)
            {
                plugOff4.SetActive(false);
                plugOn4.SetActive(true);
            }
            else
            {
                plugOff4.SetActive(true);
                plugOn4.SetActive(false);
            }
        }
        if (plug5 != tempPlug5)
        {
            CmdUpdateGuards(5);
            if (plug5)
            {
                plugOff5.SetActive(false);
                plugOn5.SetActive(true);
            }
            else
            {
                plugOff5.SetActive(true);
                plugOn5.SetActive(false);
            }
        }
        if (plug6 != tempPlug6)
        {
            CmdUpdateGuards(6);
            if (plug6)
            {
                plugOff6.SetActive(false);
                plugOn6.SetActive(true);
            }
            else
            {
                plugOff6.SetActive(true);
                plugOn6.SetActive(false);
            }
        }

        tempPlug1 = plug1;
        tempPlug2 = plug2;
        tempPlug3 = plug3;
        tempPlug4 = plug4;
        tempPlug5 = plug5;
        tempPlug6 = plug6;

    }

    // Updates the guards
    [Command]
    void CmdUpdateGuards(int switchNum)
    {
        RpcUpdateGuards(switchNum);
    }
    [ClientRpc]
    void RpcUpdateGuards(int switchNum)
    {
        // Guards
        switch (switchNum)
        {
            case 1:
                guard1.isOn = plug1;
                guard1Light.enabled = plug1;
                guard7.isOn = plug1;
                guard7Light.enabled = plug1;
                break;
            case 2:
                guard2.isOn = plug2;
                guard2Light.enabled = plug2;
                guard8.isOn = plug2;
                guard8Light.enabled = plug2;
                break;
            case 3:
                guard3.isOn = plug3;
                guard3Light.enabled = plug3;
                guard9.isOn = plug3;
                guard9Light.enabled = plug3;
                break;
            case 4:
                guard4.isOn = plug4;
                guard4Light.enabled = plug4;
                guard10.isOn = plug4;
                guard10Light.enabled = plug4;
                break;
            case 5:
                guard5.isOn = plug5;
                guard5Light.enabled = plug5;
                guard11.isOn = plug5;
                guard11Light.enabled = plug5;
                break;
            case 6:
                guard6.isOn = plug6;
                guard6Light.enabled = plug6;
                guard12.isOn = plug6;
                guard12Light.enabled = plug6;
                break;
        }
    }

    // Changes the plugs
    [Command]
    void CmdChangePlug1()
    {
        RpcChangePlug1();
    }
    [ClientRpc]
    void RpcChangePlug1()
    {
        plug1 = !plug1;
    }

    [Command]
    void CmdChangePlug2()
    {
        RpcChangePlug2();
    }
    [ClientRpc]
    void RpcChangePlug2()
    {
        plug2 = !plug2;
    }

    [Command]
    void CmdChangePlug3()
    {
        RpcChangePlug3();
    }
    [ClientRpc]
    void RpcChangePlug3()
    {
        plug3 = !plug3;
    }

    [Command]
    void CmdChangePlug4()
    {
        RpcChangePlug4();
    }
    [ClientRpc]
    void RpcChangePlug4()
    {
        plug4 = !plug4;
    }

    [Command]
    void CmdChangePlug5()
    {
        RpcChangePlug5();
    }
    [ClientRpc]
    void RpcChangePlug5()
    {
        plug5 = !plug5;
    }

    [Command]
    void CmdChangePlug6()
    {
        RpcChangePlug6();
    }
    [ClientRpc]
    void RpcChangePlug6()
    {
        plug6 = !plug6;
    }

    // Changes the plug count
    [Command]
    void CmdChangePlugCount(bool temp)
    {
        RpcChangePlugCount(temp);
    }
    [ClientRpc]
    void RpcChangePlugCount(bool temp)
    {
        if (!temp)
        {
            plugCount++;
        }
        else
        {
            plugCount--;
        }
    }


    #endregion

    #region Guard Lights

    public void UpdateGuardLights(int guard, float playerVisibleTimer, float timeToSpotPlayer)
    {
        CmdUpdateGuardLights(guard, playerVisibleTimer, timeToSpotPlayer);
    }
    [Command]
    void CmdUpdateGuardLights(int guard, float playerVisibleTimer, float timeToSpotPlayer)
    {
        RpcUpdateGuardLights(guard, playerVisibleTimer, timeToSpotPlayer);
    }
    [ClientRpc]
    void RpcUpdateGuardLights(int guard, float playerVisibleTimer, float timeToSpotPlayer)
    {
        // Guards light color update
        switch (guard)
        {
            case 1:
                guard1Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 2:
                guard2Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 3:
                guard3Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 4:
                guard4Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 5:
                guard5Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 6:
                guard6Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 7:
                guard7Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 8:
                guard8Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 9:
                guard9Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 10:
                guard10Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 11:
                guard11Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
            case 12:
                guard12Light.color = Color.Lerp(Color.yellow, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;
        }
    }

    #endregion

    #region Drone

    [Command]
    void CmdBackDrone()
    {
        RpcBackDrone();
    }
    [ClientRpc]
    void RpcBackDrone()
    {
        if (DroneCamera == 1)
        {
            DroneCamera = 3;
        }
        else
        {
            DroneCamera--;
        }
    }

    [Command]
    void CmdForwardDrone()
    {
        RpcForwardDrone();
    }
    [ClientRpc]
    void RpcForwardDrone()
    {
        if (DroneCamera == 3)
        {
            DroneCamera = 1;
        }
        else
        {
            DroneCamera++;
        }
    }

    [Command]
    void CmdCheckDrones()
    {
        RpcCheckDrones();
    }
    [ClientRpc]
    void RpcCheckDrones()
    {
        switch(DroneCamera)
        {
            case 1:
                DroneCamera1.SetActive(true);
                DroneCamera2.SetActive(false);
                DroneCamera3.SetActive(false);
                break;
            case 2:
                DroneCamera1.SetActive(false);
                DroneCamera2.SetActive(true);
                DroneCamera3.SetActive(false);
                break;
            case 3:
                DroneCamera1.SetActive(false);
                DroneCamera2.SetActive(false);
                DroneCamera3.SetActive(true);
                break;
        }

    }

    #endregion

    #region UI

    #region Power

    void UpdatePowerUI()
    {
        powerCount = plugCount + switchCount;

        switch (powerCount)
        {
            case 0: // full power
                powerUI0.SetActive(true);
                powerUI1.SetActive(false);
                powerUI2.SetActive(false);
                powerUI3.SetActive(false);
                powerUI4.SetActive(false);
                powerUI5.SetActive(false);
                powerUI6.SetActive(false);
                powerUI7.SetActive(false);
                break;
            case 1:
                powerUI0.SetActive(false);
                powerUI1.SetActive(true);
                powerUI2.SetActive(false);
                powerUI3.SetActive(false);
                powerUI4.SetActive(false);
                powerUI5.SetActive(false);
                powerUI6.SetActive(false);
                powerUI7.SetActive(false);
                break;
            case 2:
                powerUI0.SetActive(false);
                powerUI1.SetActive(false);
                powerUI2.SetActive(true);
                powerUI3.SetActive(false);
                powerUI4.SetActive(false);
                powerUI5.SetActive(false);
                powerUI6.SetActive(false);
                powerUI7.SetActive(false);
                break;
            case 3:
                powerUI0.SetActive(false);
                powerUI1.SetActive(false);
                powerUI2.SetActive(false);
                powerUI3.SetActive(true);
                powerUI4.SetActive(false);
                powerUI5.SetActive(false);
                powerUI6.SetActive(false);
                powerUI7.SetActive(false);
                break;
            case 4:
                powerUI0.SetActive(false);
                powerUI1.SetActive(false);
                powerUI2.SetActive(false);
                powerUI3.SetActive(false);
                powerUI4.SetActive(true);
                powerUI5.SetActive(false);
                powerUI6.SetActive(false);
                powerUI7.SetActive(false);
                break;
            case 5:
                powerUI0.SetActive(false);
                powerUI1.SetActive(false);
                powerUI2.SetActive(false);
                powerUI3.SetActive(false);
                powerUI4.SetActive(false);
                powerUI5.SetActive(true);
                powerUI6.SetActive(false);
                powerUI7.SetActive(false);
                break;
            case 6:
                powerUI0.SetActive(false);
                powerUI1.SetActive(false);
                powerUI2.SetActive(false);
                powerUI3.SetActive(false);
                powerUI4.SetActive(false);
                powerUI5.SetActive(false);
                powerUI6.SetActive(true);
                powerUI7.SetActive(false);
                break;
            case 7:
                powerUI0.SetActive(false);
                powerUI1.SetActive(false);
                powerUI2.SetActive(false);
                powerUI3.SetActive(false);
                powerUI4.SetActive(false);
                powerUI5.SetActive(false);
                powerUI6.SetActive(false);
                powerUI7.SetActive(true);
                break;
            default:
                Debug.Log("Warden.cs UpdatePowerUI(): ERROR");
                break;
        }
    }

    #endregion

    #region Key
    // Updates the UI on who has the Key
    void UpdateKey()
    {
        key = GameManager.GotKey();

        CmdUpdateKey();
    }

    [Command]
    void CmdUpdateKey()
    {
        RpcUpdateKey();
    }
    [ClientRpc]
    void RpcUpdateKey()
    {
        key = GameManager.GotKey();
        if (key)
        {
            noKey.SetActive(false);
            yesKey.SetActive(true);
        }
    }
    #endregion

    #region Timer

    // Timer
    [Command]
    void CmdUpdateTimer()
    {
        RpcUpdateTimer();
    }
    [ClientRpc]
    void RpcUpdateTimer()
    {
        //Debug.Log("Main Timer: " + mainTimer);
        //Debug.Log("Timer: " + timer);

        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;

            if (timer >= ((mainTimer / 3) * 2)) // If the timer is greater than 2/3 of the way done
            {
                green.SetActive(true);
                orange.SetActive(false);
                red.SetActive(false);
            }
            else if (timer >= (mainTimer / 3)) // If the timer is greater than 1/3 of the way done
            {
                green.SetActive(false);
                orange.SetActive(true);
                red.SetActive(false);
            }
            else
            {
                green.SetActive(false);
                orange.SetActive(false);
                red.SetActive(true);
            }

            int milliseconds = (int)(timer * 1000) % 100;
            int seconds = (int)(timer % 60);
            int minutes = (int)(timer / 60) % 60;

            string timerString;

            if (seconds >= 10)
            {
                timerString = string.Format("{0:0}:{1:00}", minutes, seconds);
            }
            else
            {
                timerString = string.Format("{0:0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            }

            //uiText.text = timer.ToString("F");
            uiText.text = timerString;

        }
        else if (timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uiText.text = "0.00";
            timer = 0.0f;
            uiText.enabled = false;
            if (isServer)
            {
                SceneManager.LoadScene("Win Warden");
            }
            else
            {
                SceneManager.LoadScene("Lose Prisoner");
            }
        }
    }

    // Reset timer
    [Command]
    void CmdResetTimer()
    {
        RpcResetTimer();
    }
    [ClientRpc]
    void RpcResetTimer()
    {
        timer = mainTimer;
        canCount = true;
        doOnce = false;
    }
    #endregion

    // Used in player script and turns off UI
    public void GameOver()
    {
        CmdGameOver();
    }

    [Command]
    void CmdGameOver()
    {
        RpcGameOver();
    }
    [ClientRpc]
    void RpcGameOver()
    {
        if (GameManager.GetGameOver())
        {
            canCount = false;
            doOnce = true;
            uiText.text = "0.00";
            timer = 0.0f;
            uiText.enabled = false;
            if (isServer)
            {
                LeaveRoom();
                SceneManager.LoadScene("Lose Warden");
            }
            else
            {
                LeaveRoom();
                SceneManager.LoadScene("Win Prisoner");
            }
        }
    }
    #endregion

    void LeaveRoom()
    {
        Debug.Log("Leave Room");
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
