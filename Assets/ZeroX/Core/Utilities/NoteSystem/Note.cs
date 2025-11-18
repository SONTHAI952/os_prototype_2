using System;
using UnityEngine;

namespace ZeroX.Utilities.NoteSystem
{
    public class Note : MonoBehaviour
    {
        [TextArea(3, 10000)]
        [SerializeField] private string content;

        [NonSerialized] public bool editing;
    }
}