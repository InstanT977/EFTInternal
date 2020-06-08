using EFT.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeFromGod
{
    class CExtract : MonoBehaviour
    {
        public IEnumerable<ExfiltrationPoint> _extractPoints;

        private int distanceToObject;
        private Vector3 exfilContainerBoundingVector;
        void Update()
        {
            if (CGameWorld.IsPlaying && Settings.CExtract.bExtract)
            {
                _extractPoints = CGameWorld.activeGameworld.ExfiltrationController.ExfiltrationPoints;
            }
        }
        void OnGUI()
        {
            if (Event.current.type != EventType.Repaint)
                return;
            if (CGameWorld.IsPlaying && Settings.CExtract.bExtract)
            {
                try
                {
                    foreach (var point in _extractPoints)
                    {
                        if (point != null)
                        {
                            distanceToObject = (int)Math.Floor(Vector3.Distance(point.transform.position, CGameWorld.MainCamera.transform.position));
                            exfilContainerBoundingVector = CGameWorld.MainCamera.WorldToScreenPoint(point.transform.position);

                            if (exfilContainerBoundingVector.z > 0.01)
                            {
                                if(distanceToObject <= Settings.CExtract.ExtractDistance)
                                {
                                    if (point.HasRequirements)
                                        GUI.color = Print.colorCode(Print.colorCodeEnum.Orange);
                                    else
                                        GUI.color = Color.green;
                                    GUI.Label(new Rect(exfilContainerBoundingVector.x - 50f, (float)Screen.height - exfilContainerBoundingVector.y, 100f, 50f), $"{point.Settings.Name.Localized()} - {distanceToObject}m");
                                    
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }
    }
}
