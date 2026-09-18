using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(PlayerInputReader))]

[RequireComponent(typeof(Animator))]
public class PlayerCombatController : MonoBehaviour
{

    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private PlayerHitboxDetector hitboxDetector;

    [SerializeField]
    private TextMeshProUGUI killText;


    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    private static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");
    private PlayerInputReader inputReader;
    private PlayerMovementController movementController;
    private Animator animator;

    private bool isAttacking;         
    private bool comboWindowOpen;      
    private bool queuedNextAttack;    
    private int currentComboIndex = -1;
    
    private const int COMBO_LENGTH = 3; 

    void Start()
    {
        inputReader = GetComponent<PlayerInputReader>();
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementController>();


        inputReader.OnPlayerAttack += OnAttackInput;
    }

    void OnDestroy()
    {
        inputReader.OnPlayerAttack -= OnAttackInput;

    }


    private void OnAttackInput()
    {
        if (!isAttacking)
        {
            StartAttack(0);
            animator.SetLayerWeight(1, 1);
        }
        else if (comboWindowOpen)
        {
            queuedNextAttack = true;
        }
    }

    private void StartAttack(int index)
    {
        if(index > 2)
            index = 0;

        currentComboIndex = index;
        isAttacking = true;
        comboWindowOpen = false;
        queuedNextAttack = false;

        animator.SetInteger(AttackIndexHash, currentComboIndex);
        animator.SetBool(IsAttackingHash, true); 
    }


    public void OnComboWindowOpen()
    {
        comboWindowOpen = true;
        ApplyDamage();
    }

    public void OnAttackAnimationEnd()
    {
        comboWindowOpen = false;

        if (queuedNextAttack && currentComboIndex < COMBO_LENGTH - 1)
        {
            StartAttack(currentComboIndex + 1);
        }
        else
        {
            EndCombo();
        }
    }

    private void EndCombo()
    {
        isAttacking = false;
        comboWindowOpen = false;
        queuedNextAttack = false;
        currentComboIndex = -1;
        animator.SetInteger(AttackIndexHash, 0);
        animator.SetBool(IsAttackingHash, false);
        animator.SetLayerWeight(1, 0);

    }

    public void CancelCombo()
    {
        EndCombo();
    }

    public void ApplyDamage()
    {
        if(hitboxDetector != null)
        {
            List<EnemyHealthComponent> healthComponents = hitboxDetector.GetTargetHealthComponents();

            foreach(EnemyHealthComponent healthComponent in healthComponents)
            {
                healthComponent.TakeDamage();
                Debug.Log("Applying damage");
            }
        }
        else
        {
            Debug.Log("hitbox detector is null");
        }
    }

}