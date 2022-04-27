using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 15.0f;
    public Transform bullet;
    public Transform perryBullet;
    public int fireRateMS = 1000;
    public bool canShoot = true;
    // public int sideDamageMS;
    // public bool sideDamangeEnabling = true;
    public AudioSource bulletShot;
    public bool amIRight;

    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode shootKey;
    public KeyCode shootPerryKey;
    public KeyCode perrySchivaKey;

    // Statistiche per il calcolo del punteggio
    public int num_spari;
    public int num_kill;
    public int num_perry;
    public int num_hit;

    // Perry
    public int perryFireRateMS;
    public bool canShootPerry;
    public int perrySchivaDurationMS;
    public bool canSchivarePerry;
    public int perrySchivaDeltaY;

    void Start () {
        num_kill = num_perry = num_hit = 0;
        num_spari = 1;
        canShootPerry = canSchivarePerry = canShoot = true;
    }
    
    void Update () {
        // Movimento
        if (Input.GetKey (leftKey))
            transform.position += new Vector3 (-1 * Time.deltaTime * speed, 0, 0);
        if (Input.GetKey (rightKey))
            transform.position += new Vector3 (Time.deltaTime * speed, 0, 0);
            
        if (canSchivarePerry && Input.GetKeyDown (perrySchivaKey)) {
            canSchivarePerry = false;
            transform.position -= Vector3.forward * perrySchivaDeltaY;
            perrySchivaReset ();
        }

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
        if (canShoot && Input.GetKey (shootKey)) {
            Vector3 pos = transform.position;
            pos.z += 3;
            Bullet bulletInstance = Instantiate (bullet, pos, Quaternion.identity).gameObject.GetComponent <Bullet> ();
            bulletInstance.shooter = gameObject;
            canShoot = false;
            bulletShot.Play ();
            GameManager.instance.modifyScore ((amIRight ? 1 : 0), ScoreBuffs.PLAYER_SHOOT);
            setTimedShootEnable ();
            num_spari ++;
        }
        if (canShootPerry && Input.GetKey (shootPerryKey)) {
            Vector3 pos = transform.position;
            pos.x += (amIRight ? -4 : 4);
            PerryBullet bulletInstance = Instantiate (perryBullet, pos, Quaternion.identity).gameObject.GetComponent <PerryBullet> ();
            bulletInstance.shooter = this;
            bulletInstance.direction = (amIRight ? 1 : -1);
            canShootPerry = false;
            bulletShot.Play ();
            setTimedPerryShootEnable ();
        }
    }

    // async void setTimedSideDamageEnabling () {
    //     await Task.Delay (sideDamageMS);
    //     // Riabilita la possibilità di acquisire danno quando invade il lato dell'altro giocatore
    //     sideDamangeEnabling = true;
    // }
    async void setTimedShootEnable () {
        await Task.Delay (fireRateMS);
        // Riabilita la possibilità di sparare
        canShoot = true;
    }
    async void setTimedPerryShootEnable () {
        await Task.Delay (perryFireRateMS);
        // Riabilita la possibilità di sparare
        canShootPerry = true;
    }
    async void perrySchivaReset () {
        await Task.Delay (perrySchivaDurationMS);
        // Riabilita la possibilità di schivare il perry
        transform.position += Vector3.forward * perrySchivaDeltaY;
        canSchivarePerry = true;
    }

    public void enemyKilled () { num_kill ++; }
    public void perryExecuted () { num_perry ++; }
    public void hitted () { num_hit ++; }
}
