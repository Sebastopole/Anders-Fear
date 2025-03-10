using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Climbing : MonoBehaviour
{
    private int vaultLayer;
    public Camera cam;
    private float playerHeight = 2f;
    private float playerRadius = 0.5f;
    private KeyCode vaultKey = KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        vaultLayer = LayerMask.NameToLayer("Vault"); //On définie que le layer de vault est "Vault"
        vaultLayer = ~vaultLayer; //Les raycast n'intéragissent pas avec des layers, elles les ignorent. Donc on veut les cast qu'elles ignorent TOUT sauf les "Vault)
    }


    private void Vault()
    {
        if(Input.GetKeyDown(vaultKey))
        {
            //On lance un ray a la position de la cam / dans sa direction / ca stock le hit / de 1 de longueur / intéragit uniquement avec le vaultLayer
            if((Physics.Raycast(cam.transform.position, cam.transform.forward, out var firstHit, 1f, vaultLayer))) 
            {
                print("valuable in front");
                //On lance un ray cast a l'origine du firstHit (contre le mur) + forward le radius du joueur pour pas tombé + Up par la taille du player*0.6 pour pas qu'il monté partout / ...
                //... / on tire le ray vers le bas sur le sol puisque c'est la que le joueur bougera 
                if(Physics.Raycast(firstHit.point + (cam.transform.forward * playerRadius) + (Vector3.up * 0.6f * playerHeight), Vector3.down, out var secondHit, playerHeight))
                {
                    print("found place to land");
                    StartCoroutine(LerpVault(secondHit.point, 0.5f)); //On définit la target position et le temps que le joueur mets a monter (duration)
                }
            }
        }
    }

    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration) //Tant que le temps que le joueur mets a grimper (duration) est supérieur a time, le déplacement est émit
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time/duration); //On fait bouger le joueur depuis la startingPosition jusqu'a la targetPosition
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

    }

    // Update is called once per frame
    void Update()
    {
        Vault();
    }
}
