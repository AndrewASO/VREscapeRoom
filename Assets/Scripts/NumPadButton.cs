using UnityEngine;

public class NumPadButton : MonoBehaviour {
    
    //Change this in accordance to the corresponding # 
    [SerializeField] private string digit = "1";
    private NumPad numPad;

    private void Awake() {
        numPad = GetComponentInParent<NumPad>();
    }

    public void Press() {
        //Null Check
        if(numPad != null) {
            numPad.PressDigit(digit);
        }
    }
}
