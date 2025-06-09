using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float x0;
    float y0;

    public float time;
    public float x;
    public float y;
    public float angle = 45; //goc nem, tinh theo do

    float throwSpeed = 10; //van toc ban dau

    const float G = 9.8f;

    bool isOnGround = false;

    // Start is called before the first frame update
    void Start()
    {
        x0 = transform.position.x;
        y0 = transform.position.y;
        time = 0;
        angle *= Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnGround)
        {
            time += Time.deltaTime;

            x = throwSpeed * Mathf.Cos(angle) * time + x0;
            y = throwSpeed * Mathf.Sin(angle) * time - 0.5f * G * time * time + y0;

            this.transform.position = new Vector3(x, y, transform.position.z);
        }

        if (y < -10)
        {
            isOnGround = true;
        }
    }
}
