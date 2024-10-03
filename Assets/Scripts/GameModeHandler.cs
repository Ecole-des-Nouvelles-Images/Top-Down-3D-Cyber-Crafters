using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace characterSpawner
{
    public class GameModeHandler : MonoBehaviour
    {
        public GameObject playerPrefab;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("SpawnPlayers");
        }

        IEnumerator SpawnPlayers()
        {
            for(int i = 0; i < 4; i++)
            {
                GameObject spawner = GameObject.Find("Player " + (i + 1).ToString() + " Spawner");
                GameObject player = Instantiate(playerPrefab, spawner.transform.position, spawner.transform.rotation, spawner.transform ) ;
                /*player.transform.position = GameObject.Find("Player " + (i + 1).ToString() + " Spawner").transform.position;
                player.transform.rotation = GameObject.Find("Player " + (i + 1).ToString() + " Spawner").transform.rotation;*/
                player.GetComponent<PlayerController>().playerModelId = i switch
                {
                    0 => PlayerModelIndexer.Instance.player1ModelId,
                    1 => PlayerModelIndexer.Instance.player2ModelId,
                    2 => PlayerModelIndexer.Instance.player3ModelId,
                    3 => PlayerModelIndexer.Instance.player4ModelId,
                    _ => player.GetComponent<PlayerController>().playerModelId
                };

                if(MenuHelperFunctions.playersReady[i] == false)
                {
                    foreach(Transform transform in player.transform)
                    {
                        Destroy(transform.gameObject);
                        player.GetComponent<PlayerController>().enabled = false;
                        Destroy(player.gameObject);
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

}
