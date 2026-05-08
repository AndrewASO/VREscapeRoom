using UnityEngine;

public class SeatFlip : MonoBehaviour {

    public Animator seatAnimator; 

    public void flipBool() {
        bool seatBool = seatAnimator.GetBool("Flip");
        seatAnimator.SetBool("Flip", !seatBool);
    }
}
