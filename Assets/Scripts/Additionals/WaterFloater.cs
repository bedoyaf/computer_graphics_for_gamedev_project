using UnityEngine;

public class WaterFloater : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1.0f;
    public float speed = 1.0f;

    private Vector3 position;

    private void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        // Získáme pozici lodi
        Vector3 pos = transform.position;

        // TADY MUSÍ BÝT STEJNÁ MATEMATIKA JAKO V SHADERU
        // Pokud máš jednoduchý Sinus:
        float waveHeight = Mathf.Sin(pos.x * frequency + Time.time * speed) * amplitude;

        // Nastavíme loï na hladinu
        pos.y += waveHeight;
        transform.position = pos;
    }
}
