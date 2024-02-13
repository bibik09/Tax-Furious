using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    //Характеристики машины
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20f;

    //Для прыжков
    public SpriteRenderer carSpriteRenderer;
    public SpriteRenderer carShadowRenderer;
    public AnimationCurve jumpCurve;



    float accelerationInput = 0;
    float steeringInput = 0;

    float velositySvUp = 0;

    float rotationAngle = 0;

    bool isJumping = false;

    Rigidbody2D rb;
    Collider2D carCollider;

    public Timer timer;
    public Collider2D timeCollider;

    public Text winText;
    public Text loseText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        carCollider = GetComponentInChildren<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (!isJumping)
        {
            ApplyEngineForce();

            KillOrtogonalVelocity();

            ApplySteering();
        }
    }

    void ApplyEngineForce()
    {
        velositySvUp = Vector2.Dot(transform.up, rb.velocity);

        //проверка на максимальную скорость при езде вперед
        if (velositySvUp > maxSpeed && accelerationInput > 0)
            return;

        //Проверка на максимальную скорость при езде назад
        if (velositySvUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        //Проверка на максимальную скорость в другом направлении
        if (rb.velocity.sqrMagnitude > maxSpeed*(maxSpeed/2) && accelerationInput > 0)
            return;

        //Тормоза
        if (Input.GetKey(KeyCode.Space))
        {
            rb.drag = Mathf.Lerp(rb.drag, 3f, Time.fixedDeltaTime * 300);
            Debug.Log("Space key was pressed.");
        }
        else if (accelerationInput < 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3f, Time.fixedDeltaTime * 10);
        }
        else if (accelerationInput == 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3f, Time.fixedDeltaTime * 3);
        } 
        else rb.drag = 0;

        Vector2 EngineForceVector = transform.up * accelerationInput * accelerationFactor;

        rb.AddForce(EngineForceVector, ForceMode2D.Force);
    }

    //Повороты
    void ApplySteering()
    {
        float minSpeedBeforeAllowSteeringFactor = rb.velocity.magnitude / 5;

        minSpeedBeforeAllowSteeringFactor = Mathf.Clamp01(minSpeedBeforeAllowSteeringFactor);

        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowSteeringFactor;

        rb.MoveRotation(rotationAngle);
    }

    //предотвращение бесконечного дрифта
    void KillOrtogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);

        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    //Насколько быстро машина едет боком
    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rb.velocity);
    }

    //Следы колес
    public bool IsTireScreeching(out float lateralVelocity, out bool isBreacking)
    {
        lateralVelocity = GetLateralVelocity();
        isBreacking = false;

        if (isJumping)
            return false;

        //Если тормозит
        if ((accelerationInput < 0 || Input.GetKey(KeyCode.Space)) && velositySvUp >0)
        {
            isBreacking = true;
            return true;
        }

        //Если дрифт
        if (Mathf.Abs(GetLateralVelocity()) > 4f)
        {
            return true;
        }

        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public void Jump(float jumpHeightScale, float jumpPushScale)
    {
        if (!isJumping)
        {
            StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale));
        }
    }

    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale)
    {
        isJumping = true;

        float jumpStartTime = Time.time;
        float jumpDuration = rb.velocity.magnitude * 0.03f;

        jumpHeightScale = jumpHeightScale * rb.velocity.magnitude * 0.05f;
        jumpHeightScale = Mathf.Clamp(jumpHeightScale, 0.0f, 1.0f);

        //переносим машину и тень на слой повыше
        carShadowRenderer.sortingLayerName = "Jump";
        carSpriteRenderer.sortingLayerName = "Jump";

        //Отключаем коллайдер
        carCollider.enabled = false;

        rb.AddForce(rb.velocity.normalized * jumpPushScale * 8, ForceMode2D.Impulse);

        while (isJumping)
        {
            // Смотрим в каком моменте прыжка находимся от 0 до 1
            float jumpCompletedPersantage = (Time.time - jumpStartTime) / jumpDuration;
            jumpCompletedPersantage = Mathf.Clamp01(jumpCompletedPersantage);

            //увеличение размера спрайта машины при прыжке
            carSpriteRenderer.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletedPersantage) * jumpHeightScale;

            //тень
            carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale * 0.75f;

            //Смещение тени
            carShadowRenderer.transform.localPosition = new Vector3(1, 1, 0.0f) * 3 * jumpCurve.Evaluate(jumpCompletedPersantage) * jumpHeightScale;

            if (jumpCompletedPersantage == 1.0f)
                break;

            yield return null;
        }

        if (Physics2D.OverlapCircle(transform.position, 1.5f))
        {
            //если приземление на коллайдер
            isJumping = false;

            Jump(0.1f, 0.3f);
        }
        else
        {
            //Ресет спрайта машины
            carSpriteRenderer.transform.localScale = Vector3.one;

            //Ресет спрайта тени
            carShadowRenderer.transform.localPosition = Vector3.zero;
            carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale;

            //Возвращаем на нужный слой
            carShadowRenderer.sortingLayerName = "Car";
            carSpriteRenderer.sortingLayerName = "Car";

            //включаем коллайдер
            carCollider.enabled = true;

            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Jump"))
        {
            JumpData jumpData = collider2D.GetComponent<JumpData>();
            Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
        }

        if (collider2D.CompareTag("Client"))
        {
            //timer.timeValue += 30;
            timer.StopTime();
            timeCollider.enabled = false;
            if (timer.timeValue > 0)
            {
                winText.enabled = true;
            }
            else
            {
                loseText.enabled = true;
            }
        }
    }
}
