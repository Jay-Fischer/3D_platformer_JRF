using System.Collections;
using UnityEngine;

public class EnemyZoneScript : MonoBehaviour
{
    public GameObject[] enemies;
    public int enemyCount;
    public GameObject zone;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
            {
                enemyCount++;
            }
        }
        MusicManager.Instance.PlayMusic("Test1", 0);
        InvokeRepeating("CheckArray", 0f, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            MusicManager.Instance.PlayMusic("Test2", 0.5f);
            foreach (GameObject obj in enemies)
            {
                if (obj != null)
                {
                    EnemyBehavior behaviorScript = obj.GetComponent<EnemyBehavior>();
                    if (behaviorScript != null) behaviorScript.SwitchToPursue();
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            MusicManager.Instance.PlayMusic("Test1", 0.5f);
            foreach (GameObject obj in enemies)
            {
                if (obj != null)
                {
                    EnemyBehavior behaviorScript = obj.GetComponent<EnemyBehavior>();
                    if (behaviorScript != null) behaviorScript.SwitchToReturn();
                }

            }
        }
    }

    public void CheckArray(){
        int count = 0;
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
            {
                count++;
            }
            Debug.Log("Enemies Counted" + count.ToString());
        }
        enemyCount = count;
        if (enemyCount == 0) { MusicManager.Instance.PlayMusic("Test1", 0.5f); Destroy(zone); }
    }
}
