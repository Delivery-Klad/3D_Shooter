using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeCamera : MonoBehaviour
{
    public GameObject ACOG_FP;
    public GameObject ACOG_TP;
    public List<GameObject> Scopes;
    System.Random rnd = new System.Random();

    private void Start()
    {
        int tmp = rnd.Next(0, Scopes.Count - 1);
        Scopes[tmp].SetActive(true);
    }
}
