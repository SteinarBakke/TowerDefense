using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;

    public float speed = 70f;

    public int damage = 5;

    public GameObject impactEffect;


    private int Index = 0;
    private float Duration = 0;
    private float Amount = 0;

    //for AOE bullets
    public float explosionRadius = 0f;
    

    //Not sure if I want this
    public void Seek(Transform _target)
    {
        target = _target;
    }

    public void ApplyDebuff(int index, float duration, float amount)
    {
        Index = index;
        Duration = duration;
        Amount = amount;
    }


    void Update ()
    {

        if (target == null)
        {
            Destroy(gameObject);
            return; //should always return after destroy, sometimes takes a bit to process
        }




        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        //moving the missile
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        //making forward rotation to target
        transform.LookAt(target);
    }

    void HitTarget()
    {
        GameObject EffectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(EffectInstance, 2f);

        if (explosionRadius > 0f) //AOE dmg
            Explode();
        else //Single Target
        {
            Damage(target);
            Destroy(gameObject);
            return;
        }
    }

    void Damage(Transform enemy)
    {
        Enemy _enemy = enemy.GetComponent<Enemy>();
        if (_enemy != null)
        {
            if (Index != 0)
                _enemy.TakeDebuff(Index, Duration, Amount);

            _enemy.TakeDamage(damage);
        }
    }

    void Explode()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in hitObjects)
        {
            if (collider.tag == "Enemy")
                Damage(collider.transform);
        }
        Destroy(gameObject);
        return;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
