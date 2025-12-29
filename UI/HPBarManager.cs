// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class HPBarManager : MonoBehaviour
// {
//     public Player player;
//     public RectTransform currentHPBar;
//     private float hpBarMaxLength;

//     // Start is called before the first frame update
//     void Start()
//     {
//         hpBarMaxLength = currentHPBar.sizeDelta.x;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         var (currentHP, maxHP) = player.GetHp();
//         currentHPBar.sizeDelta = new Vector2(hpBarMaxLength * (currentHP / maxHP), currentHPBar.sizeDelta.y);
//     }
// }
