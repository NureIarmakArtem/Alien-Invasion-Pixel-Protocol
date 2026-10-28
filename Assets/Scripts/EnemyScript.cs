using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Змінна для звуку під час попадання кулі
    public AudioClip hitSound;

    private AudioSource audioSource;

    // Скільки разів потрібно потрапити у ворога, щоб знищити його
    public int health = 2;

    // Анімація вибуху при знищенні об'єкта
    public GameObject explosion;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Спрацьовує при зіткненні, якщо на колайдері увімкнено Is Trigger
    void OnTriggerEnter2D(Collider2D theCollision)
    {
        CheckHit(theCollision.gameObject);
    }

    // Спрацьовує при звичайному фізичному зіткненні
    void OnCollisionEnter2D(Collision2D theCollision)
    {
        CheckHit(theCollision.gameObject);
    }

    // Загальна логіка обробки влучання кулі
    void CheckHit(GameObject hitObject)
    {
        // Шукаємо скрипт LaserScript на об'єкті, що влучив
        LaserScript laser = hitObject.GetComponent<LaserScript>();

        // Якщо це куля (лазер)
        if (laser != null)
        {
            // Зменшуємо здоров'я ворога на величину пошкодження кулі
            health -= laser.damage;

            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // Знищуємо саму кулю при влучанні
            Destroy(hitObject);

            // Якщо здоров'я закінчилося — знищуємо ворога
            if (health <= 0)
            {
                // Створюємо анімацію вибуху на місці ворога
                if (explosion != null)
                {
                    GameObject exploder = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(exploder, 2.0f);
                }

                // Знаходимо контролер гри, сповіщаємо про знищення ворога та додаємо очки
                GameController controller = FindFirstObjectByType<GameController>();
                if (controller != null)
                {
                    controller.KilledEnemy();
                    controller.IncreaseScore(10);

                    // Знищуємо самого ворога
                    Destroy(this.gameObject);
                }
            }
        }
    }
}