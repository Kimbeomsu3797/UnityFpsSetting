using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    public int attackPower = 10;
    public float explosionRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�.
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8); // ���̾��� ������ �ٲٴ°� 1<<8�̶�µ� �� �𸣰���.
        for(int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }
        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;
        
        Destroy(gameObject);
    }
}
