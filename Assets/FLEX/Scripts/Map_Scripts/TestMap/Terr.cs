using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terr : MonoBehaviour
{
    private Terrain terr;
    private byte[,] splatIndex;//итоговый 2d массив с индексами максимально воздействующих текстур
    private Vector3 size;//размер земли
    private Vector3 tPos;//положение земли
    private int width;//размер текстуры маски
    private int height;

    void Start()
    {
        terr = GetComponent<Terrain>();//получение ссылки на землю
        CalcHiInflPrototypeIndexesPerPoint();//вызов метода подготавливающего итоговый массив
    }

    private void CalcHiInflPrototypeIndexesPerPoint()
    {
        TerrainData terrainData = terr.terrainData;//структура с большей частью земельных танных
        size = terrainData.size;//                         |
        width = terrainData.alphamapWidth;//      |просто копирование из земли в переменные
        height = terrainData.alphamapHeight;//    |
        int prototypesLength = terrainData.splatPrototypes.Length;//те самые прототипы (кол-во текстур на земле)
        tPos = terr.GetPosition();//      |просто копирование из земли в переменные

        float[,,] alphas = terrainData.GetAlphamaps(0, 0, width, height);//копирование массива с силами воздействия каждой теустуры
        splatIndex = new byte[width, height];//инициализация массива

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                byte ind = 0;//индекс последней текстуры с максимальным воздействием
                float t = 0f;//последняя наибольшая сила воздействия
                for (byte i = 0; i < prototypesLength; i++)
                {
                    if (alphas[x, y, i] > t)
                    {
                        t = alphas[x, y, i];
                        ind = i;
                    }
                }
                splatIndex[x, y] = ind;
            }
        }
    }

    public int GetMaterialIndex(Vector3 pos)
    {
        pos = pos - tPos;
        pos.x /= size.x;
        pos.z /= size.z;
        return splatIndex[(int)(pos.z * (width - 1)), (int)(pos.x * (height - 1))];
    }
}
