using UnityEngine;

//I honestly should make this thing modular for the different animations as I've done 2 very simple & same codes for the opendoor & seatflip
//at this point and they're not much different
public class OpenDoor : MonoBehaviour {
    
    public Animator doorAnimator;

    public void OpenDoorBool() {
        doorAnimator.SetBool("Open", true);
    }
}
