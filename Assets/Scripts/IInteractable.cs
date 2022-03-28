using UnityEngine.InputSystem;

public interface IInteractable
{
    void OnMove(InputValue value);
    void OnJump(InputValue value);
    void OnFire(InputValue value);
}
