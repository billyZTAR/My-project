using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int powerupID; //0 = TripleSHot, 1 = Speed, 2 = Shields;
    [SerializeField]
    private AudioClip _clip;
   
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y == -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleSHotActive();
                   
                        break;
                    case 1:
                        player.SpeedBoostActive();
                
                        break;
                    case 2:
                        player.ShieldActive();
                
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
