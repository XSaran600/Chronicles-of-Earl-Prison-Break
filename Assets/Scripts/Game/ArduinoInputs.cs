using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoInputs : MonoBehaviour
{

    // The serial port
    SerialPort sp = new SerialPort("COM4", 9600);

    [SerializeField]
    int HIDInput;

    // Use this for initialization
    void Start()
    {
        // Setting up the serial port
        sp.Open();
        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
    }

    void InputManager()
    {
        // See if its open
        if (sp.IsOpen)
        {
            // Do a try and catch
            try
            {
                // Print the serial port
                //print(sp.ReadByte());
                HIDInput = sp.ReadByte();
            }
            catch (System.Exception)
            {
            }
        }
    }

    public int GetHIDInput()
    {
        return HIDInput;
    }
}
