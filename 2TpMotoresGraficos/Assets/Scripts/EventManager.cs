using UnityEngine;
using UnityEngine.Events;
using System;


// funcion: evento personalizado que permite pasar dos enteros como parámetros
[Serializable]
public class Int2Event : UnityEvent<int, int> { }
public class EventManager : MonoBehaviour
{
    // funcion: referencia estática al EventManager actual (singleton)
    public static EventManager current;

    private void Awake()
    {
        // funcion: implementa el patrón singleton para asegurar que solo haya un EventManager activo
        if (current == null)
        {
            current = this; // funcion: asigna esta instancia como la actual si no existe otra
        }
        else if (current != this)
        {
            Destroy(this); // funcion: destruye esta instancia si ya existe otra
        }
    }

    // funcion: evento que puede ser invocado para actualizar la cantidad de balas (por ejemplo, en la UI)
    public Int2Event updateBulletsEvent = new Int2Event();
}
