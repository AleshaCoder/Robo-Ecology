using UnityEngine;

public static class CameraVisibility
{
    private static Camera _camera = Camera.main;

    public static bool IsObjectVisible(this Camera @this, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
    }

    public static bool IsObjectVisible(Renderer renderer)
    {
        if (Vector3.SqrMagnitude(renderer.transform.position - _camera.transform.position) > 1700)
            return false;
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_camera), renderer.bounds);
    }
}
