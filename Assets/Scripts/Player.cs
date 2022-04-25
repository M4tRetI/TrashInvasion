using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 15.0f;
    public Transform bullet;
    public int fireRateMS = 1000;
    public bool canShoot = true;
    public int sideDamageMS;
    public bool sideDamangeEnabling = true;
    public AudioSource bulletShot;
    public bool amIRight;

    void Start () {}
    
    void Update () {
        // Movimento
        transform.position += new Vector3 (Input.GetAxis ("Horizontal") * Time.deltaTime * speed, 0, 0);

        // if (sideDamangeEnabling) {
        //     if (amIRight && transform.position.x < 0) {
        //         GameManager.instance.modifyScore (1, ScoreBuffs.PLAYER_IN_OPPONENT_SIDE);
        //         sideDamangeEnabling = false;
        //         setTimedSideDamageEnabling ();
        //     } else if (!amIRight && transform.position.x > 0) {
        //         GameManager.instance.modifyScore (0, ScoreBuffs.PLAYER_IN_OPPONENT_SIDE);
        //         sideDamangeEnabling = false;
        //         setTimedSideDamageEnabling ();
        //     }
        // }
        if (canShoot && Input.GetKey (KeyCode.Space)) {
            Vector3 pos = transform.position;
            pos.z += 3;
            Bullet bulletInstance = Instantiate (bullet, pos, Quaternion.identity).gameObject.GetComponent <Bullet> ();
            bulletInstance.shooter = gameObject;
            canShoot = false;
            bulletShot.Play ();
            GameManager.instance.modifyScore ((amIRight ? 1 : 0), ScoreBuffs.PLAYER_SHOOT);
            setTimedShootEnable ();
        }
    }

    async void setTimedSideDamageEnabling () {
        await Task.Delay (sideDamageMS);
        // Riabilita la possibilità di acquisire danno quando invade il lato dell'altro giocatore
        sideDamangeEnabling = true;
    }
    async void setTimedShootEnable () {
        await Task.Delay (fireRateMS);
        // Riabilita la possibilità di sparare
        canShoot = true;
    }
}
