using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    public Animator ANimator;
    // Start is called before the first frame update
    void Start()
    {
        ANimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBool(string name, bool booleen)
    {
        ANimator.SetBool(name, booleen);

    }
}
