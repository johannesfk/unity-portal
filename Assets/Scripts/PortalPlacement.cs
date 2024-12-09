using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CameraMove))]
public class PortalPlacement : MonoBehaviour
{

    private PlayerInput playerInput;
    private InputAction fireBluePortalAction;
    private InputAction fireOrangePortalAction;

    [SerializeField]
    private PortalPair portals;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Crosshair crosshair;

    private CameraMove cameraMove;

    private void Awake()
    {
        cameraMove = GetComponent<CameraMove>();
        playerInput = GetComponent<PlayerInput>();        // Get references to the actions
        fireBluePortalAction = playerInput.actions["FireBluePortal"];
        fireOrangePortalAction = playerInput.actions["FireOrangePortal"];
        layerMask = 1 << LayerMask.NameToLayer("LevelGeom");
    }

    private void OnEnable()
    {
        fireBluePortalAction.performed += _ => FirePortal(0, transform.position, transform.forward, 250.0f);
        fireOrangePortalAction.performed += _ => FirePortal(1, transform.position, transform.forward, 250.0f);
    }

    private void OnDisable()
    {
        fireBluePortalAction.performed -= _ => FirePortal(0, transform.position, transform.forward, 250.0f);
        fireOrangePortalAction.performed -= _ => FirePortal(1, transform.position, transform.forward, 250.0f);
    }

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(pos, dir, out hit, distance, layerMask);

        // Add debug ray visualization
        Debug.DrawRay(pos, dir * distance, didHit ? Color.green : Color.red, 2.0f);

        // Add debug logging
        Debug.Log($"Raycast: Hit={didHit}, Distance={distance}, LayerMask={layerMask.value}");
        if (didHit)
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");
        }

        if (hit.collider != null)
        {
            Debug.Log("Hit " + hit.collider.name);
            // If we shoot a portal, recursively fire through the portal.
            if (hit.collider.tag == "Portal")
            {
                Debug.Log("Hit portal");
                var inPortal = hit.collider.GetComponent<Portal>();

                if (inPortal == null)
                {
                    return;
                }

                var outPortal = inPortal.OtherPortal;

                // Update position of raycast origin with small offset.
                Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                pos = outPortal.transform.TransformPoint(relativePos);

                // Update direction of raycast.
                Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
                relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
                dir = outPortal.transform.TransformDirection(relativeDir);

                distance -= Vector3.Distance(pos, hit.point);

                FirePortal(portalID, pos, dir, distance);

                return;
            }

            // Orient the portal according to camera look direction and surface direction.
            var cameraRotation = cameraMove.TargetRotation;
            var portalRight = cameraRotation * Vector3.right;

            if (Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            {
                portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
            }
            else
            {
                portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
            }

            var portalForward = -hit.normal;
            var portalUp = -Vector3.Cross(portalRight, portalForward);

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);

            // Attempt to place the portal.
            bool wasPlaced = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);

            if (wasPlaced)
            {
                Debug.Log("Placed portal");
                crosshair.SetPortalPlaced(portalID, true);
            }
        }
    }
}
