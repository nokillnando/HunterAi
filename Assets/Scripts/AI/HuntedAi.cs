using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntedAi : BaseAi
{
    public float DistanceFromHunter;
    // Start is called before the first frame update
    void Start()
    {
        DistanceFromHunter = 0;
        Idle();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.1)
        {
            RandomPositionToGo(this.gameObject);
            time = 0;
        }
          /*  if (Input.GetKeyDown(KeyCode.Space))
            {
                RandomPositionToGo(this.gameObject);
            }*/
        }
}
