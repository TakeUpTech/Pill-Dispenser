using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPerskBuild : MonoBehaviour
{
    public GameObject elementPrefab;
    public int yDelay;
    public string playerPrefsKey;
    private int currentNbOfElements = 1;
    private int previusNbOfElements;
    public float originX;
    public float originY;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(playerPrefsKey)) //Vérifie sur plusieurs builds ont déjà été créé sinon reste à 1
        {
            currentNbOfElements = PlayerPrefs.GetInt(playerPrefsKey); //DbD : NumberOfBuilds, Ow : NumberOfTeams
        }
        previusNbOfElements = currentNbOfElements; //Garde en mémoire la valeur pour la comparer lors d'ajouts ou de suppressions futures
        checkNumberOfBuilds();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentNbOfElements > previusNbOfElements)
        {
            previusNbOfElements = currentNbOfElements;
            createPerksBuild(currentNbOfElements-1);
        }
        else if(currentNbOfElements < previusNbOfElements)
        {
            previusNbOfElements = currentNbOfElements;
            if(currentNbOfElements != 0)
            {
                Destroy(GameObject.Find(elementPrefab.name.Split(' ')[0] + " (" + currentNbOfElements + ")"));  
            }
        }
    }

    private void createPerksBuild(int index)
    {
        GameObject perkClone;
        perkClone = Instantiate(elementPrefab, transform); //Créé un clone sur le modèle de elementPrefab
        perkClone.transform.name = elementPrefab.name.Split(' ')[0] + " (" + index + ")"; //Modifie son nom basé sur le nom de modèle seul l'index est changé
        perkClone.transform.parent = elementPrefab.transform.parent; //Indique de le clone doit avoir le même object parent que le modèle
        perkClone.transform.localPosition = new Vector2(originX, originY-yDelay*index); //Modification de sa position pour éviter les superposions de clones
    }

    private void checkNumberOfBuilds()
    {
        for(int index = 0; index < currentNbOfElements; index ++)
        {
            if(index != 0)
            {
                createPerksBuild(index); //Créé autant de build que le nombre récupéré précédemment
            }
        }  
    }

    public void AddBuild()
    {
        if(currentNbOfElements < 9){ //Nombre max de build sinon sort de la page
            currentNbOfElements++; //Augmente le nombre actuel de build de 1
            Debug.Log("Add --> " + currentNbOfElements);
        }
        PlayerPrefs.SetInt(playerPrefsKey, currentNbOfElements); //Met à jour le nombre de build
        
    }

    public void DeleteBuild()
    {
        if(currentNbOfElements > 1) //Nombre max de build sinon plus de modèle et superposion
        {
            currentNbOfElements--; //Réduit le nombre actuel de build de 1
            PlayerPrefs.DeleteKey(elementPrefab.name.Split(' ')[0] + "_" + currentNbOfElements); //Supprime la sauvegarde du build 
            Debug.Log("Remove --> " + currentNbOfElements);
        }
        PlayerPrefs.SetInt(playerPrefsKey, currentNbOfElements); //Met à jour le nombre de build
    }
}
