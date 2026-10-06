using System.Collections;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [Header("Set up Materials")]
    [SerializeField] private SkinnedMeshRenderer _skin;
    public Material _baseMat;
    public Material _hurtMat;

    [Header("Set up Effects")]
    public GameObject impactEffect;
    public GameObject deathSmokeEffect;
    public Transform impactSpawnPoint;


    public int enemyHealth;
    [SerializeField] Rigidbody rb;

    [SerializeField] private bool isHurt;

    Animator anim;

    public float knockBackForce = -10f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _skin = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponentInChildren<Animator>();
        _skin.material = _baseMat;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HitSphere" && !isHurt)
        {
            Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 lookPosition = new Vector3(_player.position.x, _player.position.y, _player.position.z);
            transform.LookAt(lookPosition);
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        isHurt = true;
        enemyHealth--;
        anim.SetTrigger("Hurt");
        Instantiate(impactEffect, impactSpawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.025f);
        HitStopManager.Instance.DoHitStop(0.15f, 0);
        rb.AddForce(transform.forward * knockBackForce, ForceMode.Impulse);
        _skin.material = _hurtMat;

        if(enemyHealth > 0)
        {
            yield return new WaitForSeconds(0.1f);
            _skin.material = _baseMat;
            isHurt = false;
        }
        else 
            StartCoroutine(EnemyDeath());
        
    }

    IEnumerator EnemyDeath()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1.5f);
        Instantiate(deathSmokeEffect, impactSpawnPoint.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
