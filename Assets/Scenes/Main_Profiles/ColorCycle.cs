using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    public int colorCycle;
    public int speed;
    public Color cycleColor;

    Material cycleMat;

    // Start is called before the first frame update
    void Start()
    {
        cycleMat = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {

        cycleMat.SetColor("_EmissionColor", cycleColor);
    }

    public int Cycle(int cycle)
    {
        cycle += speed;
        if(cycle > 255)
        {
            cycle = 0;
        }
        return cycle;

    }
}
