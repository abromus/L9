using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _player.Damaged += OnDamaged;
        _player.Cured += OnCured;
    }

    private void OnDisable()
    {
        _player.Damaged -= OnDamaged;
        _player.Cured -= OnCured;
    }

    private void OnDamaged(int damage)
    {
        _animator.SetTrigger(AnimatorPlayerController.Params.Damage);
    }

    private void OnCured(int health)
    {
        _animator.SetTrigger(AnimatorPlayerController.Params.Cure);
    }
}
