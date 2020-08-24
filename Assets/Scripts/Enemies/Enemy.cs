using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public int memoryUsage = 128;
    public float defaultMovementSpeed = 4f;
    public float HijackTime = 5f;
    public float MovementSpeed { get; private set; }

    public int maxHealth;
    [SerializeField]
    private int health;
    [SerializeField]
    private GameObject deathFXPrefab;

    private bool isDead;
    public EnemyType _type = EnemyType.Normal;

    public float HealRange = 5f;
    public int HealAmount = 20;
    public float HealCooldown = 5f;

    public enum EnemyType 
    {
        Normal,
        Healer
    }

    private void Start()
    {
        MovementSpeed = defaultMovementSpeed;
        health = maxHealth;
        if (this._type == EnemyType.Healer) 
        {
            StartCoroutine(HealCoroutine());
        }
    }

    public void HijackTower(Tower t) 
    {
        t.Stall(HijackTime);
        Die();
    }

    IEnumerator HealCoroutine() 
    {
        yield return new WaitForSeconds(HealCooldown);
        HealEnemysAround();
    }

    void HealEnemysAround() 
    {
        foreach (Transform t in WaveSpawner.Instance.GetEnemiesTransform())
        {
            if (Vector3.Distance(this.transform.position, t.position) <= HealRange)
            {
                t.GetComponent<Enemy>().Heal(HealAmount);
            }
        }
    }

    private void OnDestroy()
    {
        this.StopAllCoroutines();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        AnimateTakeDamage();
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }
    public void AnimateTakeDamage()
    {
        Sequence seq = DOTween.Sequence();
        var sprite = GetComponent<SpriteRenderer>();
        seq.Append( transform.DOScale(new Vector3(1.2f, 0.8f, 1.0f), 0.1f) );
        seq.Insert( 0.0f, sprite.DOColor(new Color(0.8f, 0.05f, 0.05f), 0.10f) );

        seq.Append( transform.DOScale(new Vector3(0.9f, 1.1f, 1.0f), 0.1f) );
        seq.Insert( 0.2f, sprite.DOColor(Color.white, 0.10f) );

        seq.Append( transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.1f) );
    }

    public void Heal(int amount)
    {
        if(health + amount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
    }

    public void Slow(float percentage,float time)
    {
        MovementSpeed = defaultMovementSpeed * (1f - percentage);
        StartCoroutine(WaitResetSpeed(time));
    }

    IEnumerator WaitResetSpeed(float time) 
    {
        yield return new WaitForSeconds(time);
        ResetSpeed();
    }

    public void IncreaseSpeed(float percentage)
    {
        MovementSpeed = defaultMovementSpeed * (1f + percentage);
    }

    public void ResetSpeed()
    {
        MovementSpeed = defaultMovementSpeed;
    }

    private void Die()
    {
        isDead = true;
        //Particle Effects
        //DestroyParticles
        WaveSpawner.Instance.RemoveEnemy(this);
        var fx = Instantiate(deathFXPrefab);
        fx.transform.position = transform.position;
        AudioManager.Instance.PlayAudio("die_virus");
        Destroy(gameObject);
    }
}
