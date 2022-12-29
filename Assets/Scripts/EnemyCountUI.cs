using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCountUI : MonoBehaviour
{

    [SerializeField] Sprite enemySprite;

    List<GameObject> enemyIcons;
    // Start is called before the first frame update
    void Start()
    {
        int enemyCount = FindObjectsOfType<EnemyMovement>().Length;
        enemyIcons = new List<GameObject>();
        DrawEnemyIcons(enemyCount);
        
    }

    // Update is called once per frame
    void Update()
    {
        int enemyCount = FindObjectsOfType<EnemyMovement>().Length;
        if (enemyIcons.ToArray().Length != enemyCount)
        {
            DrawEnemyIcons(enemyCount);
        }

        
    }

    void DrawEnemyIcons(int enemies)
    {
        foreach (GameObject icon in enemyIcons)
        {
            Destroy(icon);
        }
        enemyIcons = new List<GameObject>();

        for (int i = 0; i < enemies; i++)
        {
            enemyIcons.Add(new GameObject("Enemy Icon" + i));
            Image newEnemyIcon = enemyIcons[i].AddComponent<Image>();
            newEnemyIcon.sprite = enemySprite;
            enemyIcons[i].transform.SetParent(transform, false);
            RectTransform iconPos = enemyIcons[i].GetComponent<RectTransform>();
            iconPos.localPosition = new Vector2 (16 * i, 0);
            iconPos.localScale = new Vector2 (.2f, .2f);
        }
    }
}
