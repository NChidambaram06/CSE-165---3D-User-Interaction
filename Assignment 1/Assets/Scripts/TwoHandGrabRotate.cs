using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabRotate : XRGrabInteractable
{
    private IXRSelectInteractor secondInteractor = null;

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (interactorsSelecting.Count > 1)
        {
            Transform primary = interactorsSelecting[0].transform;
            Transform secondary = interactorsSelecting[1].transform;

            // Rotate based on two-hand vector
            Vector3 direction = secondary.position - primary.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (interactorsSelecting.Count == 2)
            secondInteractor = interactorsSelecting[1];
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        secondInteractor = null;
    }
}