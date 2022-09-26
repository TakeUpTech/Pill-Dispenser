using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using TMPro;
using UnityEngine.UI;
using System;

public class ArduinoScript : MonoBehaviour
{
    SerialPort serial1;
    private int ledState, alarmState, devLedState, date, take = 0;
    private float temp, hum;
    public TextMeshProUGUI ledStateTMP, devLedStateTMP, alarmStateTMP, remainedTMP, tempTMP, humTMP, dataStateTMP; 
    private int[] horaires;
    private int[] planning;
    private string[] ports = SerialPort.GetPortNames();
    public GameObject takeJauge;

    // Start is called before the first frame update
    void Start()
    {
      serial1 = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);

      ledState = 1;
      devLedState = 0;
      alarmState = 1;

      planning = new int [] {1, 1, 1};
      horaires = new int[] {10, 30, 50}; // 9, 12, 19 pour les heures
    }

    public void LedState()
    {
      ledState = ledState == 1 ? 0 : 1;

      if(ledState == 1)
      {
        ledStateTMP.text = "Alerte Lumineuse On";
      } else {
        ledStateTMP.text = "Alerte Lumineuse Off";
      }
    }
    
    public void AlarmState()
    {
      alarmState = alarmState == 1 ? 0 : 1;

      if(alarmState == 1)
      {
        alarmStateTMP.text = "Alerte Sonore On";
      } else {
        alarmStateTMP.text = "Alerte Sonore Off";
      }
    }

    public void ReadHeureMatin(TMP_InputField input)
    {
      ReadHeure(input, 0);
    }

    public void ReadHeureMidi(TMP_InputField input)
    {
      ReadHeure(input, 1);
    }

    public void ReadHeureSoir(TMP_InputField input)
    {
      ReadHeure(input, 2);
    }

    private void ReadHeure(TMP_InputField input, int index)
    {
      int value = -1;
      
      if(!input.text.Equals("X"))
      {
        planning[index] = 1;
        try
        {
          value = int.Parse(input.text);
        }
        catch (System.Exception) { throw; }

        if(value >= 0 && value <= 60) //60 secondes (pour les tests) sinon inscrire 23 heures
        {
          horaires[index] = value;
        }
      } else {
        planning[index] = 0;
      }
    }

    public void DevLedState()
    {
      devLedState = devLedState == 1 ? 0 : 1;

      if(devLedState == 1)
      {
        devLedStateTMP.text = "Option Développeur On";
      } else {
        devLedStateTMP.text = "Option Développeur Off";
      }
    }

    public void SendData()
    {
      date = (int) (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Local)).TotalSeconds;
      
      string data = "dat:" + date + ";" 
                  + "led:" + ledState + ";" 
                  + "ala:" + alarmState + ";"                 
                  + "hMa:" + horaires[0] + ";"
                  + "hMi:" + horaires[1] + ";"
                  + "hSo:" + horaires[2] + ";"
                  + "dev:" + devLedState + ";"
                  + "pla:" + planning[0] + planning[1] + planning[2];

      for (int i = 0; i < ports.Length; i++)
      {
        serial1.PortName = ports[i];
        try {
          serial1.Open();
          serial1.Write(data);
          //Debug.Log("All data have been send : " + data);
          dataStateTMP.text = "Succès";
          StartCoroutine(SendConfirmation());
        } catch {
          //Debug.Log("No port connected.");
        }
        serial1.Close();
      }
    }

    private IEnumerator SendConfirmation()
    {
      yield return new WaitForSeconds(2f);
      dataStateTMP.text = "Envoyer Données";
    }

    public void ReceveData()
    {
      serial1.ReadTimeout = 5000;
      for (int i = 0; i < ports.Length; i++)
      {
        serial1.PortName = ports[i];

        try {
          serial1.Open();
          string[] data = serial1.ReadLine().Split(';');
          take = int.Parse(data[0]);
          temp = float.Parse(data[1].Split('.')[0]) + float.Parse(data[1].Split('.')[1])/100f;
          hum = float.Parse(data[2].Split('.')[0]) + float.Parse(data[2].Split('.')[1])/100f;
        } catch {
          //Debug.Log("Not working.");
        }
        serial1.Close();
      }

      UiUpdate();
    }

    private void UiUpdate()
    {
      remainedTMP.text = "Prises\nRestantes\n" + "(" + (21 - take) + ")";
      tempTMP.text = "Température :\n" + temp + "°C";
      humTMP.text = "Humidité :\n" + hum + " %";

      if((21-take)/21 >= 0)
      {
        takeJauge.GetComponent<RectTransform>().localScale = new Vector3(1, (float) (21-take)/21, 1);
      } else {
        takeJauge.GetComponent<RectTransform>().localScale = new Vector3(1, 0, 1);
      }

      if(temp > 25)
      {
        tempTMP.color = Color.red;
      } else {
        tempTMP.color = new Color32(231, 231, 231, 255);
      }

      if(hum > 60)
      {
        humTMP.color = Color.red;
      } else {
        humTMP.color = new Color32(231, 231, 231, 255);
      }      
    }
}