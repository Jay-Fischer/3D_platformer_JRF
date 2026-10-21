using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealthScript : MonoBehaviour
{



    [SerializeField] Animator _anim;
    public PlayerControllerScript pc;
    public GameManager gm;
    [SerializeField] public int health;
    public bool isHurt;
    public float hurtTimer = 1f;

    public TMP_Text healthtext;
    private void Awake()
    {
        pc = GetComponentInParent<PlayerControllerScript>();
        healthtext.text = "Health: " + health.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyHurtTrigger" && !isHurt)
        {
            StartCoroutine(onHurt());
        }
    }

    IEnumerator onHurt()
    {
        isHurt = true;
        pc.hurt = true;
        health--;
        healthtext.text = "Health: " + health.ToString();
        _anim.SetTrigger("Hurt");
        SoundManager.Instance.PlaySound2D("Hurt");
        yield return new WaitForSeconds(0.25f);
        HitStopManager.Instance.DoHitStop(0.15f, 0);
        if (health > 0)
        {
            pc.hurt = false;
            yield return new WaitForSeconds(hurtTimer);
            isHurt = false;
        }
        else
            StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        _anim.SetBool("Dead", true);
        yield return new WaitForSeconds(3.0f);
        healthtext.enabled = false;
        gm.Gameover();
    }
}
