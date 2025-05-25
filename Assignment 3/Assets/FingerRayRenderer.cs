using UnityEngine;
using OculusSampleFramework;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class FingerRayRenderer : MonoBehaviour
{
    public OVRSkeleton skeleton;
    public GameObject characterPrefab;
    public float rayLength = 1.0f;
    public float moveSpeed = 1.5f;
    public float moveThreshold = 0.1f;  // Only run if pointing clearly

    private LineRenderer lineRenderer;
    private GameObject character;
    private Animator animator;

    private bool isTurningBack = false;

    private wallCollide collisionScript;

    void Start()
    {
        // Line Renderer setup
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.005f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        // Character setup
        if (characterPrefab != null)
        {
            character = Instantiate(characterPrefab);
            character.SetActive(false);
            animator = character.GetComponent<Animator>();
            collisionScript = character.GetComponent<wallCollide>();
        }
    }

    void Update()
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) ||
            OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            lineRenderer.enabled = false;
            if (character != null) character.SetActive(false);
            return;
        }

        if (skeleton == null || skeleton.Bones.Count == 0)
        {
            lineRenderer.enabled = false;
            if (character != null) character.SetActive(false);
            return;
        }

        Transform indexTip = null;
        foreach (var bone in skeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                indexTip = bone.Transform;
                break;
            }
        }

        if (indexTip == null) return;

        Vector3 origin = indexTip.position;
        Vector3 direction = indexTip.forward;
        direction.y = 0;
        float magnitude = direction.magnitude;

        // Only move if ray is clearly pointing
        bool shouldRun = magnitude > moveThreshold;
        direction.Normalize();

        // Draw Ray
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + direction * rayLength);

        if (character != null)
        {
            character.SetActive(true);

            if (shouldRun && !collisionScript.hasCollidedWithWall && !isTurningBack)
            {
                // Rotate and move
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                //character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, Time.deltaTime * 5f);
                //character.transform.position += direction * moveSpeed * Time.deltaTime;
                Rigidbody rb = character.GetComponent<Rigidbody>();
                rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

                // Compute target position
                Vector3 newPosition = rb.position + direction * moveSpeed * Time.deltaTime;

                // Compute target rotation to face movement direction
                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 5f);

                    // Apply rotation and movement
                    rb.MoveRotation(smoothRotation);
                }

                rb.MovePosition(newPosition);

                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
                Debug.Log("test collision - running stopped");

                //// Rotate character 180 degrees
                //if (character != null)
                //{
                //    Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                //    Rigidbody rb = character.GetComponent<Rigidbody>();
                //    Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 5f);
                //    rb.MoveRotation(smoothRotation);
                //}

                // Start coroutine to reset flag after 0.5 seconds
                StartCoroutine(ResetCollisionFlagAfterDelay(0.5f, direction));
            }
        }
    }

    private IEnumerator ResetCollisionFlagAfterDelay(float delay, Vector3 direction)
    {
        isTurningBack = true;

        // Rotate character 180 degrees
        if (character != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
            Rigidbody rb = character.GetComponent<Rigidbody>();
            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 5f);
            rb.MoveRotation(smoothRotation);
        }

        yield return new WaitForSeconds(delay);
        collisionScript.hasCollidedWithWall = false;
        isTurningBack = false;
    }


}
