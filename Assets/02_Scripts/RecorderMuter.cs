namespace Photon.Voice.Fusion
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Unity;

    public class RecorderMuter : MonoBehaviour
    {
        [SerializeField] Recorder rec;
        [SerializeField] HardwareRig hr;
        [SerializeField] ScreensAndHudManager shm;
        [SerializeField]
        // Start is called before the first frame update
        void Start()
        {
            rec = FindObjectOfType<Recorder>();
            hr = FindObjectOfType<HardwareRig>();
            shm = FindObjectOfType<ScreensAndHudManager>();

        }

        // Update is called once per frame
        void Update()
        {
            if (!shm.isMuted)
            {
                if (shm.wayToTalk == 0)
                {
                    MuteOrNot(hr.localIsTalking);
                    rec.VoiceDetection = false;
                }
                else
                rec.TransmitEnabled = true;
                rec.VoiceDetection = true;
            }
            else
            {
                rec.TransmitEnabled = false;
                rec.VoiceDetection = false;
            }


        }

       public void MuteOrNot(bool Istalking)
        {
            if (Istalking)
            {
                rec.TransmitEnabled = true;
            }
            else
            {
                rec.TransmitEnabled = false;
            }
        }
    }
}
