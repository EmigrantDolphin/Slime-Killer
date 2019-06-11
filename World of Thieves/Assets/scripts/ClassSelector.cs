using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class ClassSelector : MonoBehaviour {

    public GameObject[] classArray; // set in inspector

    
    private IPClass selectedClass;

    public IPClass getSelectedClass {
        get { return selectedClass; }
        set { selectedClass = value; }
    }

	// Use this for initialization
	void Start () {
    
            setClass();           
    }





    void setClass() {
        GameObject temp = (GameObject) Instantiate(classArray[0], transform.position, Quaternion.identity);
        temp.transform.parent = transform;
        selectedClass = temp.GetComponent<IPClass>();
    }



}
