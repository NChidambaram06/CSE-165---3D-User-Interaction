using UnityEngine;
using OculusSampleFramework;

public class WristTracker : MonoBehaviour
{
    public OVRSkeleton skeleton;

    void Update()
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) ||
            OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
            return;

        if (skeleton == null || skeleton.Bones.Count == 0)
            return;

        Transform indexTip = null;

        foreach (var bone in skeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                indexTip = bone.Transform;
                break;
            }
        }

        if (indexTip != null)
        {
            Vector3 origin = indexTip.position;
            Vector3 direction = indexTip.forward;
            direction.y = 0;  // flatten to XZ if needed
            direction.Normalize();

            Debug.DrawRay(origin, direction * 0.5f, Color.green);  // draws a 0.5m green ray in Scene view
        }
    }
}
