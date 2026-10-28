using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour
{
    // Як довго існує лазер 
    public float lifetime = 2.0f;
    // Як швидко рухається лазер
    public float speed = 10.0f;
    // Як багато завдає шкоди лазер при зіткненні з ворогами
    public int damage = 1;

    // Викликається при створенні об'єкта
    void Start()
    {
        // Знищення лазера по закінченню таймера
        Destroy(gameObject, lifetime);
    }

    // Викликається кожен кадр
    void Update()
    {
        // Політ у напрямку свого погляду (праворуч або ліворуч, залежно від повороту)
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
}