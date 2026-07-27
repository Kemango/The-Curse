using UnityEngine;

// Attach to the Main Camera (orthographic). Fills the screen vertically with `arenaHeight` world
// units on every aspect ratio, so there are never black bars on the top or bottom. Horizontal
// containment (so the player can't walk off the visible edge on narrow screens) is handled by the
// player position clamp in movement.cs, not by zooming the camera out.
[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class CameraFit : MonoBehaviour
{
    [Tooltip("World-unit height the camera always fills. Set this to your background's height so it covers top-to-bottom.")]
    public float arenaHeight = 5f;

    Camera cam;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        Fit();
    }

    void Update()
    {
        Fit();
    }

    void Fit()
    {
        if (cam == null || !cam.orthographic)
            return;

        // Half the vertical view = arenaHeight / 2, independent of aspect -> vertical always full,
        // no black bars top or bottom. Wider screens simply show more width (contained by the walls
        // and the player clamp); narrower screens show less.
        cam.orthographicSize = arenaHeight / 2f;
    }
}
