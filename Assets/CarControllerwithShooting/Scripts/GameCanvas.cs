using System;
using UnityEngine;
using UnityEngine.UI;

namespace CarControllerwithShooting
{
    public class GameCanvas : MonoBehaviour
    {
        public Button button_Missile;
        public Button button_HandBrake;
        public Button button_Machinegun;
        public GameObject joystick;
        public Button button_CameraChange;
        public int isFiringUpdate = 0;
        public Slider Slider_CurrentFuel;
        public Text Text_CurrentFuel;

        public Text Text_Ammo_Machinegun;
        public Text Text_Ammo_Missile;

        public static GameCanvas Instance;
        public GameObject GameUI;
        public GameObject RadarUI;
        public GameObject GasolineUI;
        public Text text_health;
        public Text text_speed;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            if (CarSystemManager.Instance.ShowRadar)
            {
                RadarUI.SetActive(true);
            }
            else
            {
                RadarUI.SetActive(false);
            }
            Update_Text_Health();
        }

        public void Update_Text_Health()
        {
            text_health.text = "HEALTH: " + CarController.Instance.Health.ToString();
        }

        public void Update_Text_Speed()
        {
            text_speed.text = Convert.ToInt32(CarController.Instance.speed).ToString();
        }

        public void Configure_For_Mobile()
        {
            joystick.gameObject.SetActive(true);
            GameCanvas.Instance.button_HandBrake.gameObject.SetActive(true);
        }

        public void Configure_For_PCConsole()
        {
            joystick.gameObject.SetActive(false);
            button_CameraChange.GetComponentInChildren<Text>().text = "Camera (C)";
        }

        public void Click_Button_CameraSwitch()
        {
            if (button_CameraChange.IsInteractable())
            {
                if (CarSystemManager.Instance.cameraFPS != null && CarSystemManager.Instance.cameraFPS.activeSelf)
                {
                    CarSystemManager.Instance.cameraFPS.SetActive(false);
                    CarSystemManager.Instance.cameraTPS.SetActive(true);
                }
                else if (CarSystemManager.Instance.cameraTPS != null)
                {
                    CarSystemManager.Instance.cameraFPS.SetActive(true);
                    CarSystemManager.Instance.cameraTPS.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.C) && CarSystemManager.Instance.controllerType == ControllerType.KeyboardMouse)
            {
                Click_Button_CameraSwitch();
            }
        }

        public void Click_Button_HandBrake_Down()
        {
            CarController.Instance.handBrake = 1;
        }

        public void Click_Button_HandBrake_Up()
        {
            CarController.Instance.handBrake = 0;
        }

        public void Hide_GameUI()
        {
            GameUI.SetActive(false);
        }

        public void Click_Button_MachineGun_Down()
        {
            isFiringUpdate = 1;
        }

        public void Click_Button_Guns_Up()
        {
            isFiringUpdate = 0;
        }

        public void Click_Button_Missle_Down()
        {
            isFiringUpdate = -1;
        }
    }
}