using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Upgrades
    private bool comboUp = true;
    private bool chargedAttackUp = true;
    private bool magicBeamUp = true;
    private bool damagingRollUp = true;
    private bool dashAttackUp = true;

    //Stats
    [SerializeField] public int maxHp;
    [SerializeField] private int attack;
    [SerializeField] public int damage;
    [SerializeField] private float dodgeIFrame;
    [SerializeField] private int speed;
    [SerializeField] private int maxShield;
    [SerializeField] private int currentShield;

    private Vector3 dashAttackDirection;

    private bool canDodge = true;
    private Vector3 dodgeDirection;
    private bool canMove = true;
    private bool canRotate = true;
    [System.NonSerialized] public bool isDodging = false;

    Coroutine dodgeIFrameCoroutine;

    //Input
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputMaster inputMaster;

    //Compenent
    [SerializeField] private Animator animator;

    //Instance
    public static PlayerMovement instance;

    //Prefabs
    [SerializeField] private GameObject magicBeamPrefab;

    //MagicBeam Spawn point
    public Transform magicBeamSpawnPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();

        animator.SetBool("Combo", comboUp);
        animator.SetBool("DashAttack", dashAttackUp);

        Application.targetFrameRate = 60;

        damage = attack;
    }

    void Update()
    {
        #region Rotation vers la souris
        if (canRotate)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
        #endregion

        #region Lire la valeur de Movement
        Movement(inputMaster.Player.Movement.ReadValue<Vector2>());
        #endregion
    }

    public void Movement(Vector2 inputValue)
    {
        if (canMove)
        {
            Vector3 dir = new Vector3(inputValue.x, inputValue.y);

            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    #region Attack functions
    public void Attack(InputAction.CallbackContext context)
    {
        //Si l'input est réalisé (dans ce cas ci, uniquement appuyé
        if (context.performed)
        {
            animator.SetTrigger("Attack");
        }
    }

    private bool isChargeFinished = false;
    public void ChargedAttack(InputAction.CallbackContext context)
    {
        animator.ResetTrigger("CancelCharge");
        if (chargedAttackUp)
        {
            if (context.started)
            {
                //Commencer la charge
                animator.SetTrigger("StartCharge");
            }
            else if (context.performed)
            {
                isChargeFinished = true;
            }
            else if (context.canceled)
            {

                if (isChargeFinished)
                {
                    //Lancer l'attaque
                    animator.SetTrigger("FinishCharge");
                    isChargeFinished = false;
                }
                else
                {
                    //Cancel l'attaque
                    animator.SetTrigger("CancelCharge");
                }
            }
        }
    }

    public void LaunchBeam()
    {
        if (magicBeamUp)
        {
            GameObject magicBeam = Instantiate(magicBeamPrefab, magicBeamSpawnPoint.position, transform.rotation);
            //Création d'un Vector3 contenant la distance entre la perso et la souris . ScreenToWorldPoint pour convertir la position de la souris à l'échelle de l'écran (et 0 en z sinon il fait n'imp)
            magicBeam.GetComponent<MagicBeam>().SetDirection(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - transform.position);
        }
    }

    #region Dash Attack
    public void DashAttackStart()
    {
        StopCoroutine(dodgeIFrameCoroutine);
        isDodging = false;
        animator.SetBool("Dodge", false);
        canDodge = true;

        canMove = false;
        canRotate = false;
        PlayerHealth.instance.canTakeDamage = false;
        dashAttackDirection = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0) - transform.position;
    }
    public void DashAttackMove()
    {
        transform.Translate(dashAttackDirection.normalized * 10 * Time.deltaTime, Space.World);
    }
    public void DashAttackEnd()
    {
        canMove = true;
        canRotate = true;
        PlayerHealth.instance.canTakeDamage = true;
    }
    #endregion
    #endregion

    #region Dodge
    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed && canDodge)
        {
            canDodge = false;
            isDodging = true;
            PlayerHealth.instance.canTakeDamage = false;
            canMove = false;
            animator.SetBool("Dodge", true);

            dodgeDirection = new Vector3(inputMaster.Player.Movement.ReadValue<Vector2>().x, inputMaster.Player.Movement.ReadValue<Vector2>().y);

            dodgeIFrameCoroutine = StartCoroutine(DodgeInvulnerabilityCD());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDodging && damagingRollUp)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                animator.SetTrigger("Bonk");
                StopCoroutine(DodgeInvulnerabilityCD());
                animator.SetBool("Dodge", false);
                collision.gameObject.GetComponent<Enemy>().health.TakeDamage(attack);
            }
            if (collision.gameObject.CompareTag("Box"))
            {
                animator.SetTrigger("Bonk");
                StopCoroutine(DodgeInvulnerabilityCD());
                animator.SetBool("Dodge", false);
                collision.gameObject.GetComponent<Box>().DestroyBox();
            }
        }
    }

    IEnumerator DodgeInvulnerabilityCD()
    {
        yield return new WaitForSeconds(dodgeIFrame);
        PlayerHealth.instance.canTakeDamage = true;
        canMove = true;
        isDodging = false;
        animator.SetBool("Dodge", false);
        yield return new WaitForSeconds(1);
        canDodge = true;
    }

    public void DodgeMove()
    {
        transform.Translate(dodgeDirection.normalized * speed * Time.deltaTime, Space.World);
    }

    public void BonkRecoil()
    {
       transform.Translate(-dodgeDirection.normalized * speed * 2 * Time.deltaTime, Space.World);
        isDodging = false;
    }
    #endregion

    #region Get/Set
    public int GetAttack()
    {
        return attack;
    }
    #endregion

    #region Weapon
    public void DoubleDamage()
    {
        damage *= 2;
    }

    public void HalfDamage()
    {
        damage /= 2;
    }
    #endregion

    public void Death()
    {
        canDodge = false;
        canMove = false;
        canRotate = false;
    }
}
