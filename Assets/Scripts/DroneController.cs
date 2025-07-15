using UnityEngine;

public class drone_costum : MonoBehaviour
{
    public float speed = 5f;
    public float liftForce = 5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0.1f; // supaya drone melayang pelan

        if (Input.GetKey(KeyCode.Space)) moveY = 1f;
        else if (Input.GetKey(KeyCode.LeftControl)) moveY = -1f;

        Vector3 movement = new Vector3(moveX, moveY, moveZ);
        transform.Translate(movement * speed * Time.deltaTime);
    }

}
