using ChestSystem;
using UnityEngine;

namespace PlayerSystems
{
    public class ObjectInteractor : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer; 
    
        private bool _canInteract = false;
        private QuizChest _target;
        private void Update()
        {
            if (_target)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.transform.position + new Vector3(0.5f, 0.5f, 0));
                PlayerUI.Instance.InteractInfoUI.position = screenPos;
            }
            if (!_canInteract) return;
            if (!PlayerInputController.Instance.IsInteracting) return;
            PlayerUI.Instance.InteractInfoUI.gameObject.SetActive(false);
            if (_target)
            {
                _target.gameObject.layer = LayerMask.NameToLayer("Default");
                _target.transform.parent.gameObject.layer = LayerMask.NameToLayer("Default");
                foreach(Transform child in _target.transform)
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                QuizManager.Instance.BeginDisplayQuestion(_target);
                _target = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!CheckIfObjectIsInteractableLayer(other)) return;
            _canInteract = true;
            _target = other.GetComponentInChildren<QuizChest>();
            PlayerUI.Instance.InteractInfoUI.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!CheckIfObjectIsInteractableLayer(other)) return;
            PlayerUI.Instance.InteractInfoUI.gameObject.SetActive(false);
            _canInteract = false;
            _target = null;
        }

        private bool CheckIfObjectIsInteractableLayer(Collider other)
        {
            return (1 << other.gameObject.layer & targetLayer) != 0;
        }
    }
}
