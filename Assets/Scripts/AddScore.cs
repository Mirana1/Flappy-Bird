using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Score.score++;
    }
}

