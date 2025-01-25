using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TankBehavior : MonoBehaviour
{
    // Enumération des joueurs
    public EPlayer playerID;
    #region Variables et paramètres

    [Header("Stats")]
    public int health = 100;
    public int maxHealth = 100;
    public int ammo = 10;
    public int mineAmmo = 3;

    [Header("Prefabs et positions")]
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Transform canonPos;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private Transform minePos;

    [Header("Mouvement")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float angularSpeed = 120f;

    [Header("Paramètres de tir")]
    [SerializeField] private float maxShootPower = 3000f;
    [SerializeField] private float chargeTimeToMax = 3f; // Temps pour atteindre la puissance maximale
    private float shootHoldTime = 0f;

    [Header("Sons du moteur")]
    [SerializeField] private AudioClip engineIdleSound;
    [SerializeField] private AudioClip engineDrivingSound;
    private AudioSource engineAudioSource;

    [Header("Effets et feedbacks")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip bombDropSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private ParticleSystem bombEffect;
    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private ParticleSystem deathEffect;

    // Événements et délégués
    public delegate void PowerUpChanged(string powerUpName);
    public event PowerUpChanged OnPowerUpChanged;
    public delegate void TankDeathEvent(EPlayer playerID);
    public event TankDeathEvent OnTankDeath;

    // État du tank
    private Rigidbody rb;
    private bool isMoving = false;
    public string ActivePowerUp { get; private set; } = "None";

    // New Input System
    private PlayerControls controls;
    private Vector2 moveInput;
    private float turnInput;
    private bool isCharging;

    #endregion

    #region Méthodes Unity

    private void OnEnable()
    {
        controls = new PlayerControls();

        // Assignation des actions
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Shoot.performed += ctx => StartCharge();
        controls.Player.Shoot.canceled += ctx => ShootWithPower();

        controls.Player.Drop.performed += ctx => DropBomb();

        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        engineAudioSource = GetComponent<AudioSource>();

        // Initialisation du son du moteur
        if (engineAudioSource != null && engineIdleSound != null)
        {
            engineAudioSource.clip = engineIdleSound;
            engineAudioSource.Play();
        }
    }

    private void Update()
    {
        UpdateEngineSound(rb.velocity.magnitude);

        if (isCharging)
        {
            Charge();
        }
    }

    private void FixedUpdate()
    {
        // Get input from the new Input System
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();

        // Use Y axis for Move and X axis for Turn
        Move(new Vector3(0, 0, moveInput.y));
        Turn(moveInput.x);
    }

    #endregion

    #region Mouvement et rotation

    public void Move(Vector3 direction)
    {
        float currentSpeed = GetCurrentSpeed();
        transform.Translate(direction * currentSpeed * Time.deltaTime);
    }

    private float GetCurrentSpeed()
    {
        if (isCharging)
        {
            float chargePercentage = Mathf.Clamp01(shootHoldTime / chargeTimeToMax);
            return Mathf.Lerp(baseSpeed, baseSpeed * 0.1f, chargePercentage); // Réduction de la vitesse à 10% au maximum
        }
        return baseSpeed;
    }

    public void Turn(float dir)
    {
        float rotationInput = dir * angularSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationInput, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    #endregion

    #region Gestion des tirs et bombes

    public void StartCharge()
    {
        shootHoldTime = 0f;
        isCharging = true;
    }

    public void Charge()
    {
        if (ammo > 0 && isCharging)
        {
            shootHoldTime += Time.deltaTime;
            shootHoldTime = Mathf.Clamp(shootHoldTime, 0, chargeTimeToMax);
        }
    }

    public void ShootWithPower()
    {
        if (ammo > 0 && isCharging)
        {
            // Calcule la puissance proportionnelle au temps de charge
            float shootPower = Mathf.Lerp(500f, maxShootPower, shootHoldTime / chargeTimeToMax);

            // Instanciation du projectile
            Projectile projectile = Instantiate(ammoPrefab, canonPos.position, canonPos.rotation).GetComponent<Projectile>();
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.AddRelativeForce(Vector3.forward * shootPower);
            projectile.firerer = this;

            ammo--;
            shootHoldTime = 0;
            isCharging = false;

            PlaySound(shootSound);
            PlayParticle(shootEffect, canonPos.position);
        }
    }

    public void DropBomb()
    {
        if (mineAmmo > 0)
        {
            Instantiate(minePrefab, minePos.position, Quaternion.identity);
            mineAmmo--;

            PlaySound(bombDropSound);
            PlayParticle(bombEffect, minePos.position);
        }
    }

    #endregion

    #region Gestion de la santé

    public void Hit(int damage)
    {
        health -= damage;
        PlaySound(damageSound);
        PlayParticle(damageEffect, transform.position);

        if (health <= 0)
        {
            Kill();
        }
    }

    public void Heal(int healValue)
    {
        health += healValue;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void Kill()
    {
        PlaySound(deathSound);
        PlayParticle(deathEffect, transform.position);
        OnTankDeath?.Invoke(playerID);
        Destroy(gameObject);
    }

    public void ResetStats()
    {
        health = maxHealth;
        ammo = 10;
        mineAmmo = 3;
        shootHoldTime = 0f;
        ActivePowerUp = "None";
        OnPowerUpChanged?.Invoke(ActivePowerUp);
        rb.velocity = Vector3.zero;
    }

    #endregion

    #region Gestion des pouvoirs

    public void ActivatePowerUp(PowerUpType powerUpType, float duration)
    {
        StartCoroutine(HandlePowerUp(powerUpType, duration));
    }

    private IEnumerator HandlePowerUp(PowerUpType powerUpType, float duration)
    {
        ActivePowerUp = powerUpType.ToString();
        OnPowerUpChanged?.Invoke(ActivePowerUp);

        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                baseSpeed *= 2;
                break;
            case PowerUpType.Invincibility:
                break;
            case PowerUpType.DoubleDamage:
                break;
        }

        yield return new WaitForSeconds(duration);

        ActivePowerUp = "None";
        OnPowerUpChanged?.Invoke(ActivePowerUp);

        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                baseSpeed /= 2;
                break;
        }
    }

    #endregion

    #region Sons et effets visuels

    private void UpdateEngineSound(float movementMagnitude)
    {
        bool currentlyMoving = movementMagnitude > 0.01f;

        if (currentlyMoving && !isMoving)
        {
            isMoving = true;
            PlayEngineDriving();
        }
        else if (!currentlyMoving && isMoving)
        {
            isMoving = false;
            PlayEngineIdle();
        }
    }

    private void PlayEngineIdle()
    {
        if (engineAudioSource.clip != engineIdleSound)
        {
            engineAudioSource.clip = engineIdleSound;
            engineAudioSource.Play();
        }
    }

    private void PlayEngineDriving()
    {
        if (engineAudioSource.clip != engineDrivingSound)
        {
            engineAudioSource.clip = engineDrivingSound;
            engineAudioSource.Play();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

    private void PlayParticle(ParticleSystem particle, Vector3 pos)
    {
        if (particle != null)
        {
            ParticleSystem instantiatedParticle = Instantiate(particle, pos, Quaternion.identity);
            instantiatedParticle.Play();
            Destroy(instantiatedParticle.gameObject, instantiatedParticle.main.duration);
        }
    }

    #endregion

}

public enum EPlayer
{
    Player1,
    Player2,
    Player3,
    Player4
}