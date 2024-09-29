using UnityEngine;
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
            confirmationPanel.SetActive(true);
        }

        void OnYesButtonClicked()
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

        void OnNoButtonClicked()
        {
            confirmationPanel.SetActive(false);
        }
    }
}
