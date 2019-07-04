using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class ClassSelector : MonoBehaviour {

    public GameObject[] ClassArray; // set in inspector

    
    private IPClass selectedClass;

    public IPClass SelectedClass {
        get { return selectedClass; }
        set { selectedClass = value; }
    }

	// Use this for initialization
	void Start () {

        SetClass();           
    }





    void SetClass() {
        GameObject temp = (GameObject) Instantiate(ClassArray[0], transform.position, Quaternion.identity);
        temp.transform.parent = transform;
        selectedClass = temp.GetComponent<IPClass>();
    }



}
