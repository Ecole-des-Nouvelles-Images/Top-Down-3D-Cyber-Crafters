using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
   public bool isActivated;
   public List<ControlStation> controlStations = new List<ControlStation>();
   
    // Permet d'appeler l'activation du trap depuis le ControlStation peu importe le trap..
    public virtual void ActivateTrap(){}
    
}