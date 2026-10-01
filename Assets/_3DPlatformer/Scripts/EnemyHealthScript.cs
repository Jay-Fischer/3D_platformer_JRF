using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public int health;
    [SerializeField] Rigidbody rb;

    public float knockBackForce = -10f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HitSphere")
        {
            Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 lookPosition = new Vector3(_player.position.x, _player.position.y, _player.position.z);
            transform.LookAt(lookPosition);
            HitStopManager.Instance.DoHitStop(0.15f, 0);
            rb.AddForce(transform.forward * knockBackForce, ForceMode.Impulse);
        }
    }
}
