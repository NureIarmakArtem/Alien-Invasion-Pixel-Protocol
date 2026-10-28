using UnityEngine;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    public AudioClip shootSound;
    private AudioSource audioSource;
    // Зміна швидкості переміщення героя
    public float playerSpeed = 2.0f;
    // Поточна швидкість переміщення
    private float currentSpeed = 0.0f;
    // Створення змінних для кнопок
    public List<KeyCode> upButton;
    public List<KeyCode> downButton;
    public List<KeyCode> leftButton;
    public List<KeyCode> rightButton;

    // Збереження останнього переміщення
    private SpriteRenderer spriteRenderer;
    private Vector3 lastMovement = new Vector3();

    // --- Змінні для стрільби лазером  ---
    // Префаб лазера
    public Transform laser;
    // Як далеко від центру штурмовика по горизонталі з'являється лазер (з дула гвинтівки)
    public float laserDistance = 0.5f;
    // Зміщення точки вильоту по вертикалі (висота зброї)
    public float laserOffsetY = 0.0f;
    // Затримка між пострілами (кулдаун)
    public float timeBetweenFires = 0.3f;
    // Лічильник затримки між пострілами
    private float timeTilNextFire = 0.0f;
    // Список кнопок, які використовуються для пострілу
    public List<KeyCode> shootButton;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Фіксуємо поворот щоб солдат не ложився набік
        transform.rotation = Quaternion.identity;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Поворот героя до мишки
        LookAtCursor();
        // Переміщення героя
        Movement();

        // Перевірка натискання клавіш стрільби
        foreach (KeyCode element in shootButton)
        {
            // Якщо натиснута клавіша пострілу і кулдаун минув
            if (Input.GetKey(element) && timeTilNextFire <= 0)
            {
                timeTilNextFire = timeBetweenFires;
                ShootLaser();
                break;
            }
        }

        // Зменшуємо таймер затримки пострілу
        timeTilNextFire -= Time.deltaTime;
    }

    // Поворот героя до мишки
    void LookAtCursor()
    {
        //Показуємо гравцеві, де мишка
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Якщо курсор лівіше персонажа — розвертаємо спрайт вліво, інакше — вправо
        if (mouseWorldPos.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    // Рух персонажа
    void Movement()
    {
        Vector3 movement = new Vector3();

        // Перевірка натискання клавіш
        movement += MoveIfPressed(upButton, Vector3.up);
        movement += MoveIfPressed(downButton, Vector3.down);
        movement += MoveIfPressed(leftButton, Vector3.left);
        movement += MoveIfPressed(rightButton, Vector3.right);

        // Якщо натиснуто кілька кнопок, обробляємо це
        movement.Normalize();

        // Перевірка натискання кнопки
        if (movement.magnitude > 0)
        {
            currentSpeed = playerSpeed;
            transform.Translate(movement * Time.deltaTime * playerSpeed, Space.World);
            lastMovement = movement;
        }
        else
        {
            // Уповільнення згодом
            transform.Translate(lastMovement * Time.deltaTime * currentSpeed, Space.World);
            currentSpeed *= 0.9f;
        }
    }

    // Створення лазера в напрямку погляду
    void ShootLaser()
    {
        // Перевіряємо, куди дивиться штурмовик
        bool lookingLeft = spriteRenderer.flipX;

        // Обчислюємо позицію появи лазера: якщо дивиться ліворуч — вилітає лівіше, праворуч — правіше
        float posX = transform.position.x + (lookingLeft ? -laserDistance : laserDistance);
        float posY = transform.position.y + laserOffsetY;

        // Кут польоту: праворуч (0 градусів) або ліворуч (180 градусів)
        Quaternion spawnRotation = lookingLeft ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;

        // Створюємо об'єкт лазер на цій позиції
        Instantiate(laser, new Vector3(posX, posY, 0), spawnRotation);

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    // Повертає рух, якщо натиснута кнопка
    Vector3 MoveIfPressed(List<KeyCode> keyList, Vector3 direction)
    {
        foreach (KeyCode key in keyList)
        {
            if (Input.GetKey(key))
            {
                return direction;
            }
        }
        // Якщо кнопки не натиснуті, то не рухаємося
        return Vector3.zero;
    }
}