﻿using UnityEngine;
using System.Collections;

public class KeyboardManager : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus) && Time.timeScale > 1)
            Time.timeScale--;
        else if (Input.GetKeyDown(KeyCode.KeypadPlus) && Time.timeScale < Settings.World_MaxGameSpeed)
            Time.timeScale++;
    }
}