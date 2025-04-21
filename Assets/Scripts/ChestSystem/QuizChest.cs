using PlayerSystems;
using UnityEngine;

namespace ChestSystem
{
    public class QuizChest : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public void InteractWithChest(bool hasAnsweredCorrectly)
        {
            animator.Play(hasAnsweredCorrectly ? "Chest_Open_Close" : "Chest_Destroy", 0);

            if (hasAnsweredCorrectly)
                PlayerUI.Instance.GatherGem(this.transform);
        }
    
        public void DestroyChest()
        {
            PlayerUI.Instance.UpdateChestText();
            Destroy(transform.parent.gameObject);
        }
    }
}
