using UnityEngine;
using UnityEngine.SceneManagement;

public class GrassZone : MonoBehaviour
{
    [Header("Paramètres de rencontre")]
    [SerializeField] private int chanceDeCombat = 10; // 1 chance sur 10
    [SerializeField] private float delaiEntreTentatives = 1f;

    private bool joueurEstDedans = false;
    private float timer = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            joueurEstDedans = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            joueurEstDedans = false;
    }

    private void Update()
    {
        if (joueurEstDedans)
        {
            timer += Time.deltaTime;
            if (timer >= delaiEntreTentatives)
            {
                timer = 0f;

                // Vérifie si un combat se lance
                int chance = Random.Range(1, chanceDeCombat + 1);
                if (chance == 1)
                {
                    Debug.Log("⚔️ Un combat se lance !");
                    SceneTransition.instance.FadeToScene("CombatScene");
                }
            }
        }
    }
}
