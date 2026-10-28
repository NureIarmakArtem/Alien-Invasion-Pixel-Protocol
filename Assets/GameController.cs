using UnityEngine;
using System.Collections;
using TMPro; // Підключаємо бібліотеку для роботи з текстом TextMeshPro

public class GameController : MonoBehaviour
{
    // Створення змінної «ворог»
    public Transform enemy;

    // Тимчасові проміжки між подіями, к-ть ворогів
    public float timeBeforeSpawning = 1.5f;
    public float timeBetweenEnemies = 0.25f;
    public float timeBeforeWaves = 2.0f;
    public int enemiesPerWave = 10;
    private int currentNumberOfEnemies = 0;

    // Змінні для виведення на екран
    private int score = 0;
    private int waveNumber = 0;

    // Посилання на текстові об'єкти інтерфейсу
    public TMP_Text scoreText;
    public TMP_Text waveText;

    void Start()
    {
        // Запускаємо корутину для спавну хвиль ворогів
        StartCoroutine(SpawnEnemies());
    }

    // Поява хвиль ворогів
    IEnumerator SpawnEnemies()
    {
        // Початкова затримка перед першою появою ворогів
        yield return new WaitForSeconds(timeBeforeSpawning);

        // Коли таймер закінчиться, починаємо виробляти ці дії
        while (true)
        {
            // Не створювати нових ворогів, поки не знищені старі
            if (currentNumberOfEnemies <= 0)
            {
                // Підрахунок і виведення номера хвилі
                waveNumber++;
                if (waveText != null)
                {
                    waveText.text = "Хвиля: " + waveNumber;
                }

                float randDirection;
                float randDistance;

                // Створити 10 ворогів у випадкових місцях за екраном
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    // Задаємо випадкові змінні для відстані та напрямку
                    randDistance = Random.Range(10, 25);
                    randDirection = Random.Range(0, 360);

                    // Використовуємо змінні для завдання координат появи ворога
                    float posX = this.transform.position.x + (Mathf.Cos(randDirection * Mathf.Deg2Rad) * randDistance);
                    float posY = this.transform.position.y + (Mathf.Sin(randDirection * Mathf.Deg2Rad) * randDistance);

                    // Створюємо ворога на заданих координатах
                    Instantiate(enemy, new Vector3(posX, posY, 0), Quaternion.identity);

                    currentNumberOfEnemies++;
                    yield return new WaitForSeconds(timeBetweenEnemies);
                }
            }

            // Очікування до наступної перевірки
            yield return new WaitForSeconds(timeBeforeWaves);
        }
    }

    // Збільшення рахунку
    public void IncreaseScore(int increase)
    {
        score += increase;
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + score;
        }
    }

    // Процедура зменшення кількості ворогів у змінній
    public void KilledEnemy()
    {
        currentNumberOfEnemies--;
    }
}