using UnityEngine;
using System.Collections;

public class MoveTowardsPlayer : MonoBehaviour
{
    // Змінна для координат об'єкта гравця
    private Transform player;

    // Швидкість руху ворога
    public float speed = 1.5f;

    // Компонент відображення спрайта для розвороту ворога
    private SpriteRenderer spriteRenderer;

    // Викликається при старті гри
    void Start()
    {
        // Отримуємо компонент відображення спрайта
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Знаходимо об'єкт гравця за його точною назвою в ієрархії
        GameObject playerObj = GameObject.Find("playerSolider");

        // Якщо об'єкт знайдено — зберігаємо його Transform
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            // Запасний варіант: шукаємо за компонентом PlayerScript, якщо назва зміниться
            PlayerScript foundPlayer = FindFirstObjectByType<PlayerScript>();
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }
    }

    // Викликається один раз на кожен кадр
    void Update()
    {
        // Якщо гравець існує на сцені
        if (player != null)
        {
            // Обчислюємо різницю між ворожим об'єктом і гравцем
            Vector3 delta = player.position - transform.position;

            // Розворот спрайта ворога обличчям до гравця
            if (spriteRenderer != null)
            {
                if (delta.x < 0)
                {
                    spriteRenderer.flipX = true; // Дивиться ліворуч (залежно від вихідного малюнка)
                }
                else
                {
                    spriteRenderer.flipX = false;  // Дивиться праворуч
                }
            }

            // Нормалізуємо вектор напрямку
            delta.Normalize();

            // Розраховуємо переміщення за кадр
            float moveSpeed = speed * Time.deltaTime;

            // Переміщуємо ворога в напрямку гравця
            transform.position = transform.position + (delta * moveSpeed);
        }
    }
}