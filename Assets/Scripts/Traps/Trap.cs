using UnityEngine;

public class Trap : MonoBehaviour
{
   public bool isActivated;
    // Permet d'appeler l'activation du trap depuis le ControlStation peu importe le trap..
    public virtual void ActivateTrap(){}
    
}