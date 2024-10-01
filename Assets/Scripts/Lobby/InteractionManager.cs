using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby
{
    public class InteractionManager : MonoBehaviour
    {
        public GameObject confirmationPanel; // Référence au panneau de confirmation
        public Button yesButton; // Référence au bouton "Oui"
        public Button noButton; // Référence au bouton "Non"
        public string characterSelectionScene;

        public EventSystem EventSystem;

        private void Start()
        {
            // Désactiver le panneau de confirmation au démarrage
            confirmationPanel.SetActive(false);

            // Ajouter des écouteurs d'événements aux boutons
            yesButton.onClick.AddListener(OnYesButtonClicked);
            noButton.onClick.AddListener(OnNoButtonClicked);
        }

        public void HandleInteraction(Collider other, bool buttonAPressed)
        {
            if (other.GetComponent<InterractiveItem>())
            {
                if (other.GetComponent<InterractiveItem>().PlayButton)
                {
                    if (buttonAPressed)
                    {
                        Debug.Log("Play");
                        SceneManager.LoadScene(characterSelectionScene);
                    }
                }

                if (other.GetComponent<InterractiveItem>().OptionButton)
                {
                    if (buttonAPressed)
                    {
                        Debug.Log("Settings");
                    }
                }

                if (other.GetComponent<InterractiveItem>().QuitButton)
                {
                    if (buttonAPressed)
                    {
                        Debug.Log("Quit");
                        ShowConfirmationPanel();
                    }
                }
            }
        }

        void ShowConfirmationPanel()
        {
            // Mettre le jeu en pause
            Time.timeScale = 0;

            // // Désactiver les contrôles du joueur
            // isPaused = true;

            // Afficher le panneau de confirmation
            confirmationPanel.SetActive(true);
            
            EventSystem.SetSelectedGameObject(noButton.gameObject);
        }

        void ResumeGame()
        {
            Time.timeScale = 1;
            confirmationPanel.SetActive(false);
        }
       public void OnYesButtonClicked()
        {
            Debug.Log("Quitting the game");
            if (Application.isEditor)
            {
                Debug.Log("Quitting the game (in editor mode)");
            }
            else
            {
                Application.Quit();
            }
        }

        public void OnNoButtonClicked()
        {
            ResumeGame();
        }
    }
}
